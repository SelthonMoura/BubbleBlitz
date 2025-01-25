using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string _firstLevelScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(_firstLevelScene);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
