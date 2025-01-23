using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _levels = new List<GameObject>();
    [SerializeField] private GameEventListener<CustomEvent> _bubbleSpawnEvent;
    [SerializeField] private GameEventListener<CustomEvent> _bubbleDeathEvent;
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
            Debug.Log("Clear");
    }
}
