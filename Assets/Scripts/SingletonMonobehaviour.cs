using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using UnityEngine;

/// <summary>
/// Multi-purpose singleton implementation.
/// </summary>
/// <typeparam name="T">The implenting class. Must inherit monobehaviour.</typeparam>
public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
{
    [Header("Singleton Parameters")]
    [Tooltip("If true, the singleton will stay alive accross scenes.")]
    [SerializeField] private bool _persistent = true;

    private static T _instance;
    public static T Instance 
    { 
        get 
        {
            if (_instance != null)
                return _instance;

            return null;
        } 
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = (T)this;

        if (_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
