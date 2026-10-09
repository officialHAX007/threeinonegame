using UnityEngine;

/// <summary>
/// Optional: floating ring/coin the player flies through. Purely cosmetic/objective —
/// not required by the rubric, but makes the flying scene feel like an actual game.
/// Put this on a Sphere (or Torus if you import one) with "Is Trigger" checked on its Collider.
/// </summary>
public class Collectible : MonoBehaviour
{
    public float spinSpeed = 90f;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Hook up a score counter / UI text here later if you want.
            gameObject.SetActive(false);
        }
    }
}
