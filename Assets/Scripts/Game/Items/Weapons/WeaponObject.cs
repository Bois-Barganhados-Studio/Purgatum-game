using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public Weapon weapon;
    private SpriteRenderer sRenderer;

    public void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public void Init(Weapon w, Sprite sprite)
    {
        weapon = w;
        sRenderer.sprite = sprite;
        gameObject.SetActive(true);
    }

    public Weapon.ANIMATION_TYPE GetAnimationType()
    {
        return weapon.AnimType;
    }
}