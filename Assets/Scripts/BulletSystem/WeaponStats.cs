using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Stats")]
public class WeaponStats : ScriptableObject
{
    public GameObject bulletPrefab;
    public int damage;
    public float fireRate;
    public float shotSpeed;
    public int pierce;
    public int bulletAmount;
    public int bulletLimit;
    public bool aimed;
    public bool persistLine;
}
