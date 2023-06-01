using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Doors : MonoBehaviour
{
    private bool openned = true;
    private const string DOOR_COMPONENT = "Door";
    private GameObject doorComponent;

    void Awake()
    {
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
            doorComponent.GetComponent<PolygonCollider2D>().enabled = !openned;
            gameObject.SetActive(!openned);
        }
    }
}