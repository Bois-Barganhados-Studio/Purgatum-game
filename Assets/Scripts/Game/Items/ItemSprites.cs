using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ItemSprites
{
    private static Sprite potHealB;
    public static Sprite POTION_HEAL_BASIC
    {
        get
        {
            if (potHealB == null)
            {
                potHealB = Resources.Load<Sprite>("Prefab/Sprites/POTION_HEAL_BASIC");
            } 
            return potHealB;
        }
    }

    private static Sprite potHealE;
    public static Sprite POTION_HEAL_ENHANCED
    {
        get
        {
            if (potHealE == null)
            {
                potHealE = Resources.Load<Sprite>("Prefab/Sprites/POTION_HEAL_ENHANCED");
            }
            return potHealE;
        }
    }


    private static Sprite potHealS;
    public static Sprite POTION_HEAL_SUPERIOR
    {
        get
        {
            if (potHealS == null)
            {
                potHealS = Resources.Load<Sprite>("Prefab/Sprites/POTION_HEAL_SUPERIOR");
            }
            return potHealS;
        }
    }

    private static Sprite potDefB;
    public static Sprite POTION_DEF_BASIC
    {
        get
        {
            if (potDefB == null)
            {
                potDefB = Resources.Load<Sprite>("Prefab/Sprites/POTION_DEF_BASIC");
            } 
            return potDefB;
        }
    }

    private static Sprite potDefE;
    public static Sprite POTION_DEF_ENHANCED
    {
        get
        {
            if (potDefE == null)
            {
                potDefE = Resources.Load<Sprite>("Prefab/Sprites/POTION_DEF_ENHANCED");
            }
            return potDefE;
        }
    }


    private static Sprite potDefS;
    public static Sprite POTION_DEF_SUPERIOR
    {
        get
        {
            if (potDefS == null)
            {
                potDefS = Resources.Load<Sprite>("Prefab/Sprites/POTION_DEF_SUPERIOR");
            }
            return potDefS;
        }
    }

    private static Sprite potDmgB;
    public static Sprite POTION_DMG_BASIC
    {
        get
        {
            if (potDmgB == null)
            {
                potDmgB = Resources.Load<Sprite>("Prefab/Sprites/POTION_DMG_BASIC");
            } 
            return potDmgB;
        }
    }

    private static Sprite potDmgE;
    public static Sprite POTION_DMG_ENHANCED
    {
        get
        {
            if (potDmgE == null)
            {
                potDmgE = Resources.Load<Sprite>("Prefab/Sprites/POTION_DMG_ENHANCED");
            }
            return potDmgE;
        }
    }


    private static Sprite potDmgS;
    public static Sprite POTION_DMG_SUPERIOR
    {
        get
        {
            if (potDmgS == null)
            {
                potDmgS = Resources.Load<Sprite>("Prefab/Sprites/POTION_DMG_SUPERIOR");
            }
            return potDmgS;
        }
    }

    private static Sprite potSpdB;
    public static Sprite POTION_SPEED_BASIC
    {
        get
        {
            if (potSpdB == null)
            {
                potSpdB = Resources.Load<Sprite>("Prefab/Sprites/POTION_SPEED_BASIC");
            } 
            return potSpdB;
        }
    }

    private static Sprite potSpdE;
    public static Sprite POTION_SPEED_ENHANCED
    {
        get
        {
            if (potSpdE == null)
            {
                potSpdE = Resources.Load<Sprite>("Prefab/Sprites/POTION_SPEED_ENHANCED");
            }
            return potSpdE;
        }
    }


    private static Sprite potSpdS;
    public static Sprite POTION_SPEED_SUPERIOR
    {
        get
        {
            if (potSpdS == null)
            {
                potSpdS = Resources.Load<Sprite>("Prefab/Sprites/POTION_SPEED_SUPERIOR");
            }
            return potSpdS;
        }
    }

    private static Sprite[] potionSprites = { 
        POTION_DMG_BASIC, POTION_DMG_ENHANCED, POTION_DMG_SUPERIOR,
        POTION_DEF_BASIC, POTION_DEF_ENHANCED, POTION_DEF_SUPERIOR,
        POTION_HEAL_BASIC, POTION_HEAL_ENHANCED, POTION_HEAL_SUPERIOR,
        POTION_SPEED_BASIC, POTION_SPEED_ENHANCED, POTION_SPEED_SUPERIOR
    };

    public static Sprite GetPotionSprite(int spriteIdx)
    {
        return potionSprites[spriteIdx];
    }

    private static readonly string[] vfxSpriteSufix = { "_N", "_NW", "_W", "_SW", "_S", "_SE", "_E", "_NE" };

    private static Sprite[] wVfxBase;
    public static Sprite[] WEAPON_VFX_BASE
    {
        get
        {
            if (wVfxBase == null)
            {
                wVfxBase = new Sprite[vfxSpriteSufix.Length];
                for (int i = 0; i < vfxSpriteSufix.Length; i++)
                    wVfxBase[i] = Resources.Load<Sprite>("Prefab/Sprites/WEAPON_VFX_BASE" + vfxSpriteSufix[i]);
            }
            return wVfxBase;
        }
    }
}
