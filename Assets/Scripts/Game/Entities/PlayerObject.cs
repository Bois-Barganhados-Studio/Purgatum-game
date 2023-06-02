using System;
using System.Collections;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private static PlayerObject instance;
    public Player player;
    public LayerMask enemyLayer;
    public LayerMask itemLayer;
    public LayerMask destructibleLayer;
    public GameObject[] actionPoints;
    public SpriteRenderer[] actionPointsSR;
    public Animator[] apAnimator;
    public Rigidbody2D rb;
    private Vector2 idleDir;
    private WeaponObject mainWeapon;
    private WeaponObject subWeapon;
    private SpriteRenderer sr;
    public Animator animator;
    private float attackFactor;
    private int mapLevel;
    private Color currSpriteColor;


    private bool isUpdateDisabled;
    public bool IsUpdateDisabled
    {
        get { return isUpdateDisabled; }
    }
    private HealthBar HealthBarHud { get; set; }
    private Inventory HotBar { get; set; }

    // Initializing
    void Awake()
    {
        HealthBarHud = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(0).GetComponent<HealthBar>();
        HotBar = GameObject.FindGameObjectWithTag("HUD").transform.GetChild(1).GetComponent<Inventory>();
        mapLevel = 0;
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
        mainWeapon.gameObject.SetActive(false);
        mainWeapon.SetSpriteColor(Color.white);
        subWeapon.gameObject.SetActive(false);
        mainWeapon.weapon = player.MainWeapon;
        subWeapon.weapon = player.SubWeapon;
        Physics2D.IgnoreLayerCollision(Player.LAYER, Enemy.LAYER);
        Physics2D.IgnoreLayerCollision(Player.LAYER, IItem.LAYER);
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        destructibleLayer = LayerMask.GetMask("Destructables");
        UpdateWeaponVFX(ItemSprites.WEAPON_VFX_BASE);
        attackFactor = 1f;
        currSpriteColor = Color.white;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Caso duplique os gameobjects mantem apenas o original
            Destroy(gameObject);
        }
        UpdateHotBar();
    }

    void Update()
    {
        if (player.CurrentMoveState == Entity.MoveState.MOVING)
        {
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

    public Vector2 AttackVelocity()
    {
        Vector2 res = Vector2.zero;
        if (player.CurrentDirection == player.FacingDirection)
        {
            res = player.FacingDirection * player.MoveSpeed * attackFactor;
        }
        return res;
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

    public bool IsDirLocked()
    {
        return player.LockedDir;
    }

    public void SetMapLevel(int level)
    {
        mapLevel = level;
    }

    public IEnumerator CoolDown(Action func, float time)
    {
        yield return new WaitForSeconds(time);
        func();
    }

    public void Move(Vector2 dir)
    {

        if (player.CurrentMoveState != Entity.MoveState.DODGING)
        {
            if (dir == idleDir)
                player.CurrentMoveState = Entity.MoveState.IDLE;
            else
                player.CurrentMoveState = Entity.MoveState.MOVING;
        }
        SetDirection(dir);
    }

    public void Dodge()
    {
        if (player.CanDodge())
        {
            if (player.IsAttacking)
                EndAttack();
            player.CurrentMoveState = Entity.MoveState.DODGING;
            player.LockDir();
            player.DodgingCD = true;
            animator.SetBool("isRolling", true);
        }
    }

    public void EndDodge()
    {
        player.ToLastState();
        player.UnlockDir();
        animator.SetBool("isRolling", false);

        // TODO - COOLDOWN SCALE LOGIC
        StartCoroutine(player.CoolDown(() =>
        {
            player.DodgingCD = false;
        }, Player.BASE_COOLDOWN));
    }

    public void EndDeath()
    {
        gameObject.SetActive(false);

        // TODO - Go back to hub
        player.SkillPoints += mapLevel - 1;
        RogueLikeController rlc = FindObjectOfType<RogueLikeController>();
        rlc.OnGameRestart();
        player.IsDead = false;
        rb.simulated = true;
        isUpdateDisabled = false;
        animator.SetBool("isDying", false);
        player.Hp = player.MaxHp;
        UpdateHealthBar();
        gameObject.SetActive(true);
    }

    public static float WEIGHT_FACTOR = 5f;

    public void Attack()
    {
        if (player.CanAttack())
        {
            attackFactor = 0.2f;
            player.IsAttacking = true;
            player.LockDir();
            animator.SetBool("isAttacking", true);
            Debug.Log("weight: " +  player.MainWeapon.Weight);
            float attackSpeed = WEIGHT_FACTOR / player.MainWeapon.Weight;
            animator.SetFloat("attackSpeed", attackSpeed);
            int idx = DirectionToIndex(player.FacingDirection);
            apAnimator[idx].SetFloat("attackSpeed", attackSpeed);
            StartCoroutine(CoolDown(() =>
            {
                apAnimator[idx].SetTrigger("slash");
            }, 0.1f));
        }
    }

    public void DealDamage()
    {
        int idx = DirectionToIndex(player.FacingDirection);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(actionPoints[idx].transform.position, player.MainWeapon.Range * 0.5f, enemyLayer);
        foreach (var e in enemies)
        {
            e.GetComponent<EnemyObject>().TakeAttack(player.MainWeapon, player.DamageMultiplier, player.FacingDirection);
        }
        Collider2D[] destructibles = Physics2D.OverlapCircleAll(actionPoints[idx].transform.position, player.MainWeapon.Range, destructibleLayer);
        foreach (var d in destructibles)
        {
            d.GetComponent<Destructable>().DestroyObject();
        }
    }

    // TODO - DELETE THIS ON RELEASE VERSION
    private void OnDrawGizmosSelected()
    {
        foreach (var it in actionPoints)
        {
            Gizmos.DrawWireSphere(it.transform.position, 0.1f);
        }
    }

    public void EndAttack()
    {
        attackFactor = 1f;
        player.UnlockDir();
        player.IsAttacking = false;
        animator.SetBool("isAttacking", false);
        player.AttackingCD = true;
        StartCoroutine(player.CoolDown(() =>
        {
            player.AttackingCD = false;
        }, player.MainWeapon.Weight * Weapon.BASE_COOLDOWN));
    }

    public void TakeAttack(Weapon eWeapon, float eDamageMultiplier, Vector2 enemyFacingDir)
    {

        if (player.CurrentMoveState == Entity.MoveState.DODGING || player.IsDead || player.IsInvincible)
            return;
        int dmg = player.TakeAttack(eWeapon, eDamageMultiplier);
        isUpdateDisabled = true;
        rb.AddForce(enemyFacingDir * eWeapon.Weight * 0.1f, ForceMode2D.Impulse);
        StartCoroutine(CoolDown(() =>
        {
            isUpdateDisabled = false;
        }, 0.1f * eWeapon.Weight));
        player.IsInvincible = true;
        StartCoroutine(CoolDown(() =>
        {
            player.IsInvincible = false;
        }, 0.11f * eWeapon.Weight));

        if (dmg > 0)
        {
            UpdateHealthBar();
            if (player.IsDead)
            {
                rb.simulated = false;
                isUpdateDisabled = true;
                animator.SetBool("isDying", true);
            }
            else
            {
                StartCoroutine(BlinkSprite(Color.red));
            }
        }
    }

    public bool IsDead()
    {
        return player.IsDead;
    }

    public IEnumerator BlinkSprite(Color color)
    {
        sr.color = color;
        yield return new WaitForSeconds(.15f);
        sr.color = currSpriteColor;
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
        int idx = DirectionToIndex(player.FacingDirection);
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
            else if (col.TryGetComponent<Chest>(out var chest))
            {
                chest.Open(player.Luck);
            }
        }
    }

    private void CollectWeapon(WeaponObject newWeapon)
    {
        newWeapon.gameObject.transform.SetParent(this.transform);
        if (subWeapon.weapon != null)
        {
            mainWeapon.gameObject.transform.parent = null;
            mainWeapon.gameObject.transform.position = this.transform.position;
            mainWeapon.gameObject.SetActive(true);
            mainWeapon.Drop(30);
            mainWeapon = newWeapon;
            player.MainWeapon = mainWeapon.weapon;
            mainWeapon.gameObject.SetActive(false);
        }
        else
        {
            subWeapon = newWeapon;
            player.SubWeapon = subWeapon.weapon;
            subWeapon.gameObject.SetActive(false);
        }

        UpdateHotBar();
        UpdateWeaponVFX(mainWeapon.VfxSprites);
    }

    public void SwapWeapon()
    {
        if (subWeapon.weapon == null)
            return;
        (mainWeapon, subWeapon) = (subWeapon, mainWeapon);
        (player.MainWeapon, player.SubWeapon) = (mainWeapon.weapon, subWeapon.weapon);
        UpdateHotBar();
        UpdateWeaponVFX(mainWeapon.VfxSprites);
    }

    public void UpdateHotBar(bool swap = false)
    {
        HotBar.setSlots(mainWeapon.getWSprite(), subWeapon.getWSprite(), swap);
    }

    public void UpdateWeaponVFX(Sprite[] newvfx)
    {
        //if (actionPointsSR[0].sprite == newvfx[0])
        //    return;
        //for (int i = 0; i < actionPointsSR.Length; i++)
        //{
        //    actionPointsSR[i].sprite = newvfx[i];
        //}
    }

    private void CollectItem(ItemObject item)
    {
        item.Effect(this);
        Destroy(item.gameObject);
    }

    public void Heal(float healPct)
    {
        player.Heal(healPct);
        SetSpriteColor(Color.green, 0.3f);
    }

    private static readonly Color DEFENSE_COLOR = new(180, 0, 141);

    private static readonly Color DAMAGE_COLOR = new(240, 109, 65);

    private static readonly Color SPEED_COLOR = new(0, 168, 177);

    public void BoostSpeed(float BoostPct, float duration)
    {
        player.BoostSpeed(BoostPct);
        StartCoroutine(CoolDown(() =>
        {
            player.Speed = player.Speed;
        }, duration));
        SetSpriteColor(SPEED_COLOR, duration);
    }

    public void BoostDamage(float BoostPct, float duration)
    {
        player.BoostDamage(BoostPct);
        StartCoroutine(CoolDown(() =>
        {
            player.Strength = player.Strength;
        }, duration));
        SetSpriteColor(DAMAGE_COLOR, duration);
    }

    public void BoostDefense(float BoostPct, float duration)
    {
        player.BoostDefense(BoostPct);
        StartCoroutine(CoolDown(() =>
        {
            player.Defense = player.Defense;
        }, duration));
        SetSpriteColor(DEFENSE_COLOR, duration);
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

        var items = DropGenerator.GenerateDrop(69, 1);
        for (int i = 0; i < items.Count; i++)
        {
            items[i].gameObject.transform.position = this.transform.position + new Vector3(i, 1);
            items[i].gameObject.SetActive(true);
        }
    }

    public int DirectionToIndex(Vector2 _direction)
    {
        Vector2 norDir = _direction.normalized;//MARKER return this vector with a magnitude of 1 and get the normalized to an index

        float step = 360 / 8;//MARKER 45 one circle and 8 slices//Calcuate how many degrees one slice is
        float offset = step / 2;//MARKER 22.5//OFFSET help us easy to calcuate and get the correct index of the string array

        float angle = Vector2.SignedAngle(Vector2.up, norDir);//MARKER returns the signed angle in degrees between A and B

        angle += offset;//Help us easy to calcuate and get the correct index of the string array

        if (angle < 0)//avoid the negative number
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }

    internal void SetSpriteColor(Color color, float duration)
    {
        sr.color = color;
        StartCoroutine(CoolDown(() => {
            sr.color = currSpriteColor;
        }, duration));
    }
}
