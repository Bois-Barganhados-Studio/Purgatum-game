using System.Collections.Generic;
using UnityEngine;

public class DropGenerator
{
    private static readonly float LEVEL_FACTOR = 1.66f;

    private static readonly string SPRITE_PATH = "Sprite/Items/";

    // Order: axe, dagger, hammer, nunchaku, sickle, spear, sword

    private static readonly string[] weaponType = { "axes_", "daggers_", "hammers_", "nunchakus_", "sickles_", "spears_", "swords_" };

    private static readonly int[] spriteCount = { 3, 9, 6, 3, 6, 9, 12 };

    private static readonly int[] MAX_DMG = { 50, 20, 45, 15, 40, 30, 35 };

    private static readonly int[] MAX_WEIGHT = { 50, 20, 45, 15, 40, 30, 35 };

    private static readonly float[] MAX_RANGE = { 1.5f, 0.25f, 1f, 0.5f, 1.75f, 2f, 1.25f };

    public static List<GameObject> GenerateDrop(int pLuck, int pHealth, int level)
    {
        List<GameObject> drop = null;
        int numOfItems = CalculateItems(pLuck);
        int wLevel = Random.Range(0, 1) + (int)((level - 1) * LEVEL_FACTOR);
        int wType = Random.Range(0, spriteCount.Length);
        int idx = (wLevel * spriteCount[wType]) + Random.Range(0, spriteCount[wType]);
        string s = SPRITE_PATH + weaponType[wType] + idx;
        Debug.Log(s);
        // TODO - Finish this shiet
        return drop;
    }

    public static int CalculateItems(int luck)
    {
        int[] thresholds = { 33, 66, 100 };
        int[] itemCounts = { 1, 2, 3 };

        int randomNumber = Random.Range(0, 101);

        int adjustedThreshold = thresholds.Length * luck / 100;

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (i <= adjustedThreshold && randomNumber <= thresholds[i])
            {
                return itemCounts[i];
            }
        }

        return 3;
    }

}
