using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    private Enemy enemy;
    public GameObject DamageIndicator;

    public void Awake()
    {
        enemy = new Enemy(50);
    }


    public void takeAttack(int dmg)
    {
        if (enemy.takeAttack(dmg)) {
            StartCoroutine(blinkSprite());
            DamageIndicator.GetComponentInChildren<TextMesh>().text = dmg.ToString();
            Instantiate(DamageIndicator, transform.position, Quaternion.identity);
            if (enemy.IsDead) {
                Destroy(GetComponent<BoxCollider2D>());
                FindObjectOfType<EnemyAnimation>().die(enemy.FacingDir);
            }
        }
    }

    public IEnumerator blinkSprite()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Color lastColor = sr.color;
        sr.color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(.15f);
        sr.color = lastColor;
    }


}