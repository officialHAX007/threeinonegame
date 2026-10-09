using UnityEngine;

/// <summary>
/// "Fly Like a Bird" - simple flight controller.
/// Controls: W/S = pitch up/down, A/D = roll + turn, Left Shift = boost.
/// Put this on the flying object (a Capsule rotated to face forward works fine).
/// </summary>
public class FlightController : MonoBehaviour
{
    [Header("Flight Settings")]
    public float forwardSpeed = 12f;
    public float boostSpeed = 22f;
    public float pitchSpeed = 60f;   // degrees/sec, nose up/down
    public float rollSpeed = 90f;    // degrees/sec, banking left/right
    public float yawFromRoll = 30f;  // degrees/sec, turning follows the bank like a real bird/plane

    void Update()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? boostSpeed : forwardSpeed;

        float pitch = -Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime;
        float roll = -Input.GetAxis("Horizontal") * rollSpeed * Time.deltaTime;
        float yaw = Input.GetAxis("Horizontal") * yawFromRoll * Time.deltaTime;

        transform.Rotate(pitch, yaw, roll, Space.Self);
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
}
