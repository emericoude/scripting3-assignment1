using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerTool : Interactable
{
    [SerializeField] protected Collider _interactHitBox;
    [SerializeField] protected Rigidbody _rb;
    protected PlayerInputs _myUserInputs;
    protected bool _equipped = false;

    private void Awake()
    {
        if (_rb is null)
            _rb = GetComponent<Rigidbody>();
    }

    public override void Use(TopDownController user)
    {
        if (user.HeldTool is not null)
            user.HeldTool.Drop(user);

        Equip(user);
    }

    public void Equip(TopDownController user)
    {
        _interactHitBox.enabled = false;

        _myUserInputs = user.Inputs;
        user.HeldTool = this;

        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
        _rb.detectCollisions = false;

        transform.SetPositionAndRotation(user.HeldItemPosition.position, user.HeldItemPosition.rotation);
        transform.SetParent(user.HeldItemPosition);

        _equipped = true;
    }

    public void Drop(TopDownController user)
    {
        _equipped = false;

        user.HeldTool = null;
        _myUserInputs = null;

        transform.SetParent(null);
        _rb.isKinematic = false;
        _rb.detectCollisions = true;

        _interactHitBox.enabled = true;
    }

    protected override bool IsInteractable()
    {
        return true;
    }
}
