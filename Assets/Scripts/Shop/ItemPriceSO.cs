using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemPriceSO")]
public class ItemPriceSO : ScriptableObject
{
    public int baseValue;
    public int inflammedPrice;

    public void ResetInflation()
    {
        inflammedPrice = baseValue;
    }
}
