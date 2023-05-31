using UnityEngine;
public class Player : Entity
{
    public static readonly float BASE_COOLDOWN = 0.2f;

    public const int LAYER = 6;

    public Player()
        : base()
    {
        MainWeapon = new Weapon(0, 300f, 0.25f, 3f);
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

    private bool attackingCD;
    public bool AttackingCD
    {
        set { attackingCD = value; }
        get { return attackingCD; }
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
        return (!dodgingCD && CurrentMoveState != Entity.MoveState.DODGING);
    }

    public bool CanAttack()
    {
        return (!IsAttacking && !attackingCD && CurrentMoveState != Entity.MoveState.DODGING);
    }

    public bool CanCollect()
    {
        return (!IsAttacking && CurrentMoveState == Entity.MoveState.IDLE);
    }

    public int TakeAttack(Weapon eWeapon)
    {
        int dmg = Random.Range((int)(eWeapon.BaseDmg - eWeapon.BaseDmg * 0.2f), (int)(eWeapon.BaseDmg + eWeapon.BaseDmg * 0.2f));
        if (dmg > 0)
        {
            TakeDamage(dmg);
        }
        return dmg;
    }

    public void Heal(float healPct)
    {
        int tmp = Hp + (int)((float)MaxHp * healPct);
        Hp += System.Math.Min(tmp, MaxHp);
    }

    internal void BoostSpeed(float boostPct, float duration)
    {
        MoveSpeed *= boostPct;
        CoolDown(() =>
        {
            Speed = Speed;
        }, duration);
    }

    internal void BoostDamage(float boostPct, float duration)
    {
        DamageMultiplier *= boostPct;
        CoolDown(() =>
        {
            Strength = Strength;
        }, duration);
    }

    internal void BoostDefense(float boostPct, float duration)
    {
        DamageMultiplier *= boostPct;
        CoolDown(() =>
        {
            Strength = Strength;
        }, duration);
    }
}
