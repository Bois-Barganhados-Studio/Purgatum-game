public class BoostPotion : Potion, IItem
{
    public enum BOOST_TYPE
    {
        SPEED,
        DAMAGE,
        DEFENSE
    }

    private BOOST_TYPE bType;
    public BOOST_TYPE BType
    {
        get { return bType; }
        set { bType = value; }
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

    public BoostPotion(Potion.LEVEL lvl, BoostPotion.BOOST_TYPE type)
    {
        BoostPct = Potion.BASE_PCT * ((int)lvl);
        duration = Potion.BASE_DURATION * ((int)lvl);
        bType = type;
    }

    public virtual void Effect(PlayerObject p)
    {
        if (bType.Equals(BOOST_TYPE.SPEED)) 
            p.BoostSpeed(boostPct, duration);
        else if (bType.Equals(BOOST_TYPE.DAMAGE))
            p.BoostDamage(boostPct, duration);
        else if (bType.Equals(BOOST_TYPE.DEFENSE))
            p.BoostDefense(boostPct, duration);
    }

}
