using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIBars : MonoBehaviour
{
    [SerializeField] private PlayerHp_Xp_Scriptable _playerHp_Xp_SO;
    [SerializeField] private GameEventListener<CustomEvent> _onUpdatePlayerUI;
    [SerializeField] private Image _hpBar;

    private void Awake()
    {
        //EventManager.OnUpdatePlayerBarsEvent += UpdateBarsValues;
        _onUpdatePlayerUI.AddListener(UpdateBarsValues);
    }
    private void OnDestroy()
    {
        //EventManager.OnUpdatePlayerBarsEvent -= UpdateBarsValues;
        _onUpdatePlayerUI.RemoveListener(UpdateBarsValues);
    }

    private void UpdateBarsValues()
    {
        _hpBar.fillAmount = (float) _playerHp_Xp_SO.currentHp / _playerHp_Xp_SO.maxHp;
    }
}
