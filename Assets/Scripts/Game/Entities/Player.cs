using UnityEngine;
public class Player : Entity
{
    public static readonly float BASE_COOLDOWN = 0.2f;

    public const int LAYER = 6;

    public Player()
        : base()
    {
        MainWeapon = new Weapon(0, 3f, 0.1f, 3f);
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

    public bool IsInvincible { get; set; }

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

    private int skillPoints;
    public int SkillPoints
    {
        get { return skillPoints; }
        set { skillPoints = value; }
    }

    public bool CanDodge() 
    {
        return (!dodgingCD && CurrentMoveState != Entity.MoveState.DODGING);
    }

    public bool CanAttack()
    {
        return (!IsAttacking && !AttackingCD && CurrentMoveState != Entity.MoveState.DODGING);
    }

    public bool CanCollect()
    {
        return (!IsAttacking && CurrentMoveState == Entity.MoveState.IDLE);
    }

    public void Heal(float healPct)
    {
        int tmp = Hp + (int)((float)MaxHp * healPct);
        Hp = System.Math.Min(tmp, MaxHp);
    }

    internal void BoostSpeed(float boostPct)
    {
        MoveSpeed += (MoveSpeed * boostPct);
        DodgeSpeed += (DodgeSpeed * boostPct);
    }

    internal void BoostDamage(float boostPct)
    {
        DamageMultiplier *= boostPct;
    }

    internal void BoostDefense(float boostPct)
    {
        DamageReduction *= boostPct;
    }

    public void Revive()
    {
        Hp = MaxHp;
        IsDead = false;
    }   

}
