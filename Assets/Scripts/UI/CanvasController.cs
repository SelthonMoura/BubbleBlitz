using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameEventListener<CustomEvent> _onGameOverEvent;
    [SerializeField] private GameEventListener<CustomEvent> _onGameWinEvent;
    [SerializeField] private GameEventListener<CustomEvent> _onPauseGameEvent;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _gameWinPanel;
    [SerializeField] private GameObject _gamePausePanel;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _settingsPanel;

    private void Start()
    {
        _onGameOverEvent.AddListener(ShowGameOverPanel);
        _onGameWinEvent.AddListener(ShowGameWinPanel);
        _onPauseGameEvent.AddListener(PauseGame);
    }

    private void OnDestroy()
    {
        _onGameOverEvent.RemoveListener(ShowGameOverPanel);
        _onGameWinEvent.RemoveListener(ShowGameWinPanel);
        _onPauseGameEvent.RemoveListener(PauseGame);
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

    public void ShowSettigs()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void PauseGame()
    {
        if(_shopPanel.activeInHierarchy) return;

        if(Time.timeScale != 0 && !_gamePausePanel.activeInHierarchy)
        {
            _gamePausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Time.timeScale == 0 && _gamePausePanel.activeInHierarchy)
        {
            _gamePausePanel.SetActive(false);
            Time.timeScale = 1;
        }       
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}
