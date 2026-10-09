# Three-in-One Game — Starter Pack (the 3 sub-games)

This pack gives you simple, working versions of the three required games:
**Mad Driver** (driving), **Fly Like a Bird** (flying), and **I'm a Sumo and a Ball** (sumo).
They're deliberately simple — built from primitive shapes (cubes, capsules, cylinders) — because
the assignment's grading rubric awards marks for the **menu system**, not for game polish. Build
these first; we'll wire the Main Menu and In-Game (pause) Menu around them together next.

## 1. Create the project
1. Open Unity Hub → **New Project** → pick the **Universal 3D** template (URP). Avoid "3D
   (Built-in Render Pipeline)" on Unity 6 — it's shown with a warning icon since Built-in is
   legacy there. None of these games need anything URP-specific, so Universal 3D is the safe
   default.
2. Once the empty project opens, create this folder structure inside `Assets`:
   ```
   Assets/
     Scripts/
       Shared/
       Driving/
       Flying/
       Sumo/
     Scenes/
   ```
3. Copy the `.cs` files from this pack's `Scripts/` folder into the matching folders in your
   project (`Scripts/Shared/ThirdPersonCameraFollow.cs` → your `Assets/Scripts/Shared/`, etc.).
   Unity will compile them automatically once they're inside the project.

## 2. Build each scene
Follow the setup guide in each game's folder, in this order:
1. `Scripts/Driving/Driving_Setup.md` → create scene `Driving`
2. `Scripts/Flying/Flying_Setup.md` → create scene `Flying`
3. `Scripts/Sumo/Sumo_Setup.md` → create scene `Sumo`

Save each scene into `Assets/Scenes/` with the exact name shown (these names matter — the Main
Menu buttons we'll build next will load scenes by these names).

## 3. A note on Unity versions (read this if you get compile errors)
These scripts are written for **Unity 6** (`rb.linearVelocity`, `rb.linearDamping`). If you end
up on an older Unity (2021/2022/2023) instead, Unity renamed these from `rb.velocity` and
`rb.drag` in Unity 6 — rename them back if you see a compile error. The affected lines are
commented in `CarController.cs`, `SumoPlayerController.cs`, and `SumoAIController.cs`.

## 4. Test each game standalone
Open each scene and press Play to confirm it works on its own before we connect them to menus:
- **Driving**: W/S to accelerate/reverse, A/D to steer.
- **Flying**: W/S to pitch, A/D to bank and turn, Shift to boost.
- **Sumo**: WASD to move and push the AI opponent off the ring.

## 5. Git — start now, not at the end
Since the rubric penalizes any post-deadline change to the repo (-50) and wants to see commit/push
in your video, initialize git as soon as the project exists and commit after each scene:
```
git init
git add .
git commit -m "Add driving game"
...
git commit -m "Add flying game"
...
git commit -m "Add sumo game"
```
Push to GitHub (or GitLab) once you've created the remote repo:
```
git remote add origin <your-repo-url>
git push -u origin main
```

## 6. What's next
Once all three scenes are playable, come back to the chat and say so — we'll build:
- A `GameStateManager` that persists across scenes and handles the Escape-key pause
- The **Main Menu** scene (title + 4 buttons: Mad Driver / Fly Like a Bird / I'm a Sumo and a
  Ball / Exit)
- The **In-Game Menu** (Paused overlay: Resume / Restart / Back to Main Menu)

Because all three scripts here move things with `Time.deltaTime` (or physics forces each
`FixedUpdate`), the global pause (`Time.timeScale = 0`) will freeze all three games correctly
without any extra work — that was a deliberate design choice so the integration step is painless.
