using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    [SerializeField] float speed = 150.0f;

    [SerializeField] KeyCode upAction = KeyCode.W;
    [SerializeField] KeyCode leftAction = KeyCode.A;
    [SerializeField] KeyCode downAction = KeyCode.S;
    [SerializeField] KeyCode rightAction = KeyCode.D;
    Rigidbody2D body;
    Vector2 inputVector = Vector2.zero;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        inputVector = GetInput();
        body.AddForce(inputVector * speed);
    }

    Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(upAction)) input += Vector2.up;
        if (Input.GetKey(leftAction)) input += Vector2.left;
        if (Input.GetKey(downAction)) input += Vector2.down;
        if (Input.GetKey(rightAction)) input += Vector2.right;

        return input.normalized;
    }
}
