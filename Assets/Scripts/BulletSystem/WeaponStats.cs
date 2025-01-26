using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    [Header("Prefab")]
    public GameObject bulletPrefab;

    [Header("Shop")]
    public Sprite weaponSprite;
    public int weaponPrice;
    public int inflation;

    [Header("Stats")]
    public int damage;
    public float fireRate;
    public float shotSpeed;
    public int pierce;
    public int bulletAmount;
    public int bulletLimit;
    public bool aimed;
    public bool persistLine;
    public string shotSFX;
}
