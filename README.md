# Three-in-One Game Project

**Author:** Min Khant Aung  
**Student ID:** 6632753  
**Repository:** [https://github.com/officialHAX007/threeinonegame.git](https://github.com/officialHAX007/threeinonegame.git)

---

## Project Overview
This project combines three Unity game prototypes into a single unified application with centralized navigation and pause management:
1. **Mad Driver** (Driving Game)
2. **Fly Like a Bird** (Flying Game)
3. **I'm a Sumo and a Ball** (Sumo Game)

---

## Features & Implementation

### 1. Main Menu
* **Title Screen:** Custom UI layout displaying game title and author credits (`By Min Khant Aung`).
* **Game Navigation:** Buttons to launch directly into each sub-game:
  * Select **Mad Driver** → Loads the Driving scene.
  * Select **Fly Like a Bird** → Loads the Flying scene.
  * Select **I'm a Sumo and a Ball** → Loads the Sumo scene.
* **Exit:** Cleanly terminates the application (`Application.Quit()`).

### 2. In-Game Menu (Pause System)
* **Escape Key Trigger:** Pressing the `Escape` key pauses active gameplay (`Time.timeScale = 0`) and overlays the pause canvas.
* **Resume:** Closes the pause overlay and resumes real-time gameplay (`Time.timeScale = 1`).
* **Restart:** Resets the current active scene to its initial state.
* **Back to Main Menu:** Returns the player to the main menu screen.

---

## Controls
| Action | Key / Input |
| :--- | :--- |
| **Pause / Open In-Game Menu** | `Escape` |
| **Menu Navigation** | Mouse Click |
| **Gameplay Controls** | Standard game-specific inputs (WASD / Arrow Keys) |

---

## Project Structure
```text
Assets/
├── Prefabs/
│   └── PausePanel.prefab       # Reusable In-Game Pause UI overlay
├── Scenes/
│   ├── MainMenu.unity          # Main selection menu
│   ├── Driving.unity           # Mad Driver scene
│   ├── Flying.unity            # Fly Like a Bird scene
│   └── Sumo.unity              # Sumo arena scene
└── Scripts/
    ├── Menus/
    │   ├── MainMenuController.cs
    │   └── PauseMenuController.cs
    ├── Driving/
    ├── Flying/
    └── Sumo/
