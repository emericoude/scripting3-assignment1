using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTool : Interactable
{
    [SerializeField] protected Collider _interactHitBox;

    protected bool _equipped = false;
    protected TopDownController _myPlayer;
    protected PlayerInputs _myPlayerInputs;

    public void Equip(TopDownController player)
    {
        _interactHitBox.enabled = false;

        _myPlayer = player;
        _myPlayerInputs = player.Inputs;

        _equipped = true;
    }

    public void Drop()
    {
        _equipped = false;

        _myPlayer = null;
        _myPlayerInputs = null;

        _interactHitBox.enabled = true;
    }

    public override void Use(TopDownController user)
    {
        Equip(user);
    }

    protected override bool IsInteractable()
    {
        return true;
    }
}
