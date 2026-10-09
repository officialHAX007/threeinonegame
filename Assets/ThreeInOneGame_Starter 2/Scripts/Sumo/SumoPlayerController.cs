using UnityEngine;

/// <summary>
/// "I'm a Sumo and a Ball" - player-controlled sumo wrestler.
/// Controls: WASD / Arrow keys move around the ring; bumping into the opponent pushes it.
/// Put this on the player's Rigidbody object (a Capsule works fine).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SumoPlayerController : MonoBehaviour
{
    public float moveForce = 20f;
    public float maxSpeed = 6f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v);
        if (move.sqrMagnitude > 1f) move.Normalize();

        if (move.sqrMagnitude > 0.01f)
        {
            rb.AddForce(move * moveForce, ForceMode.Acceleration);
            transform.forward = Vector3.Lerp(transform.forward, move, 10f * Time.fixedDeltaTime);
        }

        // Unity 6 API (if you're on Unity 2021/2022/2023 instead, rename "linearVelocity" to "velocity").
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limited = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }
    }
}
