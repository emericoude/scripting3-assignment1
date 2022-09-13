using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    private int _currentPoints;
    public int CurrentPoints
    { 
        get { return _currentPoints; } 
        set 
        { 
            _currentPoints = value;
            UIManager.Instance.UpdatePointsUGUI(_currentPoints);
        } 
    }

    private float _runTimer = 0f;
    public float RunTimer { get { return _runTimer; } set { _runTimer = value; } }
    private bool _timerEnabled = false;
    public bool TimerEnabled { get { return _timerEnabled; } set { _timerEnabled = value; } }

    private void Update()
    {
        if (_timerEnabled)
            _runTimer += Time.deltaTime;
    }
}
