using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private IItem item;
    private SpriteRenderer sRenderer;

    public bool Used { get; set; }

    public void Init(IItem i, Sprite sprite)
    {
        Used = false;
        item = i;
        sRenderer.sprite = sprite;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Effect(PlayerObject p)
    {
        if (item == null || Used) 
            return;
        item.Effect(p);
        Used = true;
        item = null;
    }

    // Initializing
    void Awake()
    {
        gameObject.SetActive(false);
        sRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
}
