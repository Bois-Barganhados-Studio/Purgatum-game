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
    private PlayerObject p;
    private TextMesh textMesh;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 direction;


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
        p = FindObjectOfType <PlayerObject>();
        textMesh = DamageIndicator.GetComponentInChildren<TextMesh>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(Enemy.LAYER, IItem.LAYER);
        animator = GetComponentInChildren<Animator>();
        direction = Vector2.down;
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

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        
        
        

        switch (enemy.State)
        {
            case Enemy.MachineState.IDLE:
                pathfinder.canSearch = false;
                pathfinder.canMove = false;
                animator.SetFloat("Speed", 0f);
                enemy.soundController.StopBattleSong();
                break;

            case Enemy.MachineState.CHASING:
                direction = Vector2.MoveTowards(direction, p.GetDirection(), Time.deltaTime);
                animator.SetFloat("Speed", direction.sqrMagnitude);

                pathfinder.canSearch = true;
                pathfinder.canMove = true;
                pathfinder.destination = target.position;
                
                enemy.soundController.PlayBattleSong();
                break;

            case Enemy.MachineState.ATTACKING:
                if (!enemy.IsAttacking)
                {
                    pathfinder.canMove = false;
                    animator.SetTrigger("Attack");
                    enemy.IsAttacking = true;
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

    public bool CanAttack()
    {
        return !enemy.IsAttacking && !enemy.AttackingCD;
    }

    public void DealDamage()
    {
        Collider2D col = Physics2D.OverlapCircle(attackPoint.transform.position, 2 * enemy.MainWeapon.Range, playerLayer);
        if (col != null)
        {
            p.TakeAttack(enemy.MainWeapon, enemy.FacingDirection);
        }
    }

    public void EndAttack()
    {
        pathfinder.canMove = true;
        enemy.IsAttacking = false;
        enemy.AttackingCD = true;
        StartCoroutine(enemy.CoolDown(() =>
        {
            enemy.AttackingCD = false;
        }, enemy.MainWeapon.Weight * Weapon.BASE_COOLDOWN));
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        rb.simulated = false;
        DisableUpdate();
    }

    public void EndDeath()
    {
        StartCoroutine(enemy.CoolDown(() =>
        {
            Destroy(this.gameObject);
        }, 1));
        
    }

    private void DisableUpdate()
    {
        updateEnabled = false;
    }

    public void TakeAttack(Weapon pWeapon, Vector2 playerFacingDir)
    {
        if (enemy.IsDead)
            return;
        int dmg = enemy.takeAttack(pWeapon);
        if (dmg != 0) {
            updateEnabled = false;
            rb.AddForce(playerFacingDir * pWeapon.Weight * 0.1f, ForceMode2D.Impulse);
            StartCoroutine(enemy.CoolDown(() =>
            {
                updateEnabled = true;
            }, 0.1f * pWeapon.Weight));
            StartCoroutine(BlinkSprite());
            textMesh.text = dmg.ToString();
            Instantiate(DamageIndicator, transform.position, Quaternion.identity);
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
        else if (distance > 2)
        {
            enemy.State = Enemy.MachineState.IDLE;
        }
        else if(distance <= 2 && distance > 0.2)
        {
            enemy.State = Enemy.MachineState.CHASING;
        }
        else if(distance <= 2 * enemy.MainWeapon.Range && CanAttack())
        {
            enemy.State = Enemy.MachineState.ATTACKING;
        }
    }

    public IEnumerator BlinkSprite()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(.15f);
        sr.color = Color.white;
    }


}