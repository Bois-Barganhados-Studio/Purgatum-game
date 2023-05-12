using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static readonly float BASE_COOLDOWN = 0.2f;

    public const int LAYER = 6;

    public Player()
        : base(100)
    {
        MainWeapon = new DefaultWeapon();
        SubWeapon = null;
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

    private Weapon mainWeapon;
    public Weapon MainWeapon 
    {   
        get { return mainWeapon; }
        set { mainWeapon = value; }
    }

    private Weapon subWeapon;
    public Weapon SubWeapon
    {
        get { return subWeapon; }
        set { subWeapon = value; }
    }

    public bool CanDodge() 
    {
        return (!dodgingCD && !IsAttacking && Move_State != Entity.MoveState.DODGING);
    }

    public bool CanAttack()
    {
        return (!IsAttacking && Move_State != Entity.MoveState.DODGING);
    }

    public bool CanCollect()
    {
        return (!IsAttacking && Move_State != Entity.MoveState.DODGING);
    }

    //public void Attack()
    //{

    //}

    public int takeAttack(Weapon eWeapon)
    {
        int dmg = Random.Range((int)(eWeapon.BaseDmg - eWeapon.BaseDmg * 0.2f), (int)(eWeapon.BaseDmg + eWeapon.BaseDmg * 0.2f));
        if (dmg > 0)
        {
            takeDamage(dmg);
        }
        return dmg;
    }

    public void DropWeapon()
    {
        MainWeapon = subWeapon;
        subWeapon = null;
    }

    public void Heal(float healPct)
    {
        Hp += (int)((float) MaxHp * healPct);
    }
}
