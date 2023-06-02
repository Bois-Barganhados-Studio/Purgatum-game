using System.Collections.Generic;
using System.Net;
using UnityEditorInternal;
using UnityEngine;

public class DropGenerator : MonoBehaviour
{
    private static readonly float LEVEL_FACTOR = 1.66f;

    private static readonly string SPRITE_PATH = "Sprites/Items/";

    public static readonly int POTION_TYPES = 17;

    // Order: dmg, def, heal, speed

    public static readonly int[] basicPotionIdx = { 2, 3, 0, 7 };

    // Order: axe, dagger, hammer, nunchaku, sickle, spear, sword

    private static readonly string[] weaponType = { "axes", "daggers", "hammers", "nunchakus", "sickles", "spears", "swords" };

    private static readonly int[] wSpriteCount = { 3, 9, 6, 3, 6, 9, 12 };

    private static Sprite[] potionSprites = Resources.LoadAll<Sprite>(SPRITE_PATH + "potions");


    private static GameObject wPrefab;
    public static GameObject WPrefab
    {
        get
        {
            if (wPrefab == null)
            {
                wPrefab = Resources.Load<GameObject>("Prefab/Game/Entities/Weapon");
            }
            return wPrefab;
        }
    }

    private static GameObject itemPrefab;
    public static GameObject ItemPrefab
    {
        get
        {
            if (itemPrefab == null)
            {
                itemPrefab = Resources.Load<GameObject>("Prefab/Game/Entities/Item");
            }
            return itemPrefab;
        }
    }

    public static List<MonoBehaviour> GenerateDrop(int pLuck, int level)
    {
        List<MonoBehaviour> drop = new List<MonoBehaviour>();
        drop.Add(GenWeapon(level));
        int n = CalculateItems(pLuck);
        for (int i = 0; i < n; i++)
        {
            drop.Add(GenPotion(pLuck));
        }
        foreach (var d in drop)
        {
            Debug.Log(d);
        }
        return drop;
    }

    public static MonoBehaviour GenPotion(int luck)
    {
        int potType = Random.Range(0, basicPotionIdx.Length);
        int rng = Random.Range(0, 200) + luck;
        int potLevel = 0;
        if (rng > 200)
        {
            potLevel = 2;
        } else if (rng > 100)
        {
            potLevel = 1;
        }
        int potIdx = (potLevel * POTION_TYPES) + basicPotionIdx[potType];
        Sprite potSprite = potionSprites[potIdx];
        var potion = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity);
        var potScript = potion.GetComponent<ItemObject>();
        Potion pot;
        if (potType == (int)Potion.TYPE.HEAL)
        {
            Debug.Log("heal pot lvl " + potLevel);
            pot = new HealPotion((Potion.LEVEL)potLevel);
        } else
        {
            Debug.Log(potType + " boost pot lvl " + potLevel);
            pot = new BoostPotion((Potion.LEVEL)(potLevel + 1), (Potion.TYPE)potType);
        }
        potScript.Init(pot, potSprite);
        Debug.Log((int)pot.Type);
        return potScript;
    }

    public static MonoBehaviour GenWeapon(int level)
    {
        int wLevel = System.Math.Max(Random.Range(0, 1) + (int)((level - 1) * LEVEL_FACTOR), 1);
        int wType = Random.Range(0, weaponType.Length);
        int idx = (wLevel * wSpriteCount[wType]) + Random.Range(0, wSpriteCount[wType]);
        Sprite wSprite = LoadWeaponType(wType)[idx];
        var weapon = Instantiate(WPrefab, Vector3.zero, Quaternion.identity);
        var wScript = weapon.GetComponent<WeaponObject>();
        wScript.Init(new Weapon(wLevel, wType), wSprite, ItemSprites.WEAPON_VFX_BASE, false);
        // Debug.Log("new wp dmg: " + wScript.weapon.BaseDmg);
        return wScript;
    }

    private static List<Sprite[]> wSpritesType = new List<Sprite[]>();

    private static Sprite[] LoadWeaponType(int wType)
    {
        if (wSpritesType.Count == 0)
        {
            for (int i = 0; i < weaponType.Length; i++)
            {
                wSpritesType.Add(null);
            }
        }
        if (wSpritesType[wType] == null)
        {
            wSpritesType[wType] = Resources.LoadAll<Sprite>(SPRITE_PATH + weaponType[wType]);
        }
        return wSpritesType[wType];
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
