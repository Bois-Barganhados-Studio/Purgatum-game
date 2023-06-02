using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private IItem item;
    private SpriteRenderer sRenderer;

    public void Init(IItem i, Sprite sprite)
    {
        item = i;
        sRenderer.sprite = sprite;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Effect(PlayerObject p)
    {
        if (item == null) 
            return;
        item.Effect(p);
    }

    // Initializing
    void Awake()
    {
        gameObject.SetActive(false);
        sRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
}
