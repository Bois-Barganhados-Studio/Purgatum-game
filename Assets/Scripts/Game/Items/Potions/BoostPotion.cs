using UnityEngine;

public class BoostPotion : Potion, IItem
{
    private float boostPct;
    public float BoostPct
    {
        get { return boostPct; }
        set { boostPct = value; }
    }

    private float duration;
    public float Duration
    {
        get { return duration; }
        set { duration = value; }
    }

    public BoostPotion(Potion.LEVEL lvl, Potion.TYPE type)
    {
        BoostPct = Potion.BASE_PCT * ((int)lvl);
        duration = Potion.BASE_DURATION * ((int)lvl);
        this.Type = type;
    }

    public override void Effect(PlayerObject p)
    {
        if (Type.Equals(TYPE.SPEED))
        {
            p.BoostSpeed(boostPct, duration);
        }
        else if (Type.Equals(TYPE.DAMAGE))
        {
            p.BoostDamage(boostPct, duration);

        }
        else if (Type.Equals(TYPE.DEFENSE))
        {
            p.BoostDefense(boostPct, duration);
        }
    }

}
