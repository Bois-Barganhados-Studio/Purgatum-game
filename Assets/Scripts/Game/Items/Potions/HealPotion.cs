
public class HealPotion : ICollectables
{
    private float healPct;
    public float HealPct
    { 
        get { return healPct; } 
        set { healPct = value; } 
    }
        
    public HealPotion(float healPct)
    {
        HealPct = healPct;
    }

    public virtual void Effect(PlayerObject p)
    {
        p.Heal(HealPct);
    }
}