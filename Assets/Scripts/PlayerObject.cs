using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    private Player player;
    public LayerMask enemyLayer;
    public GameObject[] attackPoints;
    // Initializing
    void Awake()
    {
        player = new Player();
        Physics2D.IgnoreLayerCollision(Player.LAYER, Enemy.LAYER);
        /*attackPoints = new GameObject[8];
        attackPoints[0] = GameObject.Find("AttackPointN");
        attackPoints[1] = GameObject.Find("AttackPointNW");
        attackPoints[2] = GameObject.Find("AttackPointW");
        attackPoints[3] = GameObject.Find("AttackPointSW");
        attackPoints[4] = GameObject.Find("AttackPointS");
        attackPoints[5] = GameObject.Find("AttackPointSE");
        attackPoints[6] = GameObject.Find("AttackPointE");
        attackPoints[7] = GameObject.Find("AttackPointNE");*/
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

    public bool isAttaking()
    {
        return player.IsAttaking;
    }

    public void toLastState()
    {
        player.toLastState();
    }

    public void Move(Vector2 dir)
    {
        if (player.Move_State != Entity.MoveState.DODGING) {
            player.Move_State = Entity.MoveState.MOVING;
            setDirection(dir);
        }
    }

    public void Dodge()
    {
        if (player.CanDodge()) {
            Debug.Log("can dodge");
            player.Move_State = Entity.MoveState.DODGING;
        }
    }

    public void EndDodge()
    {
        player.toLastState();
        Debug.Log("end dodge - ms: " + player.Move_State);
    }

    public void Attack()
    {
        if (player.CanAttack()) {
            player.IsAttaking = true;
            int idx = FindObjectOfType<PlayerAnimation>().DirectionToIndex(player.FacingDir);
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoints[idx].transform.position, 0.1f, enemyLayer);
            foreach (var e in enemies) {
                e.GetComponent<EnemyObject>().takeAttack(5);
            }
            player.Attack();
            EndAttack();
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
        //Debug.Log("end atk");
        player.IsAttaking = false;
    }
}   
