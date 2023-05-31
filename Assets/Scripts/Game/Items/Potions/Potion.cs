public abstract class Potion : IItem
{

    public static readonly float BASE_PCT = 0.1f;

    public static readonly int BASE_DURATION = 20;

    public enum LEVEL
    {
        BASIC = 1,
        ENHANCED = 2,
        SUPERIOR = 3
    }

    public enum TYPE
    {
        DAMAGE,
        DEFENSE,
        HEAL,
        SPEED
    }

    public virtual void Effect(PlayerObject po)
    {

    }

}
