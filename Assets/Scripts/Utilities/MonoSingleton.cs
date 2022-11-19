using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Current { get; private set; }
    
    protected virtual void Awake()
    {
        if (Current != null)
        {
            Debug.LogError($"An instance of {typeof(T).Name} already exists!");
            Destroy(this);
            return;
        }
        
        Current = (T)this;
    }
}