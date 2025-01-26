using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemLogic : MonoBehaviour
{
    public GameEvent gameEvent;
    public int eventRaiseID;

    public Image itemImage;
    public TMP_Text itemPrice;

    public int currentPrice;
    public int inflation;

    void OnEnable()
    {
        itemPrice.text = currentPrice.ToString();
        Time.timeScale = 0;
    }

    public void BuyLogic()
    {
        if( currentPrice <= 100000)
        {
            currentPrice += inflation;
            itemPrice.text = currentPrice.ToString();
            gameEvent.Raise(eventRaiseID);
        }
    }
}
