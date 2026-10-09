# Driving Game ("Mad Driver") — Unity Editor Setup

Scene name: `Driving`

## 1. Ground
1. Right-click in Hierarchy → `3D Object > Plane`. Rename it `Ground`.
2. Scale it up (e.g. Scale `10, 1, 10`) so there's room to drive.
3. (Optional) Give it a material/color via a new Material asset dragged onto it.

## 2. Car
1. Right-click in Hierarchy → `3D Object > Cube`. Rename it `Car`.
2. Scale it to look car-like, e.g. `1, 0.5, 2`.
3. Position it above the ground, e.g. `0, 0.5, 0`.
4. Add Component → `Rigidbody`.
5. Add Component → `CarController` (the script in this folder).
6. Leave the default values (`Motor Force 1500`, `Max Speed 20`, `Turn Speed 90`) — tweak later if you want it faster/slower.

## 3. Obstacles (optional, makes it feel like a real level)
1. Add a few `3D Object > Cube`s scattered on the Ground as obstacles/cones.
2. Give each a `Box Collider` (added automatically with the Cube) — no Rigidbody needed on these, they should stay static.

## 4. Camera
1. Select `Main Camera` in the Hierarchy.
2. Add Component → `ThirdPersonCameraFollow` (from `Scripts/Shared`).
3. Drag the `Car` object into the script's `Target` field in the Inspector.
4. Press Play — the camera should chase the car from behind.

## 5. Controls recap
- `W` / `Up Arrow` — accelerate
- `S` / `Down Arrow` — reverse / brake
- `A`/`D` or `Left`/`Right` — steer

No extra Input setup needed — Unity's default Input Manager already maps "Horizontal" and "Vertical" to WASD and the arrow keys.

## 6. Add to Build Settings
`File > Build Settings > Add Open Scenes` once this scene is finished, so it's reachable from the Main Menu later.
