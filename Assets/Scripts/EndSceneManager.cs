using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.InitEndScene();
    }
}
