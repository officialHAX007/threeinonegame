using UnityEngine;

/// <summary>
/// Simple chase camera. Works for both the driving game and the flying game.
/// Drag this onto the Main Camera and assign "Target" to the car/plane transform.
/// </summary>
public class ThirdPersonCameraFollow : MonoBehaviour
{
    [Tooltip("The car or plane this camera follows.")]
    public Transform target;

    [Tooltip("Offset from the target, in the target's local space (so it stays behind it when it turns).")]
    public Vector3 offset = new Vector3(0f, 4f, -8f);

    public float followSpeed = 5f;
    public float lookSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        Vector3 lookPoint = target.position + Vector3.up * 1.5f;
        Quaternion desiredRotation = Quaternion.LookRotation((lookPoint - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, lookSpeed * Time.deltaTime);
    }
}
