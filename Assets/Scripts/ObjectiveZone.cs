using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem.HID;

public class ObjectiveZone : MonoBehaviour
{
    [SerializeField] private LayerMask _layersToCollide;

    private void OnTriggerEnter(Collider other)
    {
        if (_layersToCollide == ( _layersToCollide | ( 1 << other.gameObject.layer ) ))
        {
            Destroy(other.gameObject);
            GameManager.Instance.CurrentPoints++;
        }
    }
}
