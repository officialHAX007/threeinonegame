using UnityEngine;

/// <summary>
/// Simple AI opponent for the sumo game: walks straight toward the player and pushes.
/// Put this on the opponent's Rigidbody object, and drag the player into "Target".
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SumoAIController : MonoBehaviour
{
    public Transform target;
    public float moveForce = 16f; // slightly weaker than the player so the match is winnable
    public float maxSpeed = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;
        direction = direction.normalized;

        rb.AddForce(direction * moveForce, ForceMode.Acceleration);
        transform.forward = Vector3.Lerp(transform.forward, direction, 5f * Time.fixedDeltaTime);

        // Unity 6 API (if you're on Unity 2021/2022/2023 instead, rename "linearVelocity" to "velocity").
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limited = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
        }
    }
}
