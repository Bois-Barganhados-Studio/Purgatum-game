using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    private Enemy enemy;
    public GameObject DamageIndicator;
    public GameObject attackPoint;
    public LayerMask playerLayer;

    public void Awake()
    {
        enemy = new Enemy(50);
    }

    public void FixedUpdate()
    {
        Collider2D col = Physics2D.OverlapCircle(attackPoint.transform.position, 2 * enemy.MainWeapon.Range, playerLayer);
        PlayerObject p = null;
        if (col != null)
            p = col.GetComponent<PlayerObject>();
        if (p != null && !enemy.IsAttacking)
        {
            // TODO - make enemy look at player
            enemy.IsAttacking = true;
            StartCoroutine(enemy.coolDown(() => {
                enemy.IsAttacking = false;
            }, enemy.MainWeapon.Weight * Weapon.BASE_COOLDOWN));
            p.takeAttack(enemy.MainWeapon);
        }
    }


    public void takeAttack(Weapon pWeapon)
    {
        int dmg = enemy.takeAttack(pWeapon);
        if (dmg != 0) {
            StartCoroutine(blinkSprite());
            DamageIndicator.GetComponentInChildren<TextMesh>().text = dmg.ToString();
            Instantiate(DamageIndicator, transform.position, Quaternion.identity);
            if (enemy.IsDead) {
                FindObjectOfType<EnemyAnimation>().die(enemy.FacingDir);
                Destroy(this);
                // TODO - Destroy whole game object
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