using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private GameEventListener<CustomEvent> _onPlayerTakeDamageEvent;
    [SerializeField] private GameEvent _onUpdatePlayrUITrigger;
    [SerializeField] private GameEvent _onRestartLevelTrigger;
    [SerializeField] private GameEvent _onGameOverTrigger;
    [SerializeField] private GameEvent _onStopTimeTrigger;
    [SerializeField] private GameEvent _onResumeTimeTrigger;
    [SerializeField] private float _damageDelay;
    private bool _canTakeDamage;

    private void Start()
    {
        _onPlayerTakeDamageEvent.AddListener(PlayerTakeDamage);

        _onUpdatePlayrUITrigger.Raise();
    }

    private void OnDestroy()
    {
        _onPlayerTakeDamageEvent.RemoveListener(PlayerTakeDamage);
    }

    private void PlayerTakeDamage()
    {
        _playerStats.currentHp -= 1;

        StartCoroutine(DamageCoroutine());
    }

    private IEnumerator DamageCoroutine()
    {
        _onUpdatePlayrUITrigger.Raise();
        _onStopTimeTrigger.Raise();

        yield return new WaitForSecondsRealtime(_damageDelay);

        if(_playerStats.currentHp <= 0)
        {
            _onGameOverTrigger.Raise();
        }
        else
        {
            _onResumeTimeTrigger.Raise();
            _onRestartLevelTrigger.Raise();
        }
    }
}
