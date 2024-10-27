using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool useRawInput = false;
    Rigidbody2D body;
    Vector2 inputVector = Vector2.zero;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        inputVector = GetInput();
        //body.AddForce(inputVector * speed);
        body.linearVelocity = inputVector * speed;
    }

    Vector2 GetInput()
    {
        float x = useRawInput ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float y = useRawInput ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");
        return new Vector2(x, y).normalized;
    }
}
