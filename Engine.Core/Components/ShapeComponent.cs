using Microsoft.Xna.Framework;

namespace Engine.Core.Components;

public enum ShapeType
{
    Rectangle,
    Circle,
}

public class ShapeComponent(int width, int height)
{
    public ShapeType Type { get; set; } = ShapeType.Rectangle;
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;
    public float Radius { get; set; }

    public Rectangle GetRectangle(TransformComponent transform)
    {
        return new Rectangle((int)transform.Position.X, (int)transform.Position.Y, Width, Height);
    }
}