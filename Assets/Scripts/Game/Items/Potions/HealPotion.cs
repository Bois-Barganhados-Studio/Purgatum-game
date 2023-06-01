
public class HealPotion : Potion, IItem
{
    private float healPct;
    public float HealPct
    { 
        get { return healPct; } 
        set { healPct = value; } 
    }
        
    public HealPotion(Potion.LEVEL lvl)
    {
        HealPct = Potion.BASE_PCT * ((int)lvl);
    }

    public override void Effect(PlayerObject p)
    {
        p.Heal(HealPct);
    }
}