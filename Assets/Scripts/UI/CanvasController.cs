using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameEventListener<CustomEvent> _onGameOverEvent;
    [SerializeField] private GameObject _gameOverPanel;

    private void Start()
    {
        _onGameOverEvent.AddListener(ShowGameOverPanel);
    }

    private void OnDestroy()
    {
        _onGameOverEvent.RemoveListener(ShowGameOverPanel);
    }

    private void ShowGameOverPanel()
    {
        AudioSystem.Instance.StopBackgroundMusic();
        AudioSystem.Instance.PlaySFX("GameOver");
        _gameOverPanel.SetActive(true);
    }
}
