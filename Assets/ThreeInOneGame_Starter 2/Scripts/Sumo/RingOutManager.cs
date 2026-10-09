using UnityEngine;

/// <summary>
/// Watches both fighters and declares a winner once one of them falls off the ring.
/// Put this on an empty GameObject in the Sumo scene (e.g. "GameManager").
/// WinText / LoseText are optional UI Text/TMP objects, disabled by default, shown on game end.
/// </summary>
public class RingOutManager : MonoBehaviour
{
    public Transform player;
    public Transform opponent;
    public float fallThresholdY = -2f;
    public GameObject winText;
    public GameObject loseText;

    private bool gameEnded;

    void Update()
    {
        if (gameEnded) return;

        if (opponent != null && opponent.position.y < fallThresholdY)
        {
            EndGame(true);
        }
        else if (player != null && player.position.y < fallThresholdY)
        {
            EndGame(false);
        }
    }

    void EndGame(bool playerWon)
    {
        gameEnded = true;
        if (playerWon && winText != null) winText.SetActive(true);
        if (!playerWon && loseText != null) loseText.SetActive(true);
    }
}
