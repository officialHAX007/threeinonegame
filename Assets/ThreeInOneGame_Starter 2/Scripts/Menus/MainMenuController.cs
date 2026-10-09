using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the Main Menu's 4 buttons: Mad Driver, Fly Like a Bird, I'm a Sumo and a Ball, Exit.
/// Put this on an empty GameObject in the MainMenu scene (e.g. "MainMenuController"),
/// then wire each UI Button's OnClick() to one of the public methods below.
///
/// IMPORTANT: the scene names below must exactly match your scene file names
/// (and all four scenes must be added in File > Build Settings).
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Scene names (must match Build Settings exactly)")]
    public string drivingSceneName = "Driving";
    public string flyingSceneName = "Flying";
    public string sumoSceneName = "Sumo";

    public void PlayDriving()
    {
        SceneManager.LoadScene(drivingSceneName);
    }

    public void PlayFlying()
    {
        SceneManager.LoadScene(flyingSceneName);
    }

    public void PlaySumo()
    {
        SceneManager.LoadScene(sumoSceneName);
    }

    public void ExitGame()
    {
        Debug.Log("Exit pressed");
#if UNITY_EDITOR
        // Application.Quit() does nothing in the Editor, so stop Play Mode instead
        // when you're testing by pressing Play in Unity.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
