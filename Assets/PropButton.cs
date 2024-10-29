using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class PropButton : MonoBehaviour
{
    [SerializeField] Sprite buttonUnpressed;
    [SerializeField] Sprite buttonPressed;

    [SerializeField] UnityEvent pressEvent = null;
    [SerializeField] UnityEvent unpressEvent = null;

    SpriteRenderer renderer;
    [HideInInspector] public bool pressed = false;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        renderer.sprite = buttonPressed;
        if (!pressed) pressEvent.Invoke();
        pressed = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        renderer.sprite = buttonUnpressed;
        if (pressed) unpressEvent.Invoke();
        pressed = false;
    }
}
