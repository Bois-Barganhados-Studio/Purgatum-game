using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public const int LAYER = 6;

    public Player()
        : base(100)
    {
    }

    public Vector2 MoveVelocity()
    {
        return Direction * MoveSpeed;
    }

    public Vector2 DodgeVelocity()
    {
        return FacingDir * DodgeSpeed;
    }

    private bool dodgingCD;
    public bool DodgingCD
    {
        set { dodgingCD = value; }
        get { return dodgingCD; }
    }


    public bool CanDodge() 
    {
        return (!dodgingCD && !IsAttaking && Move_State != Entity.MoveState.DODGING);
    }

    public bool CanAttack()
    {
        return (!IsAttaking && Move_State != Entity.MoveState.DODGING);
    }

    public void Attack()
    {
        
    }

}
