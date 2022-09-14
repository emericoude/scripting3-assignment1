using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TopDownController
{
    private void OnEnable()
    {
        OnHealthChange.AddListener(CheckIfDead);
    }

    private void OnDisable()
    {
        OnHealthChange.RemoveListener(CheckIfDead);
    }

    private void CheckIfDead()
    {
        if (Health <= 0)
        {
            GameManager.Instance.PlayerDeath();
        }
    }
}
