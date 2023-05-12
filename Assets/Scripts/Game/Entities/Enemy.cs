using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public const int LAYER = 7;
    public MachineState state;
    private Weapon mainWeapon;

    public MachineState State
    {
        get { return state; }
        set { state = value; }
    }
    public Weapon MainWeapon
    {
        get { return mainWeapon; }
        set { mainWeapon = value; }
    }

    public Enemy(int hp, float speed)
        : base(hp, speed)
    {
        MainWeapon = new Weapon(3.0f, 0.1f, 2.0f);
        State = MachineState.IDLE;
    }

    
    public enum MachineState {
        IDLE,
        CHASING,
        ATTACKING,
        DYING,
    }



    public int takeAttack(Weapon pWeapon)
    {
        int dmg = Random.Range((int)(pWeapon.BaseDmg - pWeapon.BaseDmg * 0.2f), (int)(pWeapon.BaseDmg + pWeapon.BaseDmg * 0.2f));
        if (dmg > 0)
        {
            takeDamage(dmg);
        }
        return dmg;
    }
}