using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _firstLevelScene;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private Animator _anim;

    public void PlayGame()
    {
        SceneManager.LoadScene(_firstLevelScene);
    }

    public void ShowCredits()
    {
        _anim.SetTrigger("OpenCredits");
    }

    public void HideCredits()
    {
        _anim.SetTrigger("CloseCredits");
    }

    public void ShowSettings()
    {
        _anim.SetTrigger("OpenSettings");
    }

    public void HideSettings()
    {
        _anim.SetTrigger("CloseSettings");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
