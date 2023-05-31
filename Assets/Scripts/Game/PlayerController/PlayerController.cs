using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerObject player;
    public Animator animator;


    private void Awake()
    {
        //player = GetComponent<PlayerObject>();
    }

    private void FixedUpdate()
    {
        if (player.IsUpdateDisabled)
            return;
        if (player.IsAttacking())
        {
            player.rb.MovePosition(player.rb.position + player.AttackVelocity() * Time.deltaTime);
        }
        else if (player.GetMoveState() == Entity.MoveState.MOVING || player.GetMoveState() == Entity.MoveState.IDLE)
        {
            player.rb.MovePosition(player.rb.position + player.MoveVelocity() * Time.deltaTime);

        }
        else if (player.GetMoveState() == Entity.MoveState.DODGING)
        {
            player.rb.MovePosition(player.rb.position + player.DodgeVelocity() * Time.deltaTime);
        }
        animator.SetFloat("Horizontal", player.GetFacingDir().x);
        animator.SetFloat("Vertical", player.GetFacingDir().y);
        animator.SetFloat("Speed", player.MoveVelocity().sqrMagnitude);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        player.Move(ctx.ReadValue<Vector2>());
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.Dodge();
        }
    }

    public void EndDodge()
    {
        player.EndDodge();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.Attack();
        }
    }

    public void EndAttack()
    {
        player.EndAttack();
    }

    public void OnCollect(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.Collect();
        }
    }

    public void OnSwap(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            player.SwapWeapon();
        }
    }

    public void OnTest(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // Add whatever you want
            // test by pressing 'T'
            player.Test();

        }
    }
}
