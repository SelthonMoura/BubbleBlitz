using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item List")]
public class ItemList : ScriptableObject
{
    public List<WeaponStats> weapons;
    public List<ItemSO> items;
}
