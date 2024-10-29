using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] Animator animator;
    [SerializeField] Animator shadowAnimator;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] SpriteRenderer shadowRenderer;
    [SerializeField] Player player;
    [SerializeField] bool useRawInput = false;
    [HideInInspector] public Rigidbody2D body;
    Vector2 inputVector = Vector2.zero;

    [Header("Movement")]
    //[HideInInspector] public bool canMove = true;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float ghostSpeed = 2.5f;
    [SerializeField] float dashSpeed = 24.0f;
    [SerializeField] float dashTime = 0.2f;
    [SerializeField] float dashCooldown = 1.0f;
    [SerializeField] float attack1Delay = 0.462f;
    [SerializeField] float attack2Delay = 0.831f;
    [SerializeField] float attackCooldown = 1.0f;
    [SerializeField] float attackRadius = 2.0f;
    [SerializeField] LayerMask damageLayers;
    bool canDash = true;
    bool dashing = false;
    bool canAttack = true;
    bool attackMovement = false;
    bool attacking = false;

    [Header("SFX")]
    [SerializeField] AudioPlayer attackSwing;
    [SerializeField] AudioPlayer attackHit;
    [SerializeField] AudioPlayer dash;
    [SerializeField] AudioPlayer step;
    [SerializeField] float stepDelay = 0.25f;
    float currentStep = 0;

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
                    dash.Play();

                    CancelInvoke(nameof(Attack));

                    animator.SetTrigger("Dash");
                    shadowAnimator.SetTrigger("Dash");
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
                    shadowAnimator.SetTrigger("Attack");
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
        if (body.simulated)
        {
            float speed = dashing ? dashSpeed : moveSpeed;
            if (attackMovement || !player.living) speed /= 2;
            if (!(dashing || attackMovement)) inputVector = GetInput();
            if (!attacking) body.linearVelocity = inputVector * speed;

            //animations
            animator.SetFloat("xMotion", body.linearVelocity.magnitude);
            shadowAnimator.SetFloat("xMotion", body.linearVelocity.magnitude);
            if (body.linearVelocityX < 0.0f) renderer.flipX = true;
            if (body.linearVelocityX > 0.0f) renderer.flipX = false;
            if (body.linearVelocityX < 0.0f) shadowRenderer.flipX = true;
            if (body.linearVelocityX > 0.0f) shadowRenderer.flipX = false;
            if (player.living && body.linearVelocity.magnitude > 0.5f)
            {
                currentStep += Time.fixedDeltaTime;
                if (currentStep >= stepDelay)
                {
                    step.Play();
                    currentStep = 0.0f;
                }
            }
        }
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

    public void Attack()
    {
        attackSwing.Play();
        attacking = true;
        bool hit = false;

        //get all enemy positions
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.living && Vector3.Distance(enemy.transform.position, transform.position) < attackRadius)
            {
                hit = true;
                enemy.Kill();
            }
        }

        if (hit)
        {
            attackHit.Play();
        }
    }

    void StopAttack()
    {
        attacking = false;
        attackMovement = false;
    }
}
