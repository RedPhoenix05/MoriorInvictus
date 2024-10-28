using UnityEngine;

public class Speedometer : MonoBehaviour
{
    public Vector3 linearVelocity = Vector3.zero;
    Vector3 oldPosition = Vector3.zero;

    protected void FixedUpdate()
    {
        linearVelocity = (transform.position - oldPosition) / Time.fixedDeltaTime;
        oldPosition = transform.position;
    }
}
