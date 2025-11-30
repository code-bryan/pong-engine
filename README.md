# 🚀 ECS-PONG: A Data-Driven Game Engine Framework

This project is a demonstration and proof-of-concept for a custom, modular **Game Engine Framework** built using **MonoGame** and the **Entity Component System (ECS)** architectural pattern.

The goal is to establish a clear separation of concerns, preparing the framework for scaling to complex 2D and 3D projects, in line with modern professional game development standards.

---

## 💡 Core Architectural Ideas

The engine structure prioritizes modularity and data flow, strictly adhering to the ECS principles to maximize performance and flexibility.

### 1. Entity Component System (ECS)

The core is built around three concepts:

* **Entities (IDs):** Simple unique integers managed by the `EntityManager`. They are abstract identifiers and hold no data or logic.
* **Components (Data):** Simple C# classes holding only data (e.g., `Position`, `Velocity`, `Color`). **Components are stored separately from Entities.**
    * **Refactor Highlights:** The engine separates the concerns into distinct data components:
        * `TransformComponent` (Position, Rotation, Scale)
        * `ShapeComponent` (Width, Height, Type)
        * `MovementComponent` (Velocity, Speed)
        * `RenderComponent` (Texture, Color)
* **Systems (Logic):** Classes that iterate over Entities that possess a specific **set of components** (the "query" or "filter") and execute logic (the "gameplay"). **Systems hold all the logic.**

### 2. Architectural Separation (Modular Projects)

The solution is divided into four distinct projects, enforcing strict dependency rules:

| Project | Role | Dependencies |
| :--- | :--- | :--- |
| **`Engine.Core`** | **The ECS Framework.** Contains `EntityManager`, all **Components**, and `ISystem` interfaces. | None (Pure C# Library) |
| **`Pong.Core`** | **Configuration & Game Data.** Contains `EngineSettings` and `GameplaySettings`. | References `Engine.Core` |
| **`Pong.Systems`** | **The Game Logic.** Contains all **Systems** (`CollisionSystem`, `AISystem`, etc.). | References `Engine.Core` & `Pong.Core` |
| **`Pong.Desktop`** | **The Launcher.** Contains `Game1.cs`, initializes hardware, loads assets, and executes the system loop. | References all other projects |

---

## 🎮 Gameplay Features & Engine Improvements

The Pong implementation served as a robust testing ground for several advanced engine features:

### 1. Data-Driven Configuration

* **`EngineSettings`:** Stores immutable hardware configuration (Screen Width/Height).
* **`GameplaySettings`:** Stores mutable game constants (Paddle Speed, Ball Size).
* **Dependency Injection:** These settings are passed into relevant Systems and Prefabs, centralizing all configuration and avoiding hard-coded constants in the logic.

### 2. Advanced Collision and Physics

* **Decoupled Geometry:** The separation of **`TransformComponent`** (Position) and **`ShapeComponent`** (Width/Height) allows the `CollisionSystem` to accurately calculate the **Axis-Aligned Bounding Box (AABB)** by reading two sources of data.
* **Dynamic Angled Bounce:** The `CollisionSystem` implements **dynamic deflection**, calculating the rebound angle based on where the ball strikes the paddle (normalized hit distance). This replaces simple velocity inversion, significantly improving gameplay quality.
    * The angle is capped at $60^\circ$ ($\pi/3$ radians) to ensure the ball maintains horizontal velocity and doesn't get stuck in vertical motion.

### 3. Dedicated AI System

* **`AIComponent`:** A dedicated data component holds the difficulty and error margin for computer-controlled entities.
* **Reactive Tracking:** The **`AISystem`** implements a **limited, reactive movement pattern** that moves the AI paddle towards the ball's current Y-position. Crucially, the movement is limited by a maximum speed (`maxMoveDistance`), ensuring the AI is **beatable** and does not "teleport," creating a challenging but fair opponent.

### 4. Code Quality and Maintenance

* **`TagComponent`:** Used as a universal identifier (e.g., `"Ball"`, `"Paddle"`), replacing empty component classes. This allows systems to filter entities based on role without needing unnecessary component dependencies.
* **Prefab Pattern:** Entity creation is clean and reusable via the `Pong.Desktop.Prefab` factory classes, making `Game1.cs` simple and readable.
