# Sumo Game ("I'm a Sumo and a Ball") — Unity Editor Setup

Scene name: `Sumo`

## 1. The ring
1. Right-click in Hierarchy → `3D Object > Cylinder`. Rename it `Ring`.
2. Scale it flat and wide, e.g. `Scale: 8, 0.2, 8`.
3. Position at origin `0, 0, 0`. This is an open platform — fighters fall off when pushed past the edge; no walls needed.

## 2. Player
1. Right-click in Hierarchy → `3D Object > Capsule`. Rename it `Player`.
2. Position it on top of the ring, e.g. `-2, 1.1, 0`.
3. Add Component → `Rigidbody`.
   - In the Rigidbody's **Constraints**, check **Freeze Rotation X, Y, Z** (stops it from tipping over or spinning wildly when bumped — the script still turns it by directly rotating the transform, so this is safe).
4. Add Component → `SumoPlayerController` (from this folder).
5. Set Tag to `Player` (Inspector → `Tag` dropdown).

## 3. Opponent (AI)
1. Right-click in Hierarchy → `3D Object > Capsule`. Rename it `Opponent`.
2. Position it on the other side of the ring, e.g. `2, 1.1, 0`.
3. Add Component → `Rigidbody` with the same **Freeze Rotation X, Y, Z** constraints as the player.
4. Add Component → `SumoAIController` (from this folder).
5. Drag `Player` into the script's `Target` field.
6. (Optional) give it a different color material so it's visually distinct from the player.

## 4. Game Manager (win/lose detection)
1. Create an empty GameObject: right-click Hierarchy → `Create Empty`. Rename it `GameManager`.
2. Add Component → `RingOutManager` (from this folder).
3. Drag `Player` into the `Player` field, `Opponent` into the `Opponent` field.
4. (Optional) Create two UI Text objects ("You Win!" / "You Lose") via `GameObject > UI > Text - TextMeshPro`, disable them by default (uncheck the GameObject's checkbox in the Inspector), and drag them into `Win Text` / `Lose Text`.

## 5. Camera
A simple fixed overhead/angled camera works well for sumo (no need for the chase-cam script here).
1. Select `Main Camera`.
2. Position it above and to the side of the ring, e.g. `0, 10, -8`, rotated to look down at the ring (e.g. `Rotation: 45, 0, 0`).

## 6. Controls recap
- `WASD` / Arrow keys — move the player around the ring and push the opponent

## 7. Add to Build Settings
`File > Build Settings > Add Open Scenes` once this scene is finished.
