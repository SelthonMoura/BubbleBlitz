using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _firstLevelScene;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _exitPanel;
    [SerializeField] private Animator _anim;
    [SerializeField] private ItemPriceSO[] _itemPriceSOs;

    public void PlayGame()
    {
        SceneManager.LoadScene(_firstLevelScene);

        for(int i = 0; i < _itemPriceSOs.Length; i++)
        {
            _itemPriceSOs[i].ResetInflation();
        }
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

    public void OpenExitPanel()
    {
        _exitPanel.SetActive(true);
        _mainMenuPanel.SetActive(false);
    }

    public void CloseExitPanel()
    {
        _exitPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
