using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private Player player;
    public LayerMask enemyLayer;
    public LayerMask itemLayer;
    public LayerMask weaponLayer;
    public GameObject[] attackPoints;
    private Vector2 idleDir;
    private WeaponObject mw;
    private WeaponObject sw;


    // Initializing
    void Awake()
    {
        player = new Player();
        idleDir = new Vector2(0, 0);
        mw = transform.Find("Weapon1").GetComponent<WeaponObject>();
        sw = transform.Find("Weapon2").GetComponent<WeaponObject>();
        Physics2D.IgnoreLayerCollision(Player.LAYER, Enemy.LAYER);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector2 MoveVelocity()
    {
        return player.MoveVelocity();
    }

    public Vector2 DodgeVelocity()
    {
        return player.DodgeVelocity();
    }

    public Vector2 getDirection()
    {
        return player.Direction;
    }

    public void setDirection(Vector2 dir)
    {
        player.Direction = dir;
    }

    public Vector2 getFacingDir()
    {
        return player.FacingDir;
    }

    public Entity.MoveState getMoveState()
    {
        return player.Move_State;
    }

    public void setMoveState(Entity.MoveState state)
    {
        player.Move_State = state;
    }

    public bool isAttacking()
    {
        return player.IsAttacking;
    }

    public void toLastState()
    {
        player.toLastState();
    }

    public void Move(Vector2 dir)
    {
        if (player.Move_State != Entity.MoveState.DODGING) {
            player.Move_State = Entity.MoveState.MOVING;
        }
        setDirection(dir);
    }

    public void EndMove()
    {
        if (player.LastState == Entity.MoveState.MOVING) {
            player.LastState = Entity.MoveState.IDLE;
        } else if (player.Move_State == Entity.MoveState.MOVING) {
            player.toLastState();
        }
        player.Direction = idleDir; 
    }

    public void Dodge()
    {
        if (player.CanDodge()) {
            player.Move_State = Entity.MoveState.DODGING;
            player.LockDir();
            player.DodgingCD = true;
        }
    }

    public void EndDodge()
    {
        player.toLastState();
        StartCoroutine(player.coolDown(() => {
            player.DodgingCD = false;
            player.UnlockDir();
        }, Player.BASE_COOLDOWN));
    }

    public void Attack()
    {
        if (player.CanAttack()) {
            player.IsAttacking = true;
            int idx = FindObjectOfType<PlayerAnimation>().DirectionToIndex(player.FacingDir);
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoints[idx].transform.position, player.MainWeapon.Range, enemyLayer);
            foreach (var e in enemies) {
                e.GetComponent<EnemyObject>().takeAttack(player.MainWeapon);
            }
            StartCoroutine(player.coolDown(() => {
                EndAttack();
            }, player.MainWeapon.Weight * Weapon.BASE_COOLDOWN));
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var it in attackPoints)
        {
            Gizmos.DrawWireSphere(it.transform.position, 0.1f);
        }
    }

    public void EndAttack()
    {
        player.IsAttacking = false;
    }

    public void takeAttack(Weapon eWeapon)
    {
        if (player.Move_State == Entity.MoveState.DODGING || player.IsDead)
            return;
        int dmg = player.takeAttack(eWeapon);
        if (dmg > 0)
        {
            UpdateHealthBar();
            if (player.IsDead)
            {
                GetComponent<Rigidbody2D>().simulated = false;
                GetComponent<PlayerController>().DisableUpdate();
                FindObjectOfType<PlayerAnimation>().SetDyingDirection(player.Direction);
                // TODO - Game Over
            } else
            {
                StartCoroutine(blinkSprite());
            }
        }
    }

    public bool IsDead()
    {
        return player.IsDead;
    }

    public IEnumerator blinkSprite()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Color lastColor = sr.color;
        sr.color = new Color(255, 255, 255, 0.5f);
        yield return new WaitForSeconds(.15f);
        sr.color = lastColor;
    }

    public void UpdateHealthBar()
    {
        GameObject.FindGameObjectWithTag("HUD").transform.GetChild(0).GetComponent<HealthBar>().setHealth(player.Hp);
    }

    public void CollectWeapon()
    {
        if (player.CanCollect())
        {
            int idx = FindObjectOfType<PlayerAnimation>().DirectionToIndex(player.FacingDir);
            Collider2D col = Physics2D.OverlapCircle(attackPoints[idx].transform.position, 0.05f, weaponLayer);
            if (col != null)
            {
                DropWeapon();
                mw = col.GetComponent<WeaponObject>();
                player.MainWeapon = mw.weapon;
                UpdateHotBar();
            }
        }
    }

    public void DropWeapon()
    {
        WeaponObject tmp = mw;
        mw = null;
        player.DropWeapon();
        UpdateHotBar();
        Instantiate(tmp, this.transform.position, Quaternion.identity);
        // TODO - Move drop away from the player
    }

    public void SwapWeapon()
    {
        var tmp = player.MainWeapon;
        player.MainWeapon = sw.weapon;
        player.SubWeapon = mw.weapon;
        UpdateHotBar();
    }

    public void UpdateHotBar()
    {
        // TODO - Update Hot Bar
    }

    public void Heal(float healPct)
    {
        player.Heal(healPct);
    }

    //public void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col != null)
    //        Debug.Log(col);
    //}
}   
