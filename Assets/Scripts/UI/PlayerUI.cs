using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private GameEventListener<CustomEvent> _onUpdatePlayerUIEvent;
    [SerializeField] private GameEventListener<CustomEvent> _weaponChangeEvent;
    [SerializeField] private TMP_Text _playerLivesTxt;
    [SerializeField] private TMP_Text _playerPoints;
    [SerializeField] private TMP_Text _playerBombs;

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
        _playerLivesTxt.text = $"<size=42>x</size><color=black>{_playerStats.currentHp}</color>";
        _playerPoints.text = _playerStats.score + " pts";
        _playerBombs.text = $"Flash Bomb <size=25>x</size><color=red>{_playerStats.bombs}</color>";
    }
}
