using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    public enum MoveState
    {
        IDLE,
        MOVING,
        DODGING,
    }

    private bool isDead;
    public bool IsDead 
    {
        get { return isDead; }
    }    

    private int hp;
    public int Hp
    {
        get { return hp; }
        set {
            if (value <= 0) {
                hp = 0;
                isDead = true;
            } else {
                hp = value;
            }
        }
    }

    private Vector2 facingDir;
    public Vector2 FacingDir
    {
        get { return facingDir; }
    }
    private Vector2 direction;
    public Vector2 Direction
    {
        get { return direction; }
        set 
        { 
            if (value != new Vector2(0, 0)) {
                facingDir = value;
            }
            direction = value; 
        }
    }

    private MoveState lastState;
    private MoveState moveState;
    public MoveState Move_State
    {
        get { return moveState; }
        set 
        { 
            lastState = moveState;
            moveState = value;
        }
    }

    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private float dodgeSpeed;
    public float DodgeSpeed
    {
        get { return dodgeSpeed; }
        set { dodgeSpeed = value; }
    }

    private bool isAttaking;
    public bool IsAttaking
    {
        get { return isAttaking; }
        set { isAttaking = value; }
    }

    public Entity(int hp)
    {
        this.hp = hp;
        direction = new Vector2();
        moveState = MoveState.IDLE;
        moveSpeed = 1.0f;
        dodgeSpeed = 1.0f;
        isAttaking = false;
        isDead = false;
        facingDir = new Vector2(0, 1);
    }

    public void toLastState()
    {
        MoveState tmp = lastState;
        lastState = moveState;
        moveState = tmp;
    }

    public void takeAttack(int dmg)
    {
        Hp = Hp - dmg;
        Debug.Log(Hp);
        if (isDead) {
            Debug.Log("died");
        }
    }
}
