using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    [SerializeField] private int _currentPoints;
    public int CurrentPoints { get { return _currentPoints; } set { _currentPoints = value; } }
}
