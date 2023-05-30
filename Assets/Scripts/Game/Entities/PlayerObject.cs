using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerObject : MonoBehaviour
{
    public Player player;
    public LayerMask enemyLayer;
    public LayerMask itemLayer;
    public GameObject[] actionPoints;
    public Rigidbody2D rb;
    private Vector2 idleDir;
    private WeaponObject mainWeapon;
    private WeaponObject subWeapon;
    private SpriteRenderer sr;
    public PlayerAnimation pAnim;

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
        mainWeapon = transform.Find("mainWeapon").GetComponent<WeaponObject>();
        subWeapon = transform.Find("subWeapon").GetComponent<WeaponObject>();
        Physics2D.IgnoreLayerCollision(Player.LAYER, Enemy.LAYER);
        Physics2D.IgnoreLayerCollision(Player.LAYER, IItem.LAYER);
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        pAnim = FindObjectOfType<PlayerAnimation>();
        mainWeapon.gameObject.SetActive(false);
        subWeapon.gameObject.SetActive(false);
    }

    void Update()
    {
        if(player.CurrentMoveState == Entity.MoveState.MOVING)
        {
            //Debug.Log("Player is moving");
            player.soundController.PlaySoundEffect("player_step");            
        }
    }

    public Vector2 MoveVelocity()
    {
        return player.MoveVelocity();
    }

    public Vector2 DodgeVelocity()
    {
        return player.DodgeVelocity();
    }

    public Vector2 GetDirection()
    {
        return player.CurrentDirection;
    }

    public void SetDirection(Vector2 dir)
    {
        player.CurrentDirection = dir;
    }

    public Vector2 GetFacingDir()
    {
        return player.FacingDirection;
    }

    public Entity.MoveState GetMoveState()
    {
        return player.CurrentMoveState;
    }

    public void SetMoveState(Entity.MoveState state)
    {
        player.CurrentMoveState = state;
    }

    public bool IsAttacking()
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
        SetDirection(dir);
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
        Debug.Log("Player is dead");
        // TODO - Game Over
        //gameObject.SetActive(false);
    }

    public void Attack()
    {
        if (player.CanAttack()) {
            player.IsAttacking = true;
            int idx = pAnim.DirectionToIndex(player.FacingDirection);
            pAnim.SetAttackDirection(player.FacingDirection);
            Collider2D[] enemies = Physics2D.OverlapCircleAll(actionPoints[idx].transform.position, player.MainWeapon.Range, enemyLayer);
            foreach (var e in enemies) {
                e.GetComponent<EnemyObject>().TakeAttack(player.MainWeapon);
            }
            //StartCoroutine(player.CoolDown(() => {
            //    EndAttack();
            //}, player.MainWeapon.Weight * Weapon.BASE_COOLDOWN));
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var it in actionPoints)
        {
            Gizmos.DrawWireSphere(it.transform.position, 0.1f);
        }
    }

    public void EndAttack()
    {
        player.IsAttacking = false;
    }

    public void TakeAttack(Weapon eWeapon)
    {
        if (player.CurrentMoveState == Entity.MoveState.DODGING || player.IsDead)
            return;
        int dmg = player.TakeAttack(eWeapon);
        if (dmg > 0)
        {
            UpdateHealthBar();
            if (player.IsDead)
            {
                rb.simulated = false;
                isUpdateDisabled = true;
                pAnim.SetDyingDirection(player.CurrentDirection);
                this.EndDeath();

            } else
            {
                StartCoroutine(BlinkSprite());
            }
        }
    }

    public bool IsDead()
    {
        return player.IsDead;
    }

    public IEnumerator BlinkSprite()
    {
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
        int idx = pAnim.DirectionToIndex(player.FacingDirection);
        // TODO - Call player collection animation
        var col = Physics2D.OverlapCircle(actionPoints[idx].transform.position, 0.05f, itemLayer);
        if (col != null)
        {
            if (col.TryGetComponent<ItemObject>(out var item))
            {
                CollectItem(item);
            }
            else if (col.TryGetComponent<WeaponObject>(out var weapon))
            {
                CollectWeapon(weapon);
            }
        }
    }

    private void CollectWeapon(WeaponObject newWeapon)
    {
        // Drop
        mainWeapon.gameObject.transform.parent = null;
        mainWeapon.gameObject.transform.position = this.transform.position;
        mainWeapon.gameObject.SetActive(true);
        StartCoroutine(mainWeapon.Drop(10));

        // Collect
        newWeapon.gameObject.transform.SetParent(this.transform);
        mainWeapon = newWeapon;
        player.MainWeapon = mainWeapon.weapon;
        mainWeapon.gameObject.SetActive(false);
        UpdateHotBar();
    }

    public void SwapWeapon()
    {
        if (subWeapon == null)
            return;
        (mainWeapon, subWeapon) = (subWeapon, mainWeapon);
        (player.MainWeapon, player.SubWeapon) = (mainWeapon.weapon, subWeapon.weapon);
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

    public void BoostSpeed(float BoostPct, float duration)
    {
        player.BoostSpeed(BoostPct, duration);
    }

    public void BoostDamage(float BoostPct, float duration)
    {
        player.BoostDamage(BoostPct, duration);
    }

    public void BoostDefense(float BoostPct, float duration)
    {
        player.BoostDefense(BoostPct, duration);
    }

    public int GetLuck()
    {
        return player.Luck;
    }

    public void Test()
    {
        // Working potion spawn test
        //var prefab = Resources.Load<GameObject>("Prefab/Game/Entities/Item");
        //if (prefab != null)
        //{
        //    var item = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        //    item.GetComponent<ItemObject>().Init(new HealPotion(Potion.LEVEL.BASIC), null);
        //}

        //var prefab = Resources.Load<GameObject>("Prefab/Game/Entities/Weapon");
        //if (prefab != null)
        //{
        //    var weapon = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);
        //    weapon.GetComponent<WeaponObject>().Init(new DefaultWeapon(), weapon.GetComponent<SpriteRenderer>().sprite, true);
        //}

        DropGenerator.GenerateDrop(69, 69, 1);
    }

}   
