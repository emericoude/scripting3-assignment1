using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoader.LoadScene(_sceneToLoad);
        }
    }
}
