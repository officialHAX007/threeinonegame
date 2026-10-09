#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Events;

/// <summary>
/// One-click scaffolder for the Three-in-One Game assignment.
///
/// This does NOT read a pre-made file — it runs inside YOUR Unity Editor and uses Unity's own
/// APIs to build the 4 scenes (MainMenu, Driving, Flying, Sumo), all their UI (styled, not flat
/// placeholder boxes), the reusable Pause prefab, and wires every button, then adds everything to
/// Build Settings.
///
/// *** READ THIS BEFORE RUNNING IT ***
/// Edit > Project Settings > Player > Other Settings > Active Input Handling must be "Both" or
/// "Input Manager (Old)" — NOT "Input System Package (New)" only. If it's left on "Input System
/// Package (New)", Unity's own UI click-handling (EventSystem) cannot read input at all, so NO
/// button anywhere will respond to clicks — this is not a bug in the generated UI, it's a
/// project-wide setting. Unity will ask to restart the Editor after you change it — let it, then
/// re-run this builder.
///
/// WHY A SCRIPT INSTEAD OF A .unitypackage FILE:
/// A .unitypackage is really a container of pre-serialized scene/prefab files. Hand-authoring
/// those by text outside of Unity (with no Editor available to actually run and verify them)
/// risks subtle, hard-to-spot breakage. This script instead runs as real, compiled code inside
/// your own Editor, using the exact same APIs Unity's own menus use — so what it builds is
/// guaranteed to be valid, and you'll see any error immediately in the Console.
///
/// HOW TO USE:
/// 1. Keep this file inside a folder literally named "Editor" under Assets.
/// 2. Fix Active Input Handling (see above) — do this first.
/// 3. Unity menu bar: Tools > Three-In-One Game > Build ALL (Menus + 3 Games). Confirm the dialog.
/// 4. Open Assets/Scenes/MainMenu.unity and press Play to test.
///
/// Re-running "Build ALL" overwrites the 4 scenes and the Pause prefab back to this baseline.
/// </summary>
public static class ThreeInOneGameBuilder
{
    private const string ScenesFolder = "Assets/Scenes";
    private const string PrefabsFolder = "Assets/Prefabs";
    private const string GeneratedFolder = "Assets/Generated";
    private const string PausePrefabPath = PrefabsFolder + "/PausePanel.prefab";
    private const string ButtonSpritePath = GeneratedFolder + "/RoundedRect.png";

    private const string MainMenuScenePath = ScenesFolder + "/MainMenu.unity";
    private const string DrivingScenePath = ScenesFolder + "/Driving.unity";
    private const string FlyingScenePath = ScenesFolder + "/Flying.unity";
    private const string SumoScenePath = ScenesFolder + "/Sumo.unity";

    // ---- Color palette (a cohesive dark theme with per-action accent colors) ----
    private static readonly Color BgColor = new Color32(14, 16, 22, 255);
    private static readonly Color CardColor = new Color32(26, 29, 39, 235);
    private static readonly Color AccentColor = new Color32(138, 122, 255, 255);   // purple
    private static readonly Color DrivingColor = new Color32(45, 143, 230, 255);   // blue
    private static readonly Color FlyingColor = new Color32(138, 122, 255, 255);   // purple
    private static readonly Color SumoColor = new Color32(235, 126, 94, 255);      // orange
    private static readonly Color ExitColor = new Color32(222, 68, 69, 255);       // red
    private static readonly Color ResumeColor = new Color32(39, 196, 158, 255);    // green
    private static readonly Color RestartColor = new Color32(230, 149, 61, 255);   // amber
    private static readonly Color BackColor = new Color32(108, 117, 130, 255);     // slate gray

    private static Sprite _roundedSprite;

