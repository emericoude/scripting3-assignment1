using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The default movement component.
/// </summary>
[CreateAssetMenu(fileName = "Default Movement Component", menuName = "Scriptable Objects/Movement Components/Default")]
public class DefaultMoveComponent : MoveComponent
{
    [Header("Parameters: Movement")]
    [SerializeField] private float _moveSpeedModifier = 1f;
    [SerializeField] private float _airSpeedModifier = 0.7f;

    public override void Move(Rigidbody rb, float baseSpeed, Vector3 direction, bool isGrounded, float deltaTime)
    {
        if (isGrounded)
        {
            rb.velocity = (direction.normalized * ( baseSpeed * _moveSpeedModifier )) * deltaTime;
        }
        else
        {
            Vector3 airVelocity = ( direction.normalized * ( baseSpeed * _moveSpeedModifier * _airSpeedModifier ) ) * deltaTime;
            airVelocity.y = rb.velocity.y;
            rb.velocity = airVelocity;
        }
    }

    public override void MoveGizmos()
    {
        throw new System.NotImplementedException();
    }

    public override void Rotate(Rigidbody rb, float inputFowardAngle)
    {
        rb.rotation = Quaternion.Euler(0f, inputFowardAngle, 0f);
    }

    public override void RotateGizmos()
    {
        throw new System.NotImplementedException();
    }
}
