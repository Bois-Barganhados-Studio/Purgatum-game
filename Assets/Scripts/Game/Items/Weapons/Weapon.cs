
public class Weapon {

    public static readonly float BASE_COOLDOWN = 0.3f;

    private float baseDmg;
    public float BaseDmg
    {
        get { return baseDmg; }
        set { baseDmg = value; }
    }

    private float range;
    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    private float weight;
    public float Weight
    {
        get { return weight; }
        set { weight = value; }
    }

    public Weapon()
    {
        BaseDmg = 0.0f;
        Range = 0.0f;
        Weight = 0.0f;
    }

    public Weapon(float baseDmg_, float range_, float weight_)
    {
        BaseDmg = baseDmg_;
        Range = range_;
        Weight = weight_;
    }
}