    [MenuItem("Tools/Three-In-One Game/Build ALL (Menus + 3 Games)")]
    public static void BuildAll()
    {
        if (!EditorUtility.DisplayDialog(
                "Build Three-In-One Game",
                "This creates/overwrites:\n\n" +
                "- Assets/Scenes/MainMenu.unity\n" +
                "- Assets/Scenes/Driving.unity\n" +
                "- Assets/Scenes/Flying.unity\n" +
                "- Assets/Scenes/Sumo.unity\n" +
                "- Assets/Prefabs/PausePanel.prefab\n\n" +
                "and adds all 4 scenes to Build Settings.\n\n" +
                "Reminder: Active Input Handling (Project Settings > Player) must be \"Both\" or " +
                "\"Input Manager (Old)\", or buttons won't respond no matter what.\n\nContinue?",
                "Build", "Cancel"))
        {
            return;
        }

        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

        EnsureFolder("Assets", "Scenes");
        EnsureFolder("Assets", "Prefabs");
        EnsureFolder("Assets", "Generated");

        try
        {
            BuildMainMenuScene();
            GameObject pausePrefab = BuildDrivingSceneAndPausePrefab();
            BuildFlyingScene(pausePrefab);
            BuildSumoScene(pausePrefab);
            SetBuildSettings();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("<color=#4CAF50><b>Three-In-One Game: build complete.</b></color> " +
                      "Open Assets/Scenes/MainMenu.unity and press Play to test. " +
                      "Remember to personalize the title text and \"By <your_name>\" credit.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Three-In-One Game build failed: " + e);
            throw;
        }
    }

    // ---------------------------------------------------------------
    // MAIN MENU
    // ---------------------------------------------------------------
    [MenuItem("Tools/Three-In-One Game/Build Main Menu Only")]
    public static void BuildMainMenuScene()
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        GameObject canvasGO = CreateCanvas("MainMenuCanvas");
        Transform canvasT = canvasGO.transform;

        CreateFullscreenPanel(canvasT, BgColor);

        CreateCard(canvasT, new Vector2(0, -40), new Vector2(460, 420), CardColor);

        Text title = CreateUIText(canvasT, "TitleText", "YOUR COOL GAME TITLE", 46,
            new Vector2(0, 330), new Vector2(900, 90), TextAnchor.MiddleCenter, Color.white);
        title.fontStyle = FontStyle.Bold;
        AddShadow(title.gameObject, new Color(0f, 0f, 0f, 0.5f));

        CreateUIText(canvasT, "SubtitleText", "CHOOSE YOUR GAME", 16,
            new Vector2(0, 280), new Vector2(600, 36), TextAnchor.MiddleCenter, new Color(0.65f, 0.67f, 0.78f, 1f));

        CreateCard(canvasT, new Vector2(0, 256), new Vector2(90, 4), AccentColor);

        GameObject controllerGO = new GameObject("MainMenuController");
        MainMenuController controller = controllerGO.AddComponent<MainMenuController>();

        Button btnDriving = CreateUIButton(canvasT, "BtnDriving", "Mad Driver", new Vector2(0, 110), new Vector2(360, 62), DrivingColor);
        Button btnFlying = CreateUIButton(canvasT, "BtnFlying", "Fly Like a Bird", new Vector2(0, 30), new Vector2(360, 62), FlyingColor);
        Button btnSumo = CreateUIButton(canvasT, "BtnSumo", "I'm a Sumo and a Ball", new Vector2(0, -50), new Vector2(360, 62), SumoColor);
        Button btnExit = CreateUIButton(canvasT, "BtnExit", "Exit", new Vector2(0, -130), new Vector2(360, 62), ExitColor);

        UnityEventTools.AddPersistentListener(btnDriving.onClick, controller.PlayDriving);
        UnityEventTools.AddPersistentListener(btnFlying.onClick, controller.PlayFlying);
        UnityEventTools.AddPersistentListener(btnSumo.onClick, controller.PlaySumo);
        UnityEventTools.AddPersistentListener(btnExit.onClick, controller.ExitGame);

        Text credit = CreateUIText(canvasT, "CreditText", "By <your_name>", 16,
            new Vector2(-20, 20), new Vector2(300, 30), TextAnchor.LowerRight, new Color(1f, 1f, 1f, 0.55f),
            anchorMin: new Vector2(1, 0), anchorMax: new Vector2(1, 0), pivot: new Vector2(1, 0));
        credit.fontStyle = FontStyle.Italic;

