using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class Unit : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 1;
    public float MaxHealth { get { return _maxHealth; } set{ _maxHealth = value; } }

    private float _health = 0;
    public float Health 
    { 
        get { return _health; } 
        set 
        {
            _health = value;
            OnHealthChange?.Invoke();
        } 
    }

    [Space]
    [SerializeField] private UnityEvent _onHealthChange = null;
    public UnityEvent OnHealthChange { get { return _onHealthChange; } set { _onHealthChange = value; } }

    protected virtual void Awake()
    {
        _health = _maxHealth;
    }
}
