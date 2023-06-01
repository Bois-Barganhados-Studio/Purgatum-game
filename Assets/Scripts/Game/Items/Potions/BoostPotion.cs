public class BoostPotion : Potion, IItem
{


    private TYPE type;
    public TYPE Type
    {
        get { return type; }
        set { type = value; }
    }

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
        this.type = type;
    }

    public override void Effect(PlayerObject p)
    {
        if (type.Equals(TYPE.SPEED)) 
            p.BoostSpeed(boostPct, duration);
        else if (type.Equals(TYPE.DAMAGE))
            p.BoostDamage(boostPct, duration);
        else if (type.Equals(TYPE.DEFENSE))
            p.BoostDefense(boostPct, duration);
    }

}
