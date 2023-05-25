using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private Player player;
    public LayerMask enemyLayer;
    public LayerMask itemLayer;
    public GameObject[] attackPoints;
    private Vector2 idleDir;
    private WeaponObject mw;
    private WeaponObject sw;

    private bool isUpdateDisabled;
    public bool IsUpdateDisabled
    {
        get { return isUpdateDisabled; }
    }
    private HealthBar HealthBarHud { get; set; }

    // Initializing
    void Awake()
    {
        HealthBarHud = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(0).GetComponent<HealthBar>();
        player = new Player
        {
            Vitality = 20,
            Strength = 1,
            Agility = 1,
            Defense = 1,
            Speed = 1,
            Luck = 1
        };
        idleDir = Vector2.zero;
        isUpdateDisabled = false;
        mw = transform.Find("Weapon1").GetComponent<WeaponObject>();
        sw = transform.Find("Weapon2").GetComponent<WeaponObject>();
        Physics2D.IgnoreLayerCollision(Player.LAYER, Enemy.LAYER);
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
        return player.CurrentDirection;
    }

    public void setDirection(Vector2 dir)
    {
        player.CurrentDirection = dir;
    }

    public Vector2 getFacingDir()
    {
        return player.FacingDirection;
    }

    public Entity.MoveState getMoveState()
    {
        return player.CurrentMoveState;
    }

    public void setMoveState(Entity.MoveState state)
    {
        player.CurrentMoveState = state;
    }

    public bool isAttacking()
    {
        return player.IsAttacking;
    }

    public IEnumerator coolDown(Action func, float time)
    {
        yield return new WaitForSeconds(time);
        func();
    }

    public void Move(Vector2 dir)
    {
        if (player.CurrentMoveState != Entity.MoveState.DODGING) {
            if (dir == idleDir)
                player.CurrentMoveState = Entity.MoveState.IDLE;
            else 
                player.CurrentMoveState = Entity.MoveState.MOVING;
        }
        setDirection(dir);
    }

    //public void EndMove()
    //{
    //    if (player.LastState == Entity.MoveState.MOVING) {
    //        player.LastState = Entity.MoveState.IDLE;
    //    } else if (player.CurrentMoveState == Entity.MoveState.MOVING) {
    //        player.toLastState();
    //    }
    //    player.Direction = idleDir; 
    //}

    public void Dodge()
    {
        if (player.CanDodge()) {
            player.CurrentMoveState = Entity.MoveState.DODGING;
            player.LockDir();
            player.DodgingCD = true;
        }
    }

    public void EndDodge()
    {
        player.ToLastState();
        player.UnlockDir();
        StartCoroutine(player.CoolDown(() => {
            player.DodgingCD = false;
        }, Player.BASE_COOLDOWN));
    }

    public void EndDeath()
    {
        // TODO - Game Over

    }

    public void Attack()
    {
        if (player.CanAttack()) {
            player.IsAttacking = true;
            int idx = FindObjectOfType<PlayerAnimation>().DirectionToIndex(player.FacingDirection);
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoints[idx].transform.position, player.MainWeapon.Range, enemyLayer);
            foreach (var e in enemies) {
                e.GetComponent<EnemyObject>().takeAttack(player.MainWeapon);
            }
            StartCoroutine(player.CoolDown(() => {
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
        if (player.CurrentMoveState == Entity.MoveState.DODGING || player.IsDead)
            return;
        int dmg = player.takeAttack(eWeapon);
        if (dmg > 0)
        {
            UpdateHealthBar();
            if (player.IsDead)
            {
                Debug.Log("died");
                GetComponent<Rigidbody2D>().simulated = false;
                isUpdateDisabled = true;
                FindObjectOfType<PlayerAnimation>().SetDyingDirection(player.CurrentDirection);
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
        HealthBarHud.setHealth(player.Hp);
    }

    public void UpdateMaxHealthBar()
    {
        HealthBarHud.setMaxHealth(player.MaxHp);
    }

    public void Collect()
    {
        if (!player.CanCollect())
            return;
        int idx = FindObjectOfType<PlayerAnimation>().DirectionToIndex(player.FacingDirection);
        // TODO - Call player collection animation
        var col = Physics2D.OverlapCircle(attackPoints[idx].transform.position, 0.05f, itemLayer);
        if (col != null)
        {
            var item = col.GetComponent<ItemObject>();
            if (item != null)
                CollectItem(item);
            else
            {
                var weapon = col.GetComponent<WeaponObject>();
                if (weapon != null)
                {
                    CollectWeapon(weapon);
                }
            }
        }
    }

    private void CollectWeapon(WeaponObject newWeapon)
    {
        DropWeapon();
        mw = newWeapon;
        player.MainWeapon = mw.weapon;
        UpdateHotBar();
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

    private void CollectItem(ItemObject item)
    {
        item.Effect(this);
        Destroy(item.gameObject);
    }

    public void Heal(float healPct)
    {
        player.Heal(healPct);
    }

}   
