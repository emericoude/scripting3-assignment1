using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : PlayerTool
{
    [SerializeField] private float _vacuumStrength = 150f;
    [SerializeField] private float _vacuumLength = 15f;
    [SerializeField] private float _vacuumWidth = 5f;
    [SerializeField] private LayerMask _layersToVacuum;

    private void Update()
    {
        if (_myPlayerInputs.Player.Tool.WasPressedThisFrame())
        {
            Collider[] vacuumables = Physics.OverlapCapsule(
                transform.position,
                transform.position + transform.forward * _vacuumLength,
                _vacuumWidth,
                _layersToVacuum,
                QueryTriggerInteraction.UseGlobal
            );

            if (vacuumables.Length > 0)
            {                
                foreach (Collider collider in vacuumables)
                {
                    Rigidbody rb = collider.GetComponent<Rigidbody>();

                    Vector3 pullDirection = ( transform.position - rb.position ).normalized;
                    rb.velocity = pullDirection * _vacuumStrength;
                }
            }
        }
    }
}
