using Pathfinding;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    MovementController controller;

    public int souls = 1;

    [HideInInspector] public bool living = true;

    public bool canResurrect = false;
    [SerializeField] string altarTag = "Resurrection Altar";
    [SerializeField] float resurrectionDelay = 3.0f;
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

    [Header("Audio")]
    [SerializeField] AudioSource livingAmbience;
    [SerializeField] AudioSource deadAmbience;
    [SerializeField] AudioPlayer deathSFX;
    [SerializeField] AudioSource resurrectionSFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == altarTag)
        {
            if (canResurrect)
            {
                Invoke(nameof(Resurrect), resurrectionDelay);
                resurrectionSFX.Play();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke(nameof(Resurrect));
        resurrectionSFX.Stop();
    }

    void Start()
    {
        controller = GetComponent<MovementController>();
        livingAmbience.Play();
    }

    public void Resurrect()
    {
        canResurrect = false;
        souls--;
        resurrectionSFX.Stop();

        LivingObjects.SetActive(true);
        GhostObjects.SetActive(false);

        deadAmbience.Stop();
        livingAmbience.Play();
        resurrectionSFX.Play();

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
        canResurrect = souls > 0;

        LivingObjects.SetActive(false);
        GhostObjects.SetActive(true);

        livingAmbience.Stop();
        deadAmbience.Play();
        deathSFX.Play();

        living = false;
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
        if (renderer.flipX && obj.GetComponent<SpriteRenderer>()) obj.GetComponent<SpriteRenderer>().flipX = true;

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
