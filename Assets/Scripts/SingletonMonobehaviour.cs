using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

using UnityEngine;

/// <summary>
/// Multi-purpose singleton implementation.
/// </summary>
/// <typeparam name="T">The implenting class. Must inherit monobehaviour.</typeparam>
public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singleton Parameters")]
    [Tooltip("If true, the singleton will stay alive accross scenes.")]
    [SerializeField] private bool _persistent = true;
    [Tooltip("If true, destroy self if there is already an instance of this singleton.")]
    [SerializeField] private bool _destroyIfExisting = true;

    private static T _instance;
    public static T Instance
    {
        get
        {
            //return the instance. If null, create one.
            return _instance ??= new GameObject($"Singleton: {typeof(T)}").AddComponent<T>();
        }
    }

    private void Awake()
    {
        if (_destroyIfExisting)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
        }

        if (_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
