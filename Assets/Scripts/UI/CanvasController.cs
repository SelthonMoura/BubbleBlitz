using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameEventListener<CustomEvent> _onGameOverEvent;
    [SerializeField] private GameEventListener<CustomEvent> _onGameWinEvent;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _gameWinPanel;

    private void Start()
    {
        _onGameOverEvent.AddListener(ShowGameOverPanel);
        _onGameWinEvent.AddListener(ShowGameWinPanel);
    }

    private void OnDestroy()
    {
        _onGameOverEvent.RemoveListener(ShowGameOverPanel);
        _onGameWinEvent.RemoveListener(ShowGameWinPanel);
    }

    private void ShowGameOverPanel()
    {
        AudioSystem.Instance.StopBackgroundMusic();
        AudioSystem.Instance.PlaySFX("GameOver");
        _gameOverPanel.SetActive(true);
    }

    private void ShowGameWinPanel()
    {
        _gameWinPanel.SetActive(true);
    }
}
