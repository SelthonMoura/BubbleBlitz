using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _levels = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI _screenText;
    [SerializeField] private GameEventListener<CustomEvent> _bubbleSpawnEvent;
    [SerializeField] private GameEventListener<CustomEvent> _bubbleDeathEvent;
    private int _currentLevel = 0;
    private int _bubbleCount;

    private void Start()
    {
        _bubbleSpawnEvent.AddListener(BubbleCountUp);
        _bubbleDeathEvent.AddListener(BubbleCountDown);
    }

    private void OnDestroy()
    {
        _bubbleSpawnEvent.RemoveListener(BubbleCountDown);
    }
    private void BubbleCountUp()
    {
        _bubbleCount++;
    }

    private void BubbleCountDown()
    {
        _bubbleCount--;
        if (_bubbleCount <= 0)
            StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        _screenText.text = "FASE COMPLETA!";
        _screenText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
    }
}
