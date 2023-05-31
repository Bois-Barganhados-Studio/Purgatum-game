using System;
using System.Collections;
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
    private SpriteRenderer sr;
    private EnemyAnimation eAnim;
    private PlayerObject p;
    private TextMesh textMesh;
    private Rigidbody2D rb;


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
        sr = GetComponentInChildren<SpriteRenderer>();
        eAnim = FindObjectOfType<EnemyAnimation>();
        p = FindObjectOfType <PlayerObject>();
        textMesh = DamageIndicator.GetComponentInChildren<TextMesh>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if (!updateEnabled)
            return;

        double distance = Math.Sqrt(Math.Pow(transform.position.x - target.position.x, 2) + Math.Pow(transform.position.y - target.position.y, 2));
        //UnityEngine.Debug.Log(distance);

        //Atualiza a máquina de estados
        UpdateMachineState(distance);
        //FindObjectOfType<EnemyAnimation>().SetMoveDirection(enemy.Direction);
        //UnityEngine.Debug.Log(enemy.State);

        switch (enemy.State)
        {
            case Enemy.MachineState.IDLE:
                pathfinder.canSearch = false;
                pathfinder.canMove = false;
                eAnim.idle(target);
                
                enemy.soundController.StopBattleSong();
                break;

            case Enemy.MachineState.CHASING:
                pathfinder.canSearch = true;
                pathfinder.canMove = true;
                pathfinder.destination = target.position;
                eAnim.moving(target);
                
                enemy.soundController.PlayBattleSong();
                break;

            case Enemy.MachineState.ATTACKING:
                eAnim.attacking(target);
                Collider2D col = Physics2D.OverlapCircle(attackPoint.transform.position, 2 * enemy.MainWeapon.Range, playerLayer);
                if (col != null && !enemy.IsAttacking)
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
                
                enemy.soundController.StopBattleSong();
                Die();
                break;

        }
    }

    public void Die()
    {
        eAnim.die(target, this.EndDeath);
        rb.simulated = false;
        DisableUpdate();
    }

    public void EndDeath()
    {
        Destroy(this.gameObject);
    }

    private void DisableUpdate()
    {
        updateEnabled = false;
    }

    public void TakeAttack(Weapon pWeapon)
    {
        int dmg = enemy.takeAttack(pWeapon);
        if (dmg != 0) {
            StartCoroutine(BlinkSprite());
            textMesh.text = dmg.ToString();
            Instantiate(DamageIndicator, transform.position, Quaternion.identity);
            // if (enemy.IsDead) {
            //     FindObjectOfType<EnemyAnimation>().die(enemy.FacingDir);
            //     //Destroy(this);
            //     // TODO - Destroy whole game object
            // }
        }
    }

    
    public void UpdateMachineState(double distance)
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

    public IEnumerator BlinkSprite()
    {
        Color lastColor = sr.color;
        sr.color = new Color(255, 255, 255, 1);
        yield return new WaitForSeconds(.15f);
        sr.color = lastColor;
    }


}