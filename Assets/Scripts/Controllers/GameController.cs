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
    [SerializeField] private GameEventListener<CustomEvent> _gameOverEvent;
    [SerializeField] private GameEvent _gameWinTrigger;
    [SerializeField] private CurrentLevelSO _currentLevelSO;
    private int _currentLevel = 0;
    private int _currentWave = 0;
    private int _currentXPAmount = 0;
    private bool _dontSpawn;

    private void Start()
    {
        _bubbleDeathEvent.AddListener(BubbleCountDown);
        _restartLevelEvent.AddListener(RestartLevel);
        _stopTimeEvent.AddListener(StopTime);
        _resumeTimeEvent.AddListener(RestartTime);
        _gameOverEvent.AddListener(ResetCurrentLevelIndex);

        _currentLevel = _currentLevelSO.currentLevelIndex;
        _levels[_currentLevel].level.SetActive(true);

        StartCoroutine(LevelLoop());
    }

    private IEnumerator LevelLoop()
    {
        if (_dontSpawn)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(LevelLoop());
            yield break;
        }
        else
        {
            foreach (var spawn in _levels[_currentLevel].waves[_currentWave].spawns)
            {
                var bubblePrefab = _bubblePrefabs[(int)spawn.bubble];
                var bubble = (Bubble)PoolManager.Instance.ReuseComponent(bubblePrefab, new Vector3(spawn.positionX * 13, 10, 0), Quaternion.identity);
                bubble.SetSpawning(true);
            }
            yield return new WaitForSeconds(_levels[_currentLevel].waves[_currentWave].secondsToNext);
            if (_dontSpawn)
            {
                yield return new WaitForSeconds(1f);
                StartCoroutine(LevelLoop());
                yield break;
            }
            _currentWave++;
            if (_currentWave >= _levels[_currentLevel].waves.Count)
                _currentWave = 0;
            StartCoroutine(LevelLoop());
        }
    }

    private void OnDestroy()
    {
        _bubbleDeathEvent.RemoveListener(BubbleCountDown);
        _restartLevelEvent.RemoveListener(RestartLevel);
        _stopTimeEvent.RemoveListener(StopTime);
        _resumeTimeEvent.RemoveListener(RestartTime);
        _gameOverEvent.RemoveListener(ResetCurrentLevelIndex);
    }
    private void BubbleCountDown()
    {
        _currentXPAmount++;
        if (_currentXPAmount >= _levels[_currentLevel].popsRequired)
            StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        _screenText.text = "STAGE COMPLETE!";
        _screenText.gameObject.SetActive(true);
        _dontSpawn = true;

        foreach (var bubble in FindObjectsOfType<Bubble>())
        {
            bubble.gameObject.SetActive(false);
        }

        yield return new WaitForSecondsRealtime(2f);


        _screenText.gameObject.SetActive(false);
        _levels[_currentLevel].level.SetActive(false);

        if((_currentLevel + 1) < _levels.Count)
        {
            _currentLevel++;
            _currentLevelSO.currentLevelIndex = _currentLevel;
            _currentWave = 0;
            _currentXPAmount = 0;
            _dontSpawn = false;
            _player.transform.position = _levels[_currentLevel].playerPosition.transform.position;
            _levels[_currentLevel].level.SetActive(true);
        }
        else
        {
            GameWin();
        }
        
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameWin()
    {
        StopTime();
        _gameWinTrigger.Raise();
        ResetCurrentLevelIndex();
    }

    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void RestartTime()
    {
        Time.timeScale = 1;
    }

    private void ResetCurrentLevelIndex()
    {
        _currentLevelSO.currentLevelIndex = 0;
    }
}

