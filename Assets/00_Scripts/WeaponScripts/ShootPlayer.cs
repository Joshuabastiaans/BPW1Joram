using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootPlayer : MonoBehaviour
{
    [SerializeField] private PlayerInputSystem playerControls;

    [Header("References")]
    [SerializeField] private Transform firePointPistol;
    [SerializeField] private Transform firePointShotgun;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletForce = 20f;
    [SerializeField] private GameObject bulletMuzzleFlashPrefab;

    private InputAction shoot;
    private InputAction reload;

    [SerializeField] private GunData pistolGunData;
    [SerializeField] private GunData shotgunGunData;

    public Animator animator;

    private int currentGun;
    private void Awake()
    {
        playerControls = new PlayerInputSystem();
    }

    private void OnEnable()
    {
        shoot = playerControls.Player.Fire;
        shoot.Enable();
        shoot.performed += Fire;
        reload = playerControls.Player.Reload;
        reload.Enable();
        reload.performed += Reload;
    }

    private void OnDisable()
    {
        shoot.Disable();
        reload.Disable();
    }

    public void SetGun(int Weapon)
    {
        currentGun = Weapon;
    }

    private void Fire(InputAction.CallbackContext context)
    {


        switch (currentGun)
        {
            case 0:

                //shoot gun 1
                GameObject pistolBullet = Instantiate(pistolGunData.BulletPrefab, firePointPistol.position, firePointPistol.rotation);
                Rigidbody2D rbP = pistolBullet.GetComponent<Rigidbody2D>();
                rbP.AddForce(firePointPistol.up * bulletForce, ForceMode2D.Impulse);
                StartCoroutine(SpawnMuzzleFlash(firePointPistol));
                Debug.Log(firePointPistol);

                animator.SetTrigger("Fire");
                Debug.Log("Fired");

                break;

            case 1:
                //shoot gun 2
                GameObject shotgunBullet = Instantiate(shotgunGunData.BulletPrefab, firePointPistol.position, firePointPistol.rotation);
                Rigidbody2D rbS = shotgunBullet.GetComponent<Rigidbody2D>();
                rbS.AddForce(firePointPistol.up * bulletForce, ForceMode2D.Impulse);
                StartCoroutine(SpawnMuzzleFlash(firePointPistol));

                animator.SetTrigger("Fire");
                Debug.Log("Fireds");

                break;

        }
    }

    private IEnumerator SpawnMuzzleFlash(Transform spawnPoint)
    {
        GameObject muzzleFlash = Instantiate(bulletMuzzleFlashPrefab, spawnPoint.position, spawnPoint.rotation);
        yield return new WaitForSeconds(.2f);
        Destroy(muzzleFlash);
    }

    private void Reload(InputAction.CallbackContext context)
    {

    }
}
