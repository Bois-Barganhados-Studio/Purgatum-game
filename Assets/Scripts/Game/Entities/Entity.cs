using System;
using System.Collections;
using UnityEngine;

public abstract class Entity
{
    #region Vector Direction
    public Vector2 FacingDirection { get; set; }

    private Vector2 currentDirection;
    public Vector2 CurrentDirection
    {
        get => currentDirection;
        set
        {
            if (value != new Vector2(0, 0) && !LockedDir)
            {
                FacingDirection = value;
            }
            currentDirection = value;
        }
    }
    #endregion

    #region Movement

    public enum MoveState
    {
        IDLE,
        MOVING,
        DODGING,
    }

    public MoveState LastState { get; set; }

    private MoveState currentMoveState;
    public MoveState CurrentMoveState
    {
        get => currentMoveState;
        set
        {
            LastState = CurrentMoveState;
            currentMoveState = value;
        }
    }

    public float MoveSpeed { get; set; }

    public float DodgeSpeed { get; set; }

    public bool LockedDir { get; set; }

    public void ToLastState()
    {
        (LastState, CurrentMoveState) = (CurrentMoveState, LastState);
    }

    public void LockDir()
    {
        LockedDir = true;
    }

    public void UnlockDir()
    {
        LockedDir = false;
    }


    #endregion

    #region Attack
    public bool IsAttacking { get; set; }

    public bool TakeDamage(int dmg)
    {
        Hp -= dmg;
        return true;
    }

    #endregion

    #region Stats

    private int vitality;
    public int Vitality
    {
        get => vitality;
        set
        {
            vitality = value;
            MaxHp = vitality * 5;
            if (hp == 0)
            {
                hp = MaxHp;
            }
        }
    }

    public int Strength { get; set; }

    private int agility;
    public int Agility
    {
        get => agility;
        set
        {
            agility = value;
            DodgeSpeed = 1 + 0.0055f * agility;
        }
    }

    public int Defense { get; set; }
    public int Luck { get; set; }

    private int speed;
    public int Speed
    {
        get => speed;
        set
        {
            speed = value;
            MoveSpeed = 1 + 0.005f * speed;
        }
    }

    #endregion

    #region EntityState
    public bool IsDead { get; set; }

    public int MaxHp { get; set; }

    private int hp;
    public int Hp
    {
        get => hp;
        set
        {
            if (value <= 0)
            {
                hp = 0;
                IsDead = true;
            }
            else
            {
                hp = value;
            }
        }
    }
    #endregion

    #region Utils
    public IEnumerator CoolDown(Action func, float time)
    {
        yield return new WaitForSeconds(time);
        func();
    }
    #endregion

    protected Entity()
    {
        CurrentDirection = Vector2.zero;
        CurrentMoveState = MoveState.IDLE;
        IsAttacking = false;
        IsDead = false;
        FacingDirection = new Vector2(0, 1);
    }
}
