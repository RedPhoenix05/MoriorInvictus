using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CreditsScroll : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] float scrollSpeed = 5.0f;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.linearVelocity = Vector2.up * scrollSpeed;
    }
}
