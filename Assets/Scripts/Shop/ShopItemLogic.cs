using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemLogic : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private ItemPriceSO _itemPriceSO;
    [SerializeField] private GameEventListener<CustomEvent> _gameOverEvent;
    [SerializeField] private GameEventListener<CustomEvent> _gameWinEvent;
    public GameEvent gameEvent;
    public GameEvent updateUI;
    public int eventRaiseID;

    public Image itemImage;
    public TMP_Text itemPrice;

    public int inflation;
    [SerializeField] private bool _compraUnica;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _gameOverEvent.AddListener(ResetInflation);
        _gameWinEvent.AddListener(ResetInflation);
        //ResetInflation();
    }

    private void OnDestroy()
    {
        _gameOverEvent.RemoveListener(ResetInflation);
        _gameWinEvent.RemoveListener(ResetInflation);
    }

    void OnEnable()
    {
        //itemPrice.text = currentPrice.ToString();
        itemPrice.text = _itemPriceSO.inflammedPrice.ToString();
        Time.timeScale = 0;
    }

    public void BuyLogic()
    {
        if (_itemPriceSO.inflammedPrice <= _playerStats.score)
        {
            _playerStats.score -= _itemPriceSO.inflammedPrice;
            updateUI.Raise();
            _itemPriceSO.inflammedPrice += inflation;
            itemPrice.text = _itemPriceSO.inflammedPrice.ToString();
            gameEvent.Raise(eventRaiseID);

            if (_compraUnica)
                _button.interactable = false;
            
            itemPrice.text = _itemPriceSO.inflammedPrice.ToString();
        }
    }

    private void ResetInflation()
    {
        Debug.Log("reset inflation");
        _itemPriceSO.ResetInflation();
    }
}
