using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Speedometer))]
public class EnemyController : MonoBehaviour
{
    Rigidbody2D body;
    AIPath ai;
    Animator animator;
    SpriteRenderer renderer;
    Speedometer meter;

    [HideInInspector] public bool living
    {
        get => ai.enabled;
    }

    [SerializeField] float attackCooldown = 2.0f;
    [SerializeField] float attackRadius = 1.0f;
    [SerializeField] List<float> attackDelays = new();
    bool canAttack = true;
    float defaultMoveSpeed = 0.0f;

    [Header("Audio")]
    [SerializeField] AudioPlayer attackSwing;
    [SerializeField] AudioPlayer attackHit;
    [SerializeField] AudioPlayer death;
    [SerializeField] AudioPlayer step;
    [SerializeField] float stepDelay = 0.4f;
    float currentStep = 0.0f;

    void Start()
    {
        body = GetComponentInParent<Rigidbody2D>();
        ai = GetComponentInParent<AIPath>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        meter = GetComponent<Speedometer>();

        defaultMoveSpeed = ai.maxSpeed;
    }

    void Update()
    {
        if (ai.desiredVelocity.x < -0.01f)
        {
            renderer.flipX = true;
        }
        if (ai.desiredVelocity.x > 0.01f)
        {
            renderer.flipX = false;
        }

        if (ai.reachedEndOfPath && ai.target)
        {
            if (canAttack)
            {
                animator.SetTrigger("Attack");
                canAttack = false;
                ai.maxSpeed = defaultMoveSpeed / 2;
                Invoke(nameof(EnableAttack), attackCooldown);
                //schedule 1 attack for each set attack
                for (int i = 0; i < attackDelays.Count; i++)
                {
                    Invoke(nameof(Attack), attackDelays[i]);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat("xMotion", meter.linearVelocity.magnitude);
        if (meter.linearVelocity.magnitude > 0.5f)
        {
            currentStep += Time.fixedDeltaTime;
            if (currentStep >= stepDelay)
            {
                step.Play();
                currentStep = 0.0f;
            }
        }
    }

    void EnableAttack()
    {
        canAttack = true;
        ai.maxSpeed = defaultMoveSpeed;
    }

    void Attack()
    {
        attackSwing.Play();

        if (Vector3.Distance(ai.target.position, transform.position) <= attackRadius)
        {
            attackHit.Play();

            ai.target.GetComponent<Player>().Kill();
            AIPath[] enemies = FindObjectsByType<AIPath>(FindObjectsSortMode.None);
            foreach (AIPath enemy in enemies)
            {
                enemy.target = null;
            }
        }
    }

    public void Kill()
    {
        if (ai.enabled)
        {
            death.Play();

            renderer.sortingOrder = 90;
            animator.SetTrigger("Death");
            CancelInvoke(nameof(Attack));
            ai.enabled = false;
            body.simulated = false;
        }
    }
}
