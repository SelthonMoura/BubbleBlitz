using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWeaponLogic : MonoBehaviour
{
    public WeaponStats weaponStats;

    public Image weaponImage;
    public TMP_Text weaponName;
    public TMP_Text weaponPrice;

    public int currentPrice;

    void OnEnable()
    {
        weaponImage.sprite = weaponStats.weaponSprite;
        weaponName.text = weaponStats.name;
        weaponPrice.text = weaponStats.weaponPrice.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
