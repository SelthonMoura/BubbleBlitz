using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public GameObject shopPanel;

    public void CloseShop()
    {
        Time.timeScale = 1.0f;
        shopPanel.SetActive(false);
    }
}
