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
        gameObject.SetActive(true);
    }

    public void Effect(PlayerObject p)
    {
        if (item != null)
            item.Effect(p);
        else
            Debug.Log("item is null");
    }

    // Initializing
    void Awake()
    {
        gameObject.SetActive(false);
        sRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
}
