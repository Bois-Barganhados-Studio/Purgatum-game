using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Doors : MonoBehaviour
{
    private bool openned = true;
    public void SetState(bool openned)
    {
        this.openned = openned;
    }
}