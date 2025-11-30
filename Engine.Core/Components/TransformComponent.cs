using Microsoft.Xna.Framework;

namespace Engine.Core.Components;

public class TransformComponent(Vector2 position, int width, int height)
{
    public Vector2 Position { get; set; } = position;
    public Vector2 Velocity { get; set; }

    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
}