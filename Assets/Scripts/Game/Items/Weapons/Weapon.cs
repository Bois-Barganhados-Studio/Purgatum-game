using UnityEngine;

public class Weapon {

    public static readonly float BASE_COOLDOWN = 0.1f;

    public static readonly int MAX_LEVEL = 10;

    public readonly int Level;
    public enum TYPE
    {
        AXE,
        DAGGER,
        HAMMER,
        NUNCHAKU,
        SICKLE,
        SPEAR,
        SWORD
    }

    public TYPE Type { get; set; }

    // Order: axe, dagger, hammer, nunchaku, sickle, spear, sword

    private static readonly int[] MAX_DMG = { 100, 50, 80, 30, 90, 60, 70 };

    private static readonly int[] MAX_WEIGHT = { 10, 4, 8, 2, 9, 7, 6 };

    private static readonly float[] MAX_RANGE = { 1.5f, 0.25f, 1f, 0.5f, 1.75f, 2f, 1.25f };

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
    }

    public Weapon(int level, int type)
    {
        Level = level;
        BaseDmg = Random.Range((float)(level - 1) / MAX_LEVEL * MAX_DMG[type] + 1, (float)level / MAX_LEVEL * MAX_DMG[type]);
        Range = Random.Range((float)(level - 1) / MAX_LEVEL * MAX_RANGE[type] + 1, (float)level / MAX_LEVEL * MAX_RANGE[type]);
        Weight = Random.Range((float)(level - 1) / MAX_LEVEL * MAX_WEIGHT[type] + 1, (float)level / MAX_LEVEL * MAX_WEIGHT[type]);
    }
}