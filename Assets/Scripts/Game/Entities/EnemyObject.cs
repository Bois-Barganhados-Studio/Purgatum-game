using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class EnemyObject : MonoBehaviour
{
    private Enemy enemy;
    public GameObject DamageIndicator;
    public GameObject attackPoint;
    public LayerMask playerLayer;
    private IAstarAI pathfinder;
    public Transform target;
    private bool updateEnabled;

    public void Awake()
    {
        enemy = new Enemy()
        {
            Vitality = 1,
            Strength = 1,
            Agility = 1,
            Defense = 1,
            Speed = 1,
            Luck = 1
        };
        pathfinder = GetComponent<IAstarAI>();
        pathfinder.maxSpeed = enemy.MoveSpeed;
        updateEnabled = true;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FixedUpdate()
    {
        if (!updateEnabled)
            return;

        double distance = Math.Sqrt(Math.Pow(transform.position.x - target.position.x, 2) + Math.Pow(transform.position.y - target.position.y, 2));
        //UnityEngine.Debug.Log(distance);

        //Atualiza a máquina de estados
        updateMachineState(distance);
        //FindObjectOfType<EnemyAnimation>().SetMoveDirection(enemy.Direction);
        //UnityEngine.Debug.Log(enemy.State);

        switch (enemy.State)
        {
            case Enemy.MachineState.IDLE:
                pathfinder.canSearch = false;
                pathfinder.canMove = false;
                FindObjectOfType<EnemyAnimation>().idle(target);
                break;

            case Enemy.MachineState.CHASING:
                pathfinder.canSearch = true;
                pathfinder.canMove = true;
                pathfinder.destination = target.position;
                FindObjectOfType<EnemyAnimation>().moving(target);
                break;

            case Enemy.MachineState.ATTACKING:
                FindObjectOfType<EnemyAnimation>().attacking(target);

                Collider2D col = Physics2D.OverlapCircle(attackPoint.transform.position, 2 * enemy.MainWeapon.Range, playerLayer);
                PlayerObject p = null;
                if (col != null)
                    p = col.GetComponent<PlayerObject>();
                if (p != null && !enemy.IsAttacking)
                {
                    // TODO - make enemy look at player
                    enemy.IsAttacking = true;
                    StartCoroutine(enemy.CoolDown(() =>
                    {
                        enemy.IsAttacking = false;
                    }, enemy.MainWeapon.Weight * Weapon.BASE_COOLDOWN));
                    p.takeAttack(enemy.MainWeapon);

                }
                break;

            case Enemy.MachineState.DYING:
                pathfinder.canSearch = false;
                pathfinder.canMove = false;
                Die();
                break;

        }
    }

    public void Die()
    {
        FindObjectOfType<EnemyAnimation>().die(target, this.EndDeath);
        disableUpdate();
    }

    public void EndDeath()
    {
        Destroy(this.gameObject);
    }

    private void disableUpdate()
    {
        updateEnabled = false;
    }

    public void takeAttack(Weapon pWeapon)
    {
        int dmg = enemy.takeAttack(pWeapon);
        if (dmg != 0) {
            StartCoroutine(blinkSprite());
            DamageIndicator.GetComponentInChildren<TextMesh>().text = dmg.ToString();
            Instantiate(DamageIndicator, transform.position, Quaternion.identity);
            // if (enemy.IsDead) {
            //     FindObjectOfType<EnemyAnimation>().die(enemy.FacingDir);
            //     //Destroy(this);
            //     // TODO - Destroy whole game object
            // }
        }
    }

    
    public void updateMachineState(double distance)
    {
        if(enemy.State == Enemy.MachineState.DYING)
        {
            return;
        }
        if(enemy.IsDead)
        {
            enemy.State = Enemy.MachineState.DYING;
        }
        else if(distance > 2)
        {
            enemy.State = Enemy.MachineState.IDLE;
        }
        else if(distance <= 2 && distance > 0.2)
        {
            enemy.State = Enemy.MachineState.CHASING;
        }
        else if(distance <= 2 * enemy.MainWeapon.Range)
        {
            enemy.State = Enemy.MachineState.ATTACKING;
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