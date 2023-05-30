using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerObject player;
    

    private void Awake()
    {
        player = GetComponent<PlayerObject>();
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (player.IsUpdateDisabled)
            return;
        if (player.GetMoveState() == Entity.MoveState.MOVING || player.GetMoveState() == Entity.MoveState.IDLE) {
            player.rb.velocity = player.MoveVelocity();
            if (!player.IsAttacking())
                player.pAnim.SetMoveDirection(player.GetDirection());
        } else if (player.GetMoveState() == Entity.MoveState.DODGING) {
            player.rb.velocity = player.DodgeVelocity();
            player.pAnim.SetDodgeDirection(player.GetFacingDir());
        }
        //if (player.IsAttacking())
        //{
        //    player.pAnim.SetAttackDirection(player.GetDirection());
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

    public void OnSwap(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.SwapWeapon();
        }
    }

    public void OnTest(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Add whatever you want
            // test by pressing 'T'
            player.Test();
            
        }
    }
}
