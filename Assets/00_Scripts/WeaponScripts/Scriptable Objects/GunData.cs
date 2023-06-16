using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{

    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;

    [Header("Shotgun")]
    public bool shotGunBulletEnabled;
    public float bulletSpread;
    public float bulletSpeed;
    public int bulletPelletsAmount;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    [Tooltip("In RPM")] public float fireRate;
    public float reloadTime;
    [HideInInspector] public bool reloading;

    [Header("Visuals")]
    public GameObject BulletPrefab;
    public GameObject MuzzleflashPrefab;
}