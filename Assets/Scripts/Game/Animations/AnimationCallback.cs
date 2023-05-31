using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    private PlayerObject po;

    void EndDodge()
    {
        po.EndDodge();
    }

    void EndDeath()
    {
        po.EndDeath();
    }

    void EndAttack()
    {
        po.EndAttack();
    }

    void DealDamage()
    {
        po.DealDamage();
    }

    // Initializing
    void Awake()
    {
        po = gameObject.transform.parent.GetComponent<PlayerObject>();
    }

}
