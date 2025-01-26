using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStatsSO;
    [SerializeField] private GameEvent _resumeTimeTrigger;
    [SerializeField] private string _firstLevelScene;
    [SerializeField] private string _menuScene;

    public void RestartGameButton()
    {
        AudioSystem.Instance.StopAllSFX();
        _playerStatsSO.ResetStats();
        _resumeTimeTrigger.Raise();
        SceneManager.LoadScene(_firstLevelScene);
    }

    public void MenuButton()
    {
        _playerStatsSO.ResetStats();
        _resumeTimeTrigger.Raise();
        SceneManager.LoadScene(_menuScene);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
