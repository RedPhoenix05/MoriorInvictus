using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Player player;
    [SerializeField] bool useRawInput = false;
    Rigidbody2D body;
    Vector2 inputVector = Vector2.zero;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float ghostSpeed = 2.5f;
    [SerializeField] float dashSpeed = 24.0f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] float dashCooldown = 1.0f;
    [SerializeField] float attack1Delay = 0.462f;
    [SerializeField] float attack2Delay = 0.831f;
    [SerializeField] float attackCooldown = 1.0f;
    bool canDash = true;
    bool dashing = false;
    bool canAttack = true;
    bool attackMovement = false;
    bool attacking = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        //Disable some actions if in afterlife
        if (player.living)
        {
            //Dashing
            if (Input.GetButton("Jump") && body.linearVelocity.magnitude > 0)
            {
                if (!attackMovement && canDash)
                {
                    CancelInvoke(nameof(Attack));

                    animator.SetTrigger("Dash");
                    canDash = false;
                    Invoke(nameof(EnableDash), dashCooldown);
                    dashing = true;
                    Invoke(nameof(StopDash), dashTime);

                    //allow turning on exact frame cooldown
                    inputVector = GetInput();
                }
            }

            //Attacking
            if (Input.GetButtonDown("Fire4"))
            {
                if (!dashing && canAttack)
                {
                    animator.SetTrigger("Attack");
                    canAttack = false;
                    attackMovement = true;
                    Invoke(nameof(Attack), attack1Delay);
                    Invoke(nameof(StopAttack), attack1Delay + 0.2f);
                    attackMovement = true;
                    Invoke(nameof(Attack), attack2Delay);
                    Invoke(nameof(StopAttack), attack2Delay + 0.2f);
                    Invoke(nameof(EnableAttack), attackCooldown);
                }
            }
        }
    }

    void FixedUpdate()
    {
        float speed = dashing ? dashSpeed : moveSpeed;
        if (attackMovement || !player.living) speed /= 2;
        if (!(dashing || attackMovement)) inputVector = GetInput();
        if (!attacking) body.linearVelocity = inputVector * speed;

        //animations
        animator.SetFloat("xMotion", body.linearVelocity.magnitude);
        if (body.linearVelocityX < 0.0f) renderer.flipX = true;
        if (body.linearVelocityX > 0.0f) renderer.flipX = false;
    }

    Vector2 GetInput()
    {
        float x = useRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float y = useRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");
        return new Vector2(x, y).normalized;
    }

    void EnableDash()
    {
        canDash = true;
    }

    void StopDash()
    {
        dashing = false;
    }

    void EnableAttack()
    {
        canAttack = true;
    }
    void Attack()
    {
        attacking = true;
    }

    void StopAttack()
    {
        attacking = false;
        attackMovement = false;
    }
}
