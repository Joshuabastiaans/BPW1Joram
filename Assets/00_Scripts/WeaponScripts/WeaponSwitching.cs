using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    [SerializeField] private PlayerInputSystem playerControls;

    private InputAction ScrollWeapon;
    private InputAction SelectWeapon;

    public switchAnimation switchAnimation;
    public ShootPlayer shootPlayer;
    private void Awake()
    {
        shootPlayer = GetComponent<ShootPlayer>();
        playerControls = new PlayerInputSystem();

        ScrollWeapon = playerControls.Player.ScrollWeapon;
        SelectWeapon = playerControls.Player.SelectWeapon;
    }

    private void OnEnable()
    {
        ScrollWeapon.Enable();
        SelectWeapon.Enable();

        ScrollWeapon.performed += OnScroll;
        SelectWeapon.performed += OnSelect;
    }

    private void OnDisable()
    {
        ScrollWeapon.Disable();
        SelectWeapon.Disable();
    }

    private void Start()
    {
        Select(selectedWeapon);

        timeSinceLastSwitch = 0f;
    }


    private void OnScroll(InputAction.CallbackContext context)
    {
        float scrollInput = context.ReadValue<float>();
        int previousSelectedWeapon = selectedWeapon;

        if (scrollInput > 0)
        {
            selectedWeapon = (selectedWeapon + 1) % weapons.Length;
        }
        else if (scrollInput < 0)
        {
            selectedWeapon--;
            if (selectedWeapon < 0)
            {
                selectedWeapon = weapons.Length - 1;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            Select(selectedWeapon);
        }

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        int numberInput = (int)context.ReadValue<float>();
        int weaponIndex = numberInput - 1;

        if (weaponIndex >= 0 && weaponIndex < weapons.Length)
        {
            selectedWeapon = weaponIndex;
            Select(selectedWeapon);
        }

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == weaponIndex);
            switchAnimation.SwitchAnimator(selectedWeapon);
            shootPlayer.SetGun(selectedWeapon);
        }
        timeSinceLastSwitch = 0f;
    }
}
