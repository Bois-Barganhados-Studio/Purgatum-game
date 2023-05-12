using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public const int LAYER = 7;

    private Weapon mainWeapon;
    public Weapon MainWeapon
    {
        get { return mainWeapon; }
        set { mainWeapon = value; }
    }

    public Enemy(int hp)
        : base(hp)
    {
        MainWeapon = new Weapon(20.0f, 0.1f, 2.0f);
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