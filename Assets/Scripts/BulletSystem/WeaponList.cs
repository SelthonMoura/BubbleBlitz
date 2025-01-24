using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon List")]
public class WeaponList : ScriptableObject
{
    public List<WeaponStats> weapons;
}
