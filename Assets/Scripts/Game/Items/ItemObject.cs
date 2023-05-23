using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private IItem item;

    public void SetItem(IItem i)
    {
        item = i;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