        EditorSceneManager.SaveScene(scene, MainMenuScenePath);
        Debug.Log("Built " + MainMenuScenePath);
    }

    // ---------------------------------------------------------------
    // DRIVING  (also builds the shared Pause prefab, once)
    // ---------------------------------------------------------------
    [MenuItem("Tools/Three-In-One Game/Build Driving Only (also builds Pause prefab)")]
    public static void BuildDrivingOnly()
    {
        BuildDrivingSceneAndPausePrefab();
    }

    private static GameObject BuildDrivingSceneAndPausePrefab()
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(10f, 1f, 10f);

        GameObject car = GameObject.CreatePrimitive(PrimitiveType.Cube);
        car.name = "Car";
        car.transform.localScale = new Vector3(1f, 0.5f, 2f);
        car.transform.position = new Vector3(0f, 0.5f, 0f);
        car.AddComponent<Rigidbody>();
        car.AddComponent<CarController>();

        SetUpChaseCamera(car.transform, new Vector3(0f, 4f, -8f));

        GameObject pausePrefab = BuildPausePanelPrefab();

        EditorSceneManager.SaveScene(scene, DrivingScenePath);
        Debug.Log("Built " + DrivingScenePath + " and " + PausePrefabPath);
        return pausePrefab;
    }

    // ---------------------------------------------------------------
    // FLYING
    // ---------------------------------------------------------------
    [MenuItem("Tools/Three-In-One Game/Build Flying Only")]
    public static void BuildFlyingScene()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PausePrefabPath);
        if (prefab == null)
        {
            Debug.LogError("PausePanel.prefab not found. Run \"Build Driving Only\" (or Build ALL) first.");
            return;
        }
        BuildFlyingScene(prefab);
    }

    private static void BuildFlyingScene(GameObject pausePrefab)
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        GameObject flyer = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        flyer.name = "Flyer";
        flyer.transform.position = new Vector3(0f, 5f, 0f);
        flyer.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        flyer.tag = "Player";
        flyer.AddComponent<FlightController>();

        SetUpChaseCamera(flyer.transform, new Vector3(0f, 2f, -6f));

        InstantiatePausePanel(pausePrefab);

        EditorSceneManager.SaveScene(scene, FlyingScenePath);
        Debug.Log("Built " + FlyingScenePath);
    }

    // ---------------------------------------------------------------
    // SUMO
    // ---------------------------------------------------------------
    [MenuItem("Tools/Three-In-One Game/Build Sumo Only")]
    public static void BuildSumoScene()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PausePrefabPath);
        if (prefab == null)
        {
            Debug.LogError("PausePanel.prefab not found. Run \"Build Driving Only\" (or Build ALL) first.");
            return;
        }
        BuildSumoScene(prefab);
    }

    private static void BuildSumoScene(GameObject pausePrefab)
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        GameObject ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring.name = "Ring";
        ring.transform.localScale = new Vector3(8f, 0.2f, 8f);

        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(-2f, 1.1f, 0f);
        player.tag = "Player";
        Rigidbody playerRb = player.AddComponent<Rigidbody>();
        playerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        player.AddComponent<SumoPlayerController>();

        GameObject opponent = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        opponent.name = "Opponent";
        opponent.transform.position = new Vector3(2f, 1.1f, 0f);
        Rigidbody oppRb = opponent.AddComponent<Rigidbody>();
        oppRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        SumoAIController ai = opponent.AddComponent<SumoAIController>();
        ai.target = player.transform;

        Renderer oppRenderer = opponent.GetComponent<Renderer>();
        if (oppRenderer != null)
        {
            Material mat = new Material(oppRenderer.sharedMaterial);
            mat.color = SumoColor;
            oppRenderer.sharedMaterial = mat;
        }

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.transform.position = new Vector3(0f, 10f, -8f);
            mainCam.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        }

        GameObject gameManagerGO = new GameObject("GameManager");
        RingOutManager ringOut = gameManagerGO.AddComponent<RingOutManager>();
        ringOut.player = player.transform;
        ringOut.opponent = opponent.transform;

        GameObject resultCanvasGO = CreateCanvas("ResultCanvas");
        Text winText = CreateUIText(resultCanvasGO.transform, "WinText", "YOU WIN!", 60,
            Vector2.zero, new Vector2(600, 150), TextAnchor.MiddleCenter, ResumeColor);
        winText.fontStyle = FontStyle.Bold;
        AddShadow(winText.gameObject, new Color(0f, 0f, 0f, 0.5f));
        winText.gameObject.SetActive(false);

        Text loseText = CreateUIText(resultCanvasGO.transform, "LoseText", "YOU LOSE", 60,
            Vector2.zero, new Vector2(600, 150), TextAnchor.MiddleCenter, ExitColor);
        loseText.fontStyle = FontStyle.Bold;
        AddShadow(loseText.gameObject, new Color(0f, 0f, 0f, 0.5f));
        loseText.gameObject.SetActive(false);

        ringOut.winText = winText.gameObject;
        ringOut.loseText = loseText.gameObject;

        InstantiatePausePanel(pausePrefab);

        EditorSceneManager.SaveScene(scene, SumoScenePath);
        Debug.Log("Built " + SumoScenePath);
    }

    // ---------------------------------------------------------------
    // SHARED HELPERS
    // ---------------------------------------------------------------

    private static void SetUpChaseCamera(Transform target, Vector3 offset)
    {
        Camera mainCam = Camera.main;
        if (mainCam == null) return;
        ThirdPersonCameraFollow follow = mainCam.gameObject.AddComponent<ThirdPersonCameraFollow>();
        follow.target = target;
        follow.offset = offset;
    }

    private static GameObject BuildPausePanelPrefab()
    {
        GameObject canvasGO = CreateCanvas("PauseCanvas");

        GameObject panel = CreateFullscreenPanel(canvasGO.transform, new Color(0f, 0f, 0f, 0.7f));
        panel.name = "PausePanel";

        CreateCard(panel.transform, new Vector2(0, 0), new Vector2(400, 360), CardColor);

        Text pausedTitle = CreateUIText(panel.transform, "PausedText", "PAUSED", 44,
            new Vector2(0, 140), new Vector2(500, 70), TextAnchor.MiddleCenter, Color.white);
        pausedTitle.fontStyle = FontStyle.Bold;
        AddShadow(pausedTitle.gameObject, new Color(0f, 0f, 0f, 0.5f));

        CreateCard(panel.transform, new Vector2(0, 98), new Vector2(70, 4), AccentColor);

        Button btnResume = CreateUIButton(panel.transform, "BtnResume", "Resume", new Vector2(0, 40), new Vector2(300, 58), ResumeColor);
        Button btnRestart = CreateUIButton(panel.transform, "BtnRestart", "Restart", new Vector2(0, -30), new Vector2(300, 58), RestartColor);
        Button btnMainMenu = CreateUIButton(panel.transform, "BtnMainMenu", "Back to Main Menu", new Vector2(0, -100), new Vector2(300, 58), BackColor);

        PauseMenuController pauseController = canvasGO.AddComponent<PauseMenuController>();
        pauseController.pausePanel = panel;
        pauseController.mainMenuSceneName = "MainMenu";

        UnityEventTools.AddPersistentListener(btnResume.onClick, pauseController.Resume);
        UnityEventTools.AddPersistentListener(btnRestart.onClick, pauseController.Restart);
        UnityEventTools.AddPersistentListener(btnMainMenu.onClick, pauseController.BackToMainMenu);

        panel.SetActive(false);

        GameObject prefabAsset = PrefabUtility.SaveAsPrefabAssetAndConnect(canvasGO, PausePrefabPath, InteractionMode.AutomatedAction);
        return prefabAsset;
    }

    private static void InstantiatePausePanel(GameObject pausePrefab)
    {
        PrefabUtility.InstantiatePrefab(pausePrefab);
    }

    private static GameObject CreateCanvas(string name)
    {
        GameObject canvasGO = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        EnsureEventSystem();
        return canvasGO;
    }

    private static void EnsureEventSystem()
    {
        if (Object.FindAnyObjectByType<EventSystem>() != null) return;
        new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    private static GameObject CreateFullscreenPanel(Transform parent, Color color)
    {
        GameObject go = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        go.GetComponent<Image>().color = color;
        return go;
    }

    private static GameObject CreateCard(Transform parent, Vector2 anchoredPos, Vector2 sizeDelta, Color color)
    {
        GameObject go = new GameObject("Card", typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;
        Image img = go.GetComponent<Image>();
        img.sprite = GetRoundedSprite();
        img.type = Image.Type.Sliced;
        img.color = color;
        return go;
    }

    private static Text CreateUIText(Transform parent, string name, string text, int fontSize,
        Vector2 anchoredPos, Vector2 sizeDelta, TextAnchor alignment, Color color,
        Vector2? anchorMin = null, Vector2? anchorMax = null, Vector2? pivot = null)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin ?? new Vector2(0.5f, 0.5f);
        rt.anchorMax = anchorMax ?? new Vector2(0.5f, 0.5f);
        rt.pivot = pivot ?? new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;

        Text t = go.GetComponent<Text>();
        t.text = text;
        t.font = GetLegacyFont();
        t.fontSize = fontSize;
        t.alignment = alignment;
        t.color = color;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        return t;
    }

    private static Button CreateUIButton(Transform parent, string name, string label, Vector2 anchoredPos, Vector2 sizeDelta, Color baseColor)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;

        Image img = go.GetComponent<Image>();
        img.sprite = GetRoundedSprite();
        img.type = Image.Type.Sliced;
        img.color = Color.white; // Button's ColorTint multiplies this, so the real color lives in ColorBlock below

        Button btn = go.GetComponent<Button>();
        ColorBlock colors = btn.colors;
        colors.normalColor = baseColor;
        colors.highlightedColor = Lighten(baseColor, 0.18f);
        colors.pressedColor = Darken(baseColor, 0.18f);
        colors.selectedColor = baseColor;
        colors.disabledColor = Darken(baseColor, 0.5f);
        colors.fadeDuration = 0.08f;
        btn.colors = colors;

        Text label_ = CreateUIText(go.transform, "Label", label, 24, Vector2.zero,
            new Vector2(sizeDelta.x - 16f, sizeDelta.y), TextAnchor.MiddleCenter, Color.white);
        label_.fontStyle = FontStyle.Bold;
        AddShadow(label_.gameObject, new Color(0f, 0f, 0f, 0.35f));

        return btn;
    }

    private static Color Lighten(Color c, float t) => Color.Lerp(c, Color.white, t);
    private static Color Darken(Color c, float t) => Color.Lerp(c, Color.black, t);

    private static void AddShadow(GameObject go, Color shadowColor)
    {
        Shadow sh = go.AddComponent<Shadow>();
        sh.effectColor = shadowColor;
        sh.effectDistance = new Vector2(1.5f, -1.5f);
    }

    private static Font GetLegacyFont()
    {
        Font f = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (f == null) f = Resources.GetBuiltinResource<Font>("Arial.ttf");
        return f;
    }

    /// <summary>
    /// Generates (once) a small rounded-rectangle sprite, saved as a real PNG asset so scene
    /// references to it survive saving/reopening. Used as a 9-sliced sprite for every button and
    /// card panel, which is what gives them rounded corners instead of flat boxes — all without
    /// needing any external art asset.
    /// </summary>
    private static Sprite GetRoundedSprite()
    {
        if (_roundedSprite != null) return _roundedSprite;

        Sprite existing = AssetDatabase.LoadAssetAtPath<Sprite>(ButtonSpritePath);
        if (existing != null)
        {
            _roundedSprite = existing;
            return _roundedSprite;
        }

        EnsureFolder("Assets", "Generated");

        const int size = 128;
        const int radius = 36;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color32[] pixels = new Color32[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                bool inside = IsInsideRoundedRect(x, y, size, size, radius);
                pixels[y * size + x] = inside ? new Color32(255, 255, 255, 255) : new Color32(255, 255, 255, 0);
            }
        }
        tex.SetPixels32(pixels);
        tex.Apply();

        byte[] png = tex.EncodeToPNG();
        File.WriteAllBytes(ButtonSpritePath, png);
        Object.DestroyImmediate(tex);

        AssetDatabase.ImportAsset(ButtonSpritePath);
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(ButtonSpritePath);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spriteBorder = new Vector4(radius, radius, radius, radius);
        importer.filterMode = FilterMode.Bilinear;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();

        _roundedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(ButtonSpritePath);
        return _roundedSprite;
    }

    private static bool IsInsideRoundedRect(int x, int y, int w, int h, int r)
    {
        int left = r, right = w - r - 1, bottom = r, top = h - r - 1;
        float cx = Mathf.Clamp(x, left, right);
        float cy = Mathf.Clamp(y, bottom, top);
        float dx = x - cx;
        float dy = y - cy;
        return (dx * dx + dy * dy) <= r * r;
    }

    private static void SetBuildSettings()
    {
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(MainMenuScenePath, true),
            new EditorBuildSettingsScene(DrivingScenePath, true),
            new EditorBuildSettingsScene(FlyingScenePath, true),
            new EditorBuildSettingsScene(SumoScenePath, true),
        };
    }

    private static void EnsureFolder(string parent, string folderName)
    {
        string full = parent + "/" + folderName;
        if (!AssetDatabase.IsValidFolder(full))
        {
            AssetDatabase.CreateFolder(parent, folderName);
        }
    }
}
#endif
