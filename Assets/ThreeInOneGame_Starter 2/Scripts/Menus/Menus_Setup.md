# Main Menu + In-Game (Pause) Menu — Unity Editor Setup

This covers the two menu screens from the assignment sheet. Build these after all three games
(Driving, Flying, Sumo) are already working on their own.

---

## Part A — Main Menu scene

1. Create a new scene: `File > New Scene` → Basic (Built-in) → save as `MainMenu` in `Assets/Scenes/`.
2. `GameObject > UI > Canvas` (this also auto-creates an `EventSystem` — you only need one per scene, so don't add a second).
3. Under the Canvas, add a **Panel** (optional background) and these UI elements
   (`GameObject > UI > Text - TextMeshPro` for text, `GameObject > UI > Button - TextMeshPro` for buttons):

   - `TitleText` — "YOUR COOL GAME TITLE" (or your own title), large font, centered near the top.
   - `BtnDriving` — button labeled **Mad Driver**
   - `BtnFlying` — button labeled **Fly Like a Bird**
   - `BtnSumo` — button labeled **I'm a Sumo and a Ball**
   - `BtnExit` — button labeled **Exit**
   - `CreditText` — small text in the bottom-right corner: "By \<your_name\>"

   Stack the 4 buttons vertically in the center of the screen (use a Vertical Layout Group on a
   container, or just position them manually) to match the reference image.

4. Create an empty GameObject, rename it `MainMenuController`, and add the `MainMenuController`
   script (from this folder) to it.
5. Wire each button's **OnClick()** (in the Button's Inspector):
   - `BtnDriving` → drag `MainMenuController` object into the object slot → pick
     `MainMenuController.PlayDriving`
   - `BtnFlying` → `MainMenuController.PlayFlying`
   - `BtnSumo` → `MainMenuController.PlaySumo`
   - `BtnExit` → `MainMenuController.ExitGame`
6. Double-check the `MainMenuController` component's scene-name fields exactly match your actual
   scene file names (`Driving`, `Flying`, `Sumo` by default).

---

## Part B — In-Game (Pause) Menu — build once, reuse in all 3 games

Build this inside the **Driving** scene first, then turn it into a Prefab so Flying and Sumo can
reuse the exact same thing without rebuilding it three times.

1. Open the `Driving` scene.
2. `GameObject > UI > Canvas` → rename it `PauseCanvas` (this scene already has its own
   EventSystem now too — that's fine, each scene has its own, they don't conflict).
3. Under `PauseCanvas`, add a **Panel** named `PausePanel` that covers the screen (semi-transparent
   dark background works well). This is the object you'll toggle on/off.
4. Under `PausePanel`, add:
   - `PausedText` — "PAUSED", large, centered near the top
   - `BtnResume` — button labeled **Resume**
   - `BtnRestart` — button labeled **Restart**
   - `BtnMainMenu` — button labeled **Back to Main Menu**

   Stack the 3 buttons vertically in the center, matching the reference image.

5. Select `PauseCanvas`, add the `PauseMenuController` script (from this folder).
6. Drag `PausePanel` into the script's `Pause Panel` field.
7. Confirm `Main Menu Scene Name` is set to `MainMenu`.
8. Wire the 3 buttons' **OnClick()**:
   - `BtnResume` → `PauseMenuController.Resume`
   - `BtnRestart` → `PauseMenuController.Restart`
   - `BtnMainMenu` → `PauseMenuController.BackToMainMenu`
9. In the Hierarchy, select `PausePanel` and uncheck the checkbox next to its name in the
   Inspector (so it starts hidden) — `PauseMenuController.Start()` also force-hides it at
   runtime, so this is just a tidy default while editing.
10. Press Play, confirm: Escape pauses/shows the panel, Escape again (or Resume) un-pauses,
    Restart reloads the Driving scene, Back to Main Menu loads the MainMenu scene.

### Turn it into a reusable Prefab
1. Drag the `PauseCanvas` object from the Hierarchy into a `Assets/Prefabs/` folder (create the
   folder if needed). This creates a Prefab asset and the Hierarchy object becomes a prefab
   instance (blue text).
2. Open the `Flying` scene and the `Sumo` scene, and in each one, drag the same `PauseCanvas`
   prefab from `Assets/Prefabs/` into the Hierarchy. It arrives fully wired — buttons, script,
   and panel reference all already set.
3. If you ever want to tweak the pause menu's look later, edit the Prefab once (double-click it
   in the Project window) and every scene updates automatically.

---

## Part C — Build Settings (required for scene loading to work at all)

1. `File > Build Settings`.
2. Add all 4 scenes, in this order:
   0. `MainMenu`
   1. `Driving`
   2. `Flying`
   3. `Sumo`
3. Index 0 (`MainMenu`) is what loads first when you build/run the game — make sure it's on top
   of the list. Drag to reorder if needed.

---

## Part D — Final checklist against the rubric

Run through each of these once everything is wired, with the pause menu tested in all 3 games:

- [ ] Main Menu shows title + 4 options, matches the reference layout *(5 marks — design)*
- [ ] In-Game Menu shows "PAUSED" + 3 options, matches the reference layout *(5 marks — design)*
- [ ] Main Menu → Mad Driver loads the Driving scene *(5 marks)*
- [ ] Main Menu → Fly Like a Bird loads the Flying scene *(5 marks)*
- [ ] Main Menu → I'm a Sumo and a Ball loads the Sumo scene *(5 marks)*
- [ ] Main Menu → Exit quits the game *(5 marks)*
- [ ] Escape in any of the 3 games pauses it and shows the In-Game Menu *(5 marks)*
- [ ] In-Game Menu → Resume un-pauses and hides the menu *(5 marks)*
- [ ] In-Game Menu → Restart reloads the current game from the start *(5 marks)*
- [ ] In-Game Menu → Back to Main Menu returns to the Main Menu *(5 marks)*

That's all 10 line items / 50 marks. Commit to git after this part works, same as before —
don't wait until the very end to make your first/only commit.
