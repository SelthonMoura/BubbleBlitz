using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private List<GameObject> _bubblePrefabs;
    [SerializeField] private List<GameLevel> _levels = new List<GameLevel>();
    [SerializeField] private TextMeshProUGUI _screenText;
    [SerializeField] private GameEventListener<CustomEvent> _bubbleDeathEvent;
    [SerializeField] private GameEventListener<CustomEvent> _restartLevelEvent;
    [SerializeField] private GameEventListener<CustomEvent> _stopTimeEvent;
    [SerializeField] private GameEventListener<CustomEvent> _resumeTimeEvent;
    private int _currentLevel = 0;
    private int _currentWave = 0;
    private int _currentXPAmount = 0;
    private int _XPToNextLevel = 0;

    private void Start()
    {
        _bubbleDeathEvent.AddListener(BubbleCountDown);
        _restartLevelEvent.AddListener(RestartLevel);
        _stopTimeEvent.AddListener(StopTime);
        _resumeTimeEvent.AddListener(RestartTime);
        CalculateXPToNextLevel(_currentLevel);
        StartCoroutine(LevelLoop());
    }

    private IEnumerator LevelLoop()
    {
        foreach(var spawn in _levels[_currentLevel].waves[_currentWave].spawns)
        {
            var bubblePrefab = _bubblePrefabs[(int)spawn.bubble];
            var bubble = (Bubble)PoolManager.Instance.ReuseComponent(bubblePrefab, new Vector3(spawn.positionX*13, 10, 0), Quaternion.identity);
            bubble.SetSpawning(true);
        }
        yield return new WaitForSeconds(_levels[_currentLevel].waves[_currentWave].secondsToNext);
        _currentWave++;
        if(_currentWave>= _levels[_currentLevel].waves.Count)
            _currentWave = 0;
        StartCoroutine(LevelLoop());
    }

    private void OnDestroy()
    {
        _bubbleDeathEvent.RemoveListener(BubbleCountDown);
        _restartLevelEvent.RemoveListener(RestartLevel);
        _stopTimeEvent.RemoveListener(StopTime);
        _resumeTimeEvent.RemoveListener(RestartTime);
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
        foreach (var bubble in FindObjectsOfType<Bubble>()) {
            bubble.gameObject.SetActive(false);
        }
        yield return new WaitForSecondsRealtime(2f);
        _screenText.gameObject.SetActive(false);
        _levels[_currentLevel].level.SetActive(false);
        _currentLevel++;
        _player.transform.position = _levels[_currentLevel].playerPosition.transform.position;
        _levels[_currentLevel].level.SetActive(true);
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

