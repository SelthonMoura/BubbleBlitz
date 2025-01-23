using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class GameEventListener
{
    public virtual void OnEventRaised(object[] parameters)
    {
    }
}

[Serializable]
public class GameEventListener<T> : GameEventListener where T : CustomEvent, new()
{
    public GameEvent @event;
    public T customEvent;

    public void AddListener(Action method)
    {
        customEvent ??= new T();

        if (customEvent is CustomEvent cE)
        {
            cE.response.AddListener(new UnityAction(method));
        }
        @event.RegisterListener(this);
    }

    public void AddListener<U>(Action<U> method)
    {
        customEvent ??= new T();

        if (customEvent is CustomEvent<U> cE)
        {
            cE.response.AddListener(new UnityAction<U>(method));
        }
        @event.RegisterListener(this);
    }

    public void AddListener<U, V>(Action<U, V> method)
    {
        customEvent ??= new T();

        if (customEvent is CustomEvent<U, V> cE)
        {
            cE.response.AddListener(new UnityAction<U, V>(method));
        }
        @event.RegisterListener(this);
    }

    public void AddListener<U, V, W>(Action<U, V, W> method)
    {
        customEvent ??= new T();

        if (customEvent is CustomEvent<U, V, W> cE)
        {
            cE.response.AddListener(new UnityAction<U, V, W>(method));
        }
        @event.RegisterListener(this);
    }

    public void RemoveListener(Action method)
    {
        if (customEvent == null)
        {
            customEvent = new T();
        }
        if (customEvent is CustomEvent cE)
        {
            cE.response.RemoveListener(new UnityAction(method));
        }
        @event.UnRegisterListener(this);
    }

    public void RemoveListener<U>(Action<U> method)
    {
        if (customEvent == null)
        {
            customEvent = new T();
        }
        if (customEvent is CustomEvent<U> cE)
        {
            cE.response.RemoveListener(new UnityAction<U>(method));
        }
        @event.UnRegisterListener(this);
    }

    public void RemoveListener<U, V>(Action<U, V> method)
    {
        if (customEvent == null)
        {
            customEvent = new T();
        }
        if (customEvent is CustomEvent<U, V> cE)
        {
            cE.response.RemoveListener(new UnityAction<U, V>(method));
        }
        @event.UnRegisterListener(this);
    }

    public void RemoveListener<U, V, W>(Action<U, V, W> method)
    {
        if (customEvent == null)
        {
            customEvent = new T();
        }
        if (customEvent is CustomEvent<U, V, W> cE)
        {
            cE.response.RemoveListener(new UnityAction<U, V, W>(method));
        }
        @event.UnRegisterListener(this);
    }

    public override void OnEventRaised(object[] parameters)
    {
        customEvent.Invoke(parameters);
    }
}