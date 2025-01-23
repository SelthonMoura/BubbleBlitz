using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float fireRate;
    [SerializeField] private float shotSpeed;
    [SerializeField] private int pierce;
    [SerializeField] private int bulletAmount;
    [SerializeField] private int bulletLimit;

    public int Damage { get => damage; set => damage = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float ShotSpeed { get => shotSpeed; set => shotSpeed = value; }
    public int Pierce { get => pierce; set => pierce = value; }
    public int BulletAmount { get => bulletAmount; set => bulletAmount = value; }
    public int BulletLimit { get => bulletLimit; set => bulletLimit = value; }
}
