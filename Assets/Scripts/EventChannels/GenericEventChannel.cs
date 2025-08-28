using System;
using UnityEngine;

public class GenericEventChannel<T> : ScriptableObject
{
    private event Action<T> OnEventRaised;

    public void Invoke(T value)
    {
        OnEventRaised?.Invoke(value);
    }
}

