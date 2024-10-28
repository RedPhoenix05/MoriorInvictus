using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool living = true;

    public float health = 100.0f;
    public bool canResurrect = false;
    [SerializeField] string altarTag = "Resurrection Altar";
    [SerializeField] float resurrectionDelay = 3.0f;

    [SerializeField] Animator animator;
    [SerializeField] AnimatorController livingAnimation;
    [SerializeField] AnimatorController ghostAnimation;
    [SerializeField] GameObject LivingObjects;
    [SerializeField] GameObject GhostObjects;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == altarTag)
        {
            if (canResurrect) Invoke(nameof(Resurrect), resurrectionDelay);
            Die();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke(nameof(Resurrect));
    }

    void Resurrect()
    {
        animator.runtimeAnimatorController = livingAnimation;
        living = true;
    }

    void Die()
    {
        animator.runtimeAnimatorController = ghostAnimation;
        living = false;
    }
}
