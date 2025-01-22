using UnityEngine.Events;

public class CustomEvent
{
    public UnityEvent response = new UnityEvent();

    public virtual void Invoke(object[] parameters)
    {
        response.Invoke();
    }
}

public class CustomEvent<T> : CustomEvent
{
    public new UnityEvent<T> response = new UnityEvent<T>();

    public override void Invoke(object[] parameters)
    {
        T parameter = (T)parameters[0];
        response.Invoke(parameter);
    }
}

public class CustomEvent<T, U> : CustomEvent
{
    public new UnityEvent<T, U> response = new UnityEvent<T, U>();

    public override void Invoke(object[] parameters)
    {
        T p1 = (T)parameters[0];
        U p2 = (U)parameters[1];
        response.Invoke(p1, p2);
    }
}

public class CustomEvent<T, U, V> : CustomEvent
{
    public new UnityEvent<T, U, V> response = new UnityEvent<T, U, V>();

    public override void Invoke(object[] parameters)
    {
        T p1 = (T)parameters[0];
        U p2 = (U)parameters[1];
        V p3 = (V)parameters[2];
        response.Invoke(p1, p2, p3);
    }
}
