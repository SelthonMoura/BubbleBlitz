using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _levels = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI _screenText;
    [SerializeField] private GameEventListener<CustomEvent> _bubbleSpawnEvent;
    [SerializeField] private GameEventListener<CustomEvent> _bubbleDeathEvent;
    [SerializeField] private GameEventListener<CustomEvent> _restartLevelEvent;
    [SerializeField] private GameEventListener<CustomEvent> _stopTimeEvent;
    [SerializeField] private GameEventListener<CustomEvent> _resumeTimeEvent;
    [SerializeField] private int _currentLevel = 0;
    [SerializeField] private int _currentXPAmount = 0;
    [SerializeField] private int _XPToNextLevel = 0;
    [SerializeField] private int _bubbleCount;

    private void Start()
    {
        _bubbleSpawnEvent.AddListener(BubbleCountUp);
        _bubbleDeathEvent.AddListener(BubbleCountDown);
        _restartLevelEvent.AddListener(RestartLevel);
        _stopTimeEvent.AddListener(StopTime);
        _resumeTimeEvent.AddListener(RestartTime);

        CalculateXPToNextLevel(_currentLevel);
    }

    private void OnDestroy()
    {
        _bubbleSpawnEvent.RemoveListener(BubbleCountUp);
        _bubbleSpawnEvent.RemoveListener(BubbleCountDown);
        _restartLevelEvent.RemoveListener(RestartLevel);
        _stopTimeEvent.RemoveListener(StopTime);
        _resumeTimeEvent.RemoveListener(RestartTime);
    }

    private void BubbleCountUp()
    {
        _bubbleCount++;
    }

    private void BubbleCountDown()
    {
        _currentXPAmount++;
        if (_currentXPAmount >= _XPToNextLevel)
            StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        _screenText.text = "FASE COMPLETA!";
        _screenText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {

    }

    private void CalculateXPToNextLevel(int level)
    {
        _XPToNextLevel = (int)Mathf.Pow(level + 4, 2) * 2;
    }

    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void RestartTime()
    {
        Time.timeScale = 1;
    }
}

