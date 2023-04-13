using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    private Enemy enemy;

    public void Awake()
    {
        enemy = new Enemy(50);
        
    }

    public void takeAttack(int dmg)
    {
        enemy.takeAttack(dmg);

        // SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        // Color lastColor = sr.color;
        // sr.color = new Color();
        // TODO - wait (prob do it with update)
        // sr.color = lastColor;
    }
}