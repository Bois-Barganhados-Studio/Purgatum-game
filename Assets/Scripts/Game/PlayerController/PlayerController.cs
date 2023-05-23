using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public PlayerObject player;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerObject>();
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (player.IsUpdateDisabled)
            return;
        if (player.getMoveState() == Entity.MoveState.MOVING || player.getMoveState() == Entity.MoveState.IDLE) {
            rb.velocity = player.MoveVelocity();
            FindObjectOfType<PlayerAnimation>().SetMoveDirection(player.getDirection());
        } else if (player.getMoveState() == Entity.MoveState.DODGING) {
            rb.velocity = player.DodgeVelocity();
            FindObjectOfType<PlayerAnimation>().SetDodgeDirection(player.getFacingDir());
        }
        //if (player.isAttacking()) {
        //    //FindObjectOfType<PlayerAnimation>().SetAttackDirection(player.getDirection());
        //}
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        player.Move(ctx.ReadValue<Vector2>());
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.Dodge();
        }
    }

    public void EndDodge()
    {
        player.EndDodge();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) {
            player.Attack();
        }
    }

    public void EndAttack()
    {
        player.EndAttack();
    }

    public void OnCollect(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.Collect();
        }
    }
}
