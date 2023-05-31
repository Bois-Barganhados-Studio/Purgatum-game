using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public Weapon weapon;
    private SpriteRenderer sRenderer;

    public void Awake()
    {
        sRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public void Init(Weapon w, Sprite sprite, bool active)
    {
        weapon = w;
        sRenderer.sprite = sprite;
        gameObject.SetActive(active);
    }

    public Weapon.ANIMATION_TYPE GetAnimationType()
    {
        return weapon.AnimType;
    }

    public IEnumerator Drop(int seconds)
    {
        if (seconds > 6)
            yield return new WaitForSeconds(seconds - 6);
        var oneSec = new WaitForSeconds(1);
        var oldColor = sRenderer.color;
        for (int i = 0; i < 6; i++)
        {
            if (sRenderer.color == oldColor)
                sRenderer.color = new Color(0, 0, 0, 0);
            else
                sRenderer.color = oldColor;
            yield return oneSec;
        }
        Destroy(gameObject);
    }
}