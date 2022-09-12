using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Vacuum : PlayerTool
{
    [Header("Parameters: Vacuum")]
    [SerializeField] private float _vacuumStrength = 150f;
    [SerializeField] private float _vacuumLength = 15f;
    [SerializeField] private float _vacuumWidth = 5f;
    [SerializeField] private LayerMask _layersToVacuum;

    [Header("Parameters: Throw")]
    [SerializeField] private float _throwForce = 50f;
    [SerializeField] private Transform _heldItemTransform;
    private GameObject _heldItem;

    [Header("Debug")]
    [SerializeField] private bool _debug = false;

    private void Awake()
    {
        if (_heldItemTransform == null)
            _heldItemTransform = transform;
    }

    private void Update()
    {
        if (_myPlayerInputs.Player.Tool.WasPressedThisFrame())
        {
            if (_heldItem == null)
            {
                PullObjects();
            }
            else
            {
                ThrowHeldObject();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (_heldItem == null)
        {
            if (other.CompareTag("Vacuumable"))
            {
                _heldItem = other.gameObject;
                _heldItem.transform.SetPositionAndRotation(_heldItemTransform.position, _heldItemTransform.rotation);
                _heldItem.transform.SetParent(this.transform);
            }
        }
    }

    private void PullObjects()
    {
        Collider[] vacuumables = Physics.OverlapCapsule(
            transform.position,
            transform.position + ( transform.forward * _vacuumLength ),
            _vacuumWidth,
            _layersToVacuum,
            QueryTriggerInteraction.UseGlobal
        );

        if (vacuumables.Length > 0)
        {
            foreach (Collider collider in vacuumables)
            {
                //check if they're blocked first.

                Rigidbody rb = collider.GetComponent<Rigidbody>();

                Vector3 pullDirection = ( transform.position - rb.position ).normalized;
                rb.velocity = pullDirection * _vacuumStrength;
            }
        }
    }

    private void ThrowHeldObject()
    {
        Rigidbody rb = _heldItem.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _throwForce, ForceMode.Impulse);

        _heldItem = null;
        //I will probably need a reference to the collider
    }


    private void OnDrawGizmos()
    {
        if (_debug)
        {
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Vector3.zero, _vacuumWidth);
            Gizmos.DrawWireSphere(Vector3.forward * _vacuumLength, _vacuumWidth);

            var cubeSize = new Vector3(
                x: _vacuumWidth * 2,
                y: _vacuumWidth * 2,
                z: _vacuumLength
            );

            Gizmos.DrawWireCube(Vector3.forward * _vacuumLength / 2, cubeSize);
        }
    }
}
