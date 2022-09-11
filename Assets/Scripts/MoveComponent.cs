using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The base abstract movement component class. This includes functionality for: <br/>
/// - Ground Check <br/>
/// - Gravity
/// </summary>
/// <remarks>MoveComponents are meant to be used as parameterizable scriptable objects.</remarks>
public abstract class MoveComponent : ScriptableObject
{
    #region Parameters: Ground Check
    [Header("Parameters: Ground Check")]

    [Tooltip("The radius of the spherecast.")]
    [SerializeField] private float _sphereCastRadiusModifier = 1f;

    [Tooltip("The distance travelled by the spherecast.")]
    [SerializeField] private float _sphereCastLength = 0.1f;

    [Tooltip("An offset added to the spherecast spawn position.\n\nNote that the base position is calculated with the character's main collider.")]
    [SerializeField] private Vector3 _sphereCastOffset = Vector3.zero;

    [Tooltip("The layers that the ground check can hit.")]
    [SerializeField] private LayerMask _layersToHit;

    [Space]

    [Tooltip("An event invoked when the entity lands on the ground.")]
    public UnityEvent OnLandEvent;
    [Tooltip("An event invoked when the entity stops touching the ground.")]
    public UnityEvent OnExitGroundEvent;


    private bool _isGrounded = false;
    private RaycastHit _groundHit;

    #endregion
    #region Parameters: Gravity
    [Header("Parameters: Gravity")]

    [Tooltip("The amount of velocity (-y) added to the unit's rigidbody each second.")]
    [SerializeField] private float _gravityPullPerSecond = 50f;

    [Tooltip("The maximum (negative) velocity to reach before gravityPullPerSecond stops being added.")]
    [SerializeField] private float _terminalVelocity = -150f;

    #endregion
    #region Ground Check

    /// <summary>
    ///  Sends a raycast (sphereCast) downwards to check if the entity is grounded.
    /// </summary>
    /// <remarks>Simplified version of the <seealso cref="IsGrounded(out RaycastHit, Collider)"/> function.</remarks>
    /// <param name="sourceCollider">The collider of the controller. This is used to calculate the radius of the sphere cast as well as its position.</param>
    /// <returns>Returns true if grounded; otherwise false.</returns>
    public bool IsGrounded(Collider sourceCollider)
    {
        return IsGrounded(out _groundHit, sourceCollider);
    }

    /// <summary>
    /// Sends a raycast (sphereCast) downwards to check if the entity is grounded.
    /// </summary>
    /// <param name="hit">The hit of the sphereCast.</param>
    /// <param name="sourceCollider">The collider of the controller. This is used to calculate the radius of the sphere cast as well as its position.</param>
    /// <returns>Returns true if grounded; otherwise false.</returns>
    public bool IsGrounded(out RaycastHit hit, Collider sourceCollider)
    {
        if (Physics.SphereCast(CalculateGroundCastPosition(sourceCollider), CalculateGroundCastRadius(sourceCollider), Vector3.down, out hit, _sphereCastLength, _layersToHit, QueryTriggerInteraction.Ignore))
        {
            if (!_isGrounded)
                OnLandEvent?.Invoke();

            _isGrounded = true;
        }
        else
        {
            if (_isGrounded)
                OnExitGroundEvent?.Invoke();

            _isGrounded = false;
        }

        _groundHit = hit;
        return _isGrounded;
    }

    /// <summary>
    /// Calculates the ideal spherecast radius based on the passed-in collider for the ground check.
    /// </summary>
    /// <param name="sourceCollider">The collider to use.</param>
    /// <returns>The width of the collider divided by 2, multiplied by the <see cref="_sphereCastRadiusModifier"/> (default: 1).</returns>
    private float CalculateGroundCastRadius(Collider sourceCollider)
    {
        return ( sourceCollider.bounds.size.x / 2 ) * _sphereCastRadiusModifier;
    }

    /// <summary>
    /// Calculates the ideal spherecast spawn position for the ground check based on the passed-in collider..
    /// </summary>
    /// <param name="sourceCollider">The collider to use.</param>
    /// <returns>The the bottom of the collider.</returns>
    private Vector3 CalculateGroundCastPosition(Collider sourceCollider)
    {
        Vector3 castPosition = sourceCollider.transform.position + _sphereCastOffset;
        castPosition.y = sourceCollider.bounds.min.y + 0.03f + CalculateGroundCastRadius(sourceCollider);

        return castPosition;
    }

    /// <summary>Displays gizmos used for the ground check.</summary>
    /// <remarks>This should be used in OnDrawGizmos().</remarks>
    /// <param name="sourceCollider">The main collider of the entity being controlled. Used to calculate the radius and position of the sphereCast.</param>
    /// <param name ="initialSphere">OVERLOAD: Whether to display the initial sphere of the sphere cast (sphere at the point of origin). True by default.</param>
    /// <param name ="hitSphere">OVERLOAD: Whether to display the hit sphere of the sphere cast (sphere at the position of the hit). True by default.</param>
    /// <param name ="drawLine">OVERLOAD: Whether to display a line from the castPoint to the hit point (or the max distance if no hit). False by default.</param>
    public void IsGroundedGizmos(Collider sourceCollider, bool initialSphere = true, bool hitSphere = true, bool drawLine = false)
    {
        if (!Application.isPlaying)
        {
            IsGrounded(sourceCollider);
        }

        float castRadius = CalculateGroundCastRadius(sourceCollider);
        Vector3 castPosition = CalculateGroundCastPosition(sourceCollider);

        //Display the origin sphere or the spherecast.
        if (initialSphere)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(castPosition, castRadius);
        }

        //Display a sphere at the position of the hit.
        if (hitSphere)
        {
            if (_isGrounded)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_groundHit.point + ( Vector3.up * ( castRadius ) ), castRadius);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(castPosition + ( Vector3.down * _sphereCastLength ), castRadius);
            }
        }

        //Display the line in between the hit point and the initial point
        if (drawLine)
        {
            if (_isGrounded)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(castPosition, _groundHit.point);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(castPosition, castPosition + ( Vector3.down * _sphereCastLength ));
            }
        }
    }

    #endregion
    #region Gravity

    /// <summary>
    /// Adds downwards velocity, up to a maxiumum.
    /// </summary>
    /// <param name="rb">The rigidbody to which the force is applied.</param>
    /// <param name="deltaTime">The selected deltatime.</param>
    public void Gravity(Rigidbody rb, float deltaTime)
    {
        if (rb.velocity.y > _terminalVelocity)
            rb.velocity += (Vector3.down * _gravityPullPerSecond) * deltaTime;
    }

    #endregion
    #region Abstract Methods (to override)

    public abstract void Move(Rigidbody rb, float baseSpeed, Vector3 direction, bool isGrounded, float deltaTime);
    public abstract void Rotate(Rigidbody rb, float inputFowardAngle);
    public abstract void MoveGizmos();
    public abstract void RotateGizmos();

    #endregion
}
