using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> _listeners = new List<GameEventListener>();

    public void Raise(params object[] parameters)
    {
        for (var i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(parameters);
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!_listeners.Contains(listener))
            _listeners.Add(listener);
    }

    public void UnRegisterListener(GameEventListener listener)
    {
        if (_listeners.Contains(listener))
            _listeners.Remove(listener);
    }
}