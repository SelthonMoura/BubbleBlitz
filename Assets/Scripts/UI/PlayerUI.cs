using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private GameEventListener<CustomEvent> _onUpdatePlayerUIEvent;
    [SerializeField] private TMP_Text _playerLivesTxt;
    [SerializeField] private TMP_Text _playerPoints;

    private void Awake()
    {
        //EventManager.OnUpdatePlayerBarsEvent += UpdateBarsValues;
        _onUpdatePlayerUIEvent.AddListener(UpdateValues);
    }
    private void OnDestroy()
    {
        //EventManager.OnUpdatePlayerBarsEvent -= UpdateBarsValues;
        _onUpdatePlayerUIEvent.RemoveListener(UpdateValues);
    }

    private void UpdateValues()
    {
        _playerLivesTxt.text = "x" + _playerStats.currentHp;
        _playerPoints.text = _playerStats.score + " pts";
    }
}
