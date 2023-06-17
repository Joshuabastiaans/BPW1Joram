using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{

    [Header("References")]
    public GunData gunData;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform firePoint;
    
    public Animator animator;

    private InputAction fire;
    private InputAction reload;

    float timeSinceLastShot;

    [SerializeField] private PlayerInputSystem playerControls;

    private AudioManager audioManager;

    private void Awake()
    {
        playerControls = new PlayerInputSystem();
        audioManager = FindObjectOfType<AudioManager>();

        gunData.currentAmmo = gunData.magSize;
    }

    private void OnEnable()
    {
        fire = playerControls.Player.Fire;
        fire.Enable();
        fire.performed += Shoot;
        reload = playerControls.Player.Reload;
        reload.Enable();
        reload.performed += StartReload;
    }

    private void OnDisable()
    {
        gunData.reloading = false;
        fire.Disable();
        reload.Disable();
    }

    public void StartReload(InputAction.CallbackContext context)
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;
        audioManager.Play("Reload");

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;

    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Shoot(InputAction.CallbackContext context)
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                GameObject bullet = Instantiate(gunData.BulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(firePoint.up * gunData.bulletSpeed, ForceMode2D.Impulse);
                bullet.GetComponent<Bullet>().SetDamage(gunData.damage);

                animator.SetTrigger("Fire");

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                audioManager.Play(gunData.shootSound);
                StartCoroutine(SpawnMuzzleFlash(firePoint));
            }
        }
    }

    private IEnumerator SpawnMuzzleFlash(Transform spawnPoint)
    {
        Quaternion muzzleRotation = spawnPoint.rotation * Quaternion.Euler(0f, 0f, 90f);
        Vector3 muzzleLocation = spawnPoint.position + spawnPoint.up * .5f;
        GameObject muzzleFlash = Instantiate(gunData.MuzzleflashPrefab, muzzleLocation, muzzleRotation);
        yield return new WaitForSeconds(.1f);
        Destroy(muzzleFlash);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }
}