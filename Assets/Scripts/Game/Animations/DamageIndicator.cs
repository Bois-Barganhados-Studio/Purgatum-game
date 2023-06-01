using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    void DestroyIndicator()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
