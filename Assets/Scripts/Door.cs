using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Door : MonoBehaviour
{
    [SerializeField] Sprite doorClosed;
    [SerializeField] Sprite doorOpened;

    [SerializeField] Collider2D doorCollider;

    SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Open()
    {
        renderer.sprite = doorOpened;
        doorCollider.enabled = false;
    }

    public void Close()
    {
        renderer.sprite = doorClosed;
        doorCollider.enabled = true;
    }
}
