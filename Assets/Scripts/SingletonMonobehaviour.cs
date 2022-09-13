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

    private static T _instance;
    public static T Instance 
    { 
        get 
        {
            if (_instance != null)
                return _instance;

            var instances = FindObjectsOfType<T>();
            var count = instances.Length;
            if (count > 0)
            {
                if (count == 1)
                    return _instance = instances[0];

                for (var i = 1; i < instances.Length; i++)
                    Destroy(instances[i]);

                return _instance = instances[0];
            }

            return _instance = new GameObject($"Singleton: {typeof(T)}").AddComponent<T>();
        } 
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);

        if (_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
