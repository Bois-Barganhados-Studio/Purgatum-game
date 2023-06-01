using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ItemSprites
{



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
