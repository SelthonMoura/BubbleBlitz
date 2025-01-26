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
    private float _invincibilityTimer;

    private void Start()
    {
        _onPlayerTakeDamageEvent.AddListener(PlayerTakeDamage);

        _onUpdatePlayrUITrigger.Raise();
    }

    private void OnDestroy()
    {
        _onPlayerTakeDamageEvent.RemoveListener(PlayerTakeDamage);
    }

    private void Update()
    {
        if(_invincibilityTimer > 0)
            _invincibilityTimer -= Time.deltaTime;
    }

    private void PlayerTakeDamage()
    {
        if(_playerStats.hasShield||_invincibilityTimer>0)
        {
            _playerStats.hasShield = false;
            _invincibilityTimer = 1f;
            return;
        }
        AudioSystem.Instance.PlaySFX("Hit");
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
