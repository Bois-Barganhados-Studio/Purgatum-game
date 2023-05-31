using UnityEngine;

public class Player : Entity
{
    public static readonly float BASE_COOLDOWN = 0.2f;

    public const int LAYER = 6;

    public Player()
    {
        MainWeapon = new DefaultWeapon();
        SubWeapon = null;
    }

    public Vector2 MoveVelocity()
    {
        return CurrentDirection * MoveSpeed;
    }

    public Vector2 DodgeVelocity()
    {
        return FacingDirection * DodgeSpeed;
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
        return (!dodgingCD && !IsAttacking && CurrentMoveState != Entity.MoveState.DODGING);
    }

    public bool CanAttack()
    {
        return (!IsAttacking && CurrentMoveState != Entity.MoveState.DODGING);
    }

    public bool CanCollect()
    {
        return (!IsAttacking && CurrentMoveState == Entity.MoveState.IDLE);
    }


    public int takeAttack(Weapon eWeapon)
    {
        int dmg = Random.Range((int)(eWeapon.BaseDmg - eWeapon.BaseDmg * 0.2f), (int)(eWeapon.BaseDmg + eWeapon.BaseDmg * 0.2f));
        if (dmg > 0)
        {
            TakeDamage(dmg);
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
