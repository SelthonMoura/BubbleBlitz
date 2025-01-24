using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private GameEventListener<CustomEvent> _onPlayerTakeDamage;
    [SerializeField] private GameEvent _onUpdatePlayrUITrigger;

    private void Awake()
    {
        _onPlayerTakeDamage.AddListener(PlayerTakeDamage);
    }

    private void Start()
    {
        _playerStats.currentHp = _playerStats.maxHp;
        _onUpdatePlayrUITrigger.Raise();
    }

    private void OnDestroy()
    {
        _onPlayerTakeDamage.RemoveListener(PlayerTakeDamage);
    }

    private void PlayerTakeDamage()
    {
        _playerStats.currentHp -= 1;
        Debug.Log("player damage");

        _onUpdatePlayrUITrigger.Raise();

        if(_playerStats.currentHp <= 0)
        {
            // trigger game over
        }
    }
}
