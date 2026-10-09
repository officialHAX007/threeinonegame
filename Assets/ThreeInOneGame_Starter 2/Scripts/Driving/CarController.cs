using UnityEngine;

/// <summary>
/// "Mad Driver" - simple arcade car controller.
/// Controls: W/Up = accelerate, S/Down = reverse/brake, A/D or Left/Right = steer.
/// Put this on a Rigidbody-driven car object (a stretched Cube works fine).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Movement")]
    public float motorForce = 1500f;
    public float maxSpeed = 20f;
    public float turnSpeed = 90f; // degrees/sec at full speed

    [Header("Stability")]
    public float groundDrag = 2f;

    private Rigidbody rb;
    private float moveInput;
    private float steerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f); // stops the car from flipping over

        // Unity 6 API (if you're on Unity 2021/2022/2023 instead, rename this to rb.drag).
        rb.linearDamping = groundDrag;
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        // Unity 6 API (if you're on Unity 2021/2022/2023 instead, rename this to rb.velocity).
        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        if (Mathf.Abs(currentSpeed) < maxSpeed)
        {
            rb.AddForce(transform.forward * moveInput * motorForce);
        }

        if (Mathf.Abs(currentSpeed) > 0.5f)
        {
            float speedFactor = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);
            float turn = steerInput * turnSpeed * speedFactor * Time.fixedDeltaTime * Mathf.Sign(currentSpeed);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
        }
    }
}
