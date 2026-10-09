using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// In-Game (pause) Menu. Put this on the root of the "PausePanel" Canvas/prefab that lives
/// inside each playable game scene (Driving, Flying, Sumo).
///
/// Escape toggles pause on/off. The 3 buttons (Resume / Restart / Back to Main Menu) call the
/// public methods below via their OnClick() events in the Inspector.
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    [Tooltip("The panel GameObject that holds the Resume/Restart/Back to Main Menu buttons. " +
             "Usually this script's own GameObject, or a direct child of it.")]
    public GameObject pausePanel;

    [Tooltip("Must match the Main Menu scene's name exactly, and it must be in Build Settings.")]
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused;

    void Start()
    {
        // Make sure the game never starts already paused, even if you left the panel
        // active while editing the scene.
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f; // freezes all Time.deltaTime-based movement and physics
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f; // un-pause BEFORE reloading, or the reloaded scene starts frozen
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // un-pause BEFORE leaving, or the Main Menu starts frozen too
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
