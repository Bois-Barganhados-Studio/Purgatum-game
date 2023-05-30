
public class Weapon {

    public static readonly float BASE_COOLDOWN = 0.3f;

    public readonly int Level;

    // TODO - Add meaningful names lol
    public enum ANIMATION_TYPE
    {
        ANIM_1,
        ANIM_2,
        ANIM_3
    }

    public ANIMATION_TYPE AnimType { get; set; }

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
        Level = 0;
        BaseDmg = 0.0f;
        Range = 0.0f;
        Weight = 0.0f;
    }

    public Weapon(int level, float baseDmg, float range, float weight)
    {
        Level = level;
        BaseDmg = baseDmg;
        Range = range;
        Weight = weight;
        // TODO - Range/Dmg logic to determine ANIM enum
    }
}