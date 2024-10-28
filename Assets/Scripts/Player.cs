using Pathfinding;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    MovementController controller;

    public bool living = true;

    public bool canResurrect = false;
    [SerializeField] string altarTag = "Resurrection Altar";
    [SerializeField] float resurrectionDelay = 2.0f;
    [SerializeField] float transitionTime = 4.0f;

    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorController livingAnimation;
    [SerializeField] AnimatorController ghostAnimation;
    [SerializeField] GameObject LivingObjects;
    [SerializeField] GameObject GhostObjects;

    [SerializeField] GameObject deadBody;
    [SerializeField] GameObject shadow;

    bool animatingGhost = false;
    float time = 0.0f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == altarTag)
        {
            if (canResurrect) Invoke(nameof(Resurrect), resurrectionDelay);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke(nameof(Resurrect));
    }

    void Start()
    {
        controller = GetComponent<MovementController>();
    }

    public void Resurrect()
    {
        animator.runtimeAnimatorController = livingAnimation;
        living = true;

        controller.body.simulated = false;
        EnableMove();
        shadow.SetActive(true);

        //set all AI to target player again
        AIPath[] enemies = FindObjectsByType<AIPath>(FindObjectsSortMode.None);
        foreach (AIPath enemy in enemies)
        {
            enemy.target = transform;
        }
    }

    public void Kill()
    {
        living = false;
        canResurrect = true;
        shadow.SetActive(false);

        controller.body.simulated = false;
        Invoke(nameof(EnableMove), transitionTime + 1.1f);

        animator.SetTrigger("Death");
        Invoke(nameof(AnimateGhost), 1.1f);

        controller.CancelInvoke(nameof(MovementController.Attack));
    }

    void EnableMove()
    {
        controller.body.simulated = true;
        animatingGhost = false;

        renderer.color = new Color(1, 1, 1, 1);
    }

    void AnimateGhost()
    {
        GameObject obj = Instantiate(deadBody);
        obj.transform.position = transform.position;

        renderer.color = new Color(1, 1, 1, 0);
        animator.runtimeAnimatorController = ghostAnimation;
        animatingGhost = true;
        time = -2.0f;
    }

    void FixedUpdate()
    {
        if (animatingGhost)
        {
            renderer.color = new Color(1, 1, 1, Mathf.Lerp(0.0f, 1.0f, time / transitionTime));
            time += Time.fixedDeltaTime;
        }
    }
}
