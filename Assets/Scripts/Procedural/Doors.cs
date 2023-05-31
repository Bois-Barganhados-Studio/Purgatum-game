using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Doors : MonoBehaviour
{
    private bool openned = true;
    private static readonly Vector3 MIN_POS = new Vector3(-0.16f, -0.32f, 0), MAX_POS = new Vector3(-0.16f, -0.80f, 0);
    private const string DOOR_COMPONENT = "Door";
    private GameObject doorComponent;

    void Awake()
    {
        // for (int i = 0; i < transform.childCount - 1; i++)
        // {
        //     GameObject child = transform.GetChild(i).gameObject;
        //     if (child.name == DOOR_COMPONENT)
        //     {
        //         doorComponent = child;
        //         break;
        //     }
        // }
        doorComponent = gameObject;
        SetState(false);
    }
    public void SetState(bool openned)
    {
        this.openned = openned;
        DoAnimation();
    }

    private void DoAnimation()
    {
        if (doorComponent != null)
        {
            if (openned)
            {
                doorComponent.transform.position = MAX_POS;
            }
            else
            {
                doorComponent.transform.position = MIN_POS;
            }
        }
    }
}