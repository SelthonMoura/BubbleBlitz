using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerHp_Xp_Scriptable _playerHp_Xp_SO;
    [SerializeField] private GameEventListener<CustomEvent> _onPlayerTakeDamage;
    [SerializeField] private GameEvent _onUpdatePlayrUITrigger;

    private void Awake()
    {
        _onPlayerTakeDamage.AddListener(PlayerTakeDamage);
    }

    private void Start()
    {
        _playerHp_Xp_SO.currentHp = _playerHp_Xp_SO.maxHp;
        _onUpdatePlayrUITrigger.Raise();
    }

    private void OnDestroy()
    {
        _onPlayerTakeDamage.RemoveListener(PlayerTakeDamage);
    }

    private void PlayerTakeDamage()
    {
        _playerHp_Xp_SO.currentHp -= 1;
        Debug.Log("player damage");

        _onUpdatePlayrUITrigger.Raise();

        if(_playerHp_Xp_SO.currentHp <= 0)
        {
            // trigger game over
        }
    }
}
