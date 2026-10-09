# One-Click Scene Builder — Instructions

This replaces manually following `Menus_Setup.md` + the three `*_Setup.md` guides by hand.
Instead, one script builds all 4 scenes, the UI, the reusable Pause prefab, and the Build
Settings for you automatically, inside your own Unity Editor.

## Why a script instead of a literal .unitypackage file

A `.unitypackage` is a container of pre-serialized scene/prefab data. I don't have a Unity
Editor available on my end to actually generate or test one, so hand-writing that binary-ish
format blind risks a subtle, silent error (a button that does nothing, a scene that won't open) —
exactly the kind of bug that's painful to debug under deadline. This script sidesteps that
entirely: it's normal, compiled C# that runs *inside your real Unity Editor* and calls the same
APIs Unity's own menus use, so what it builds is guaranteed valid — any problem shows up
immediately in your Console instead of silently.

## Setup (one time)

1. Make sure this script sits inside a folder literally named `Editor` somewhere under `Assets`
   (e.g. `Assets/Editor/ThreeInOneGameBuilder.cs`). This is already how it's laid out in this
   pack — when you copy the `Editor` folder into your project's `Assets`, you're done.
2. **Critical check before running it:** go to `Edit > Project Settings > Player`, open
   **Other Settings**, find **Active Input Handling**, and make sure it's set to **Both** or
   **Input Manager (Old)** — NOT "Input System Package (New)" by itself. Your Project panel
   shows an "Input System" package is installed, and if Active Input Handling is locked to only
   that, the classic `Input.GetAxis` / `Input.GetKey` calls every game script uses will throw
   errors at runtime and nothing will respond to key presses. If you change this setting, Unity
   will ask to restart the Editor — let it.

## Running it

1. In Unity's menu bar: **Tools > Three-In-One Game > Build ALL (Menus + 3 Games)**.
2. Confirm the dialog.
3. Watch the Console (bottom of the Editor) for the green "build complete" message, or any red
   errors.
4. Open `Assets/Scenes/MainMenu.unity` (double-click it in the Project window) and press **Play**
   to test the whole flow: click into each game, press Escape to pause, Resume/Restart/Back to
   Main Menu.

## After it builds — personalize it

The script leaves two placeholders for you to edit directly in the Inspector (select the object
in the Hierarchy, no need to touch code):
- `MainMenuCanvas > TitleText` — currently "YOUR COOL GAME TITLE", change to your own title.
- `MainMenuCanvas > CreditText` — currently "By <your_name>", put your actual name in.

## If you want to add more (obstacles, collectibles, nicer visuals)

The script gives you the minimum that satisfies the rubric (navigable scenes + working pause
system) built from plain primitives. If you want extra flair — obstacles on the driving track,
collectible rings in the sky, a fancier look for the sumo ring — just open the relevant scene in
the Editor afterward and add to it normally like any other Unity scene; nothing about this
process locks you out of hand-editing further. The original `Driving_Setup.md` / `Flying_Setup.md`
/ `Sumo_Setup.md` guides describe optional extras like this if you want ideas.

## Re-running

Re-running **Build ALL** fully overwrites the 4 scenes and the Pause prefab back to this clean
baseline — use it to reset if something gets broken while you're experimenting, but note it will
discard any manual changes you made inside those specific scenes since the last automated build.
The individual `Build Main Menu Only` / `Build Driving Only` / etc. menu items let you rebuild
just one piece at a time.
