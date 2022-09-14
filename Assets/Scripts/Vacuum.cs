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

    [Header("Parametesr: Vacuum Grab Zone")]
    [SerializeField] private float _vacuumGrabLength = 1f;
    [SerializeField] private float _vacuumGrabWidth = 2f;

    [Header("Parameters: Throw")]
    [SerializeField] private float _throwForce = 50f;
    [SerializeField] private Transform _heldItemTransform;
    [SerializeField] private float _pullCooldown = 1f;
    private GameObject _heldItem;
    private bool _pullOnCooldown = false;

    [Header("Debug")]
    [SerializeField] private bool _debug = false;

    private void Awake()
    {
        if (_heldItemTransform == null)
            _heldItemTransform = transform;
    }

    private void Update()
    {

        if (!_equipped)
            return;

        if (_myUserInputs.Player.Tool.IsPressed())
        {
            if (_heldItem == null)
            {
                if (!_pullOnCooldown)
                {
                    PullObjects();
                    GrabObject();
                }
            }
        }

        if (_myUserInputs.Player.Tool.WasPerformedThisFrame())
        {
            if (_heldItem != null)
                ThrowHeldObject();
        }
    }

    private void PullObjects()
    {
        Collider[] vacuumables = Physics.OverlapCapsule(
            transform.position + ( transform.forward * _vacuumWidth ),
            transform.position + ( transform.forward * _vacuumLength ) + ( transform.forward * _vacuumWidth ),
            _vacuumWidth,
            _layersToVacuum,
            QueryTriggerInteraction.UseGlobal
        );

        if (vacuumables.Length > 0)
        {
            foreach (Collider collider in vacuumables)
            {
                //check if they're blocked first.

                if (collider.TryGetComponent(out Rigidbody rb))
                {
                    Vector3 pullDirection = ( transform.position - rb.position ).normalized;
                    rb.velocity = pullDirection * _vacuumStrength; //I should be adding forces.
                }
            }
        }
    }

    private void GrabObject()
    {
        Collider[] vacuumables = Physics.OverlapBox(
            transform.position + ( transform.forward * _vacuumGrabLength ),
            new Vector3(_vacuumGrabWidth, _vacuumGrabWidth, _vacuumGrabLength),
            transform.rotation,
            _layersToVacuum,
            QueryTriggerInteraction.UseGlobal
        );

        if (vacuumables.Length > 0)
        {
            if (vacuumables[0].TryGetComponent(out Rigidbody rb))
            {
                _heldItem = vacuumables[0].gameObject;

                rb.velocity = Vector3.zero;
                rb.isKinematic = true;

                _heldItem.transform.SetPositionAndRotation(_heldItemTransform.position, _heldItemTransform.rotation);
                _heldItem.transform.SetParent(transform);
            }
        }
    }

    private void ThrowHeldObject()
    {
        if (_heldItem.TryGetComponent(out Rigidbody rb))
        {
            _heldItem.transform.SetParent(null);

            rb.isKinematic = false;
            rb.AddForce(transform.forward * _throwForce, ForceMode.Impulse);

            _heldItem = null;
            //I will probably need a reference to the collider

            StartCoroutine(PullCooldown());
        }
    }

    private IEnumerator PullCooldown()
    {
        _pullOnCooldown = true;
        yield return new WaitForSeconds(_pullCooldown);
        _pullOnCooldown = false;
    }


    private void OnDrawGizmos()
    {
        if (_debug)
        {
            if (_equipped)
            {
                Gizmos.matrix = transform.localToWorldMatrix;

                //Vacuum gizmos
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(Vector3.forward * _vacuumWidth, _vacuumWidth);
                Gizmos.DrawWireSphere(( Vector3.forward * _vacuumLength ) + ( Vector3.forward * _vacuumWidth ), _vacuumWidth);

                var cubeSize = new Vector3(
                    x: _vacuumWidth * 2,
                    y: _vacuumWidth * 2,
                    z: _vacuumLength
                );

                Gizmos.DrawWireCube(( Vector3.forward * _vacuumLength / 2 ) + ( Vector3.forward * _vacuumWidth ), cubeSize);

                //Grab Gizmos
                Gizmos.DrawCube(Vector3.forward * _vacuumGrabLength, new Vector3(_vacuumGrabWidth, _vacuumGrabWidth, _vacuumGrabLength));
            }
        }
    }
}
