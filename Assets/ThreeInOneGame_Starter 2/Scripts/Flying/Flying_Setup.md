# Flying Game ("Fly Like a Bird") — Unity Editor Setup

Scene name: `Flying`

## 1. Sky / space
Flying games don't need a ground — just open space. Optionally:
1. `Window > Rendering > Lighting` → set a Skybox material for atmosphere (defaults are fine).
2. Or just leave the default skybox.

## 2. The flyer
1. Right-click in Hierarchy → `3D Object > Capsule`. Rename it `Flyer`.
2. Rotate it 90° on X (`Rotation: 90, 0, 0`) so its length points forward (the capsule's long axis becomes "nose-to-tail").
3. Position it at `0, 5, 0` so it starts up in the air.
4. Add Component → `FlightController` (the script in this folder).
5. Tag it `Player` (Inspector → top-left `Tag` dropdown → `Player`). This is needed if you add Collectibles.

> No Rigidbody needed — FlightController moves the transform directly, which is simpler for this kind of controller.

## 3. Obstacles / rings (optional)
1. Add a few `3D Object > Sphere`s scattered in the air as collectible rings/coins.
2. On each, check **Is Trigger** on the Sphere Collider.
3. Add Component → `Collectible` (the script in this folder).

## 4. Camera
1. Select `Main Camera`.
2. Add Component → `ThirdPersonCameraFollow` (from `Scripts/Shared`).
3. Drag `Flyer` into the `Target` field.
4. Suggested offset for a flying chase cam: `0, 2, -6`.

## 5. Controls recap
- `W` / `S` — pitch nose up / down
- `A` / `D` — bank + turn left / right
- `Left Shift` — boost speed

## 6. Add to Build Settings
`File > Build Settings > Add Open Scenes` once this scene is finished.
