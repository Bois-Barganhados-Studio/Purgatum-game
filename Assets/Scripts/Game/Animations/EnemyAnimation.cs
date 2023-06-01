using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private EnemyObject enemy;

    private void Awake()
    {
        enemy = gameObject.transform.parent.GetComponent<EnemyObject>();
    }

    public void DealDamage()
    {
        enemy.DealDamage();
    }

    public void EndAttack()
    {
        enemy.EndAttack();
    }

    public void EndDeath()
    {
        enemy.EndDeath();
    }
}
