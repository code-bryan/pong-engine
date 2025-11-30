using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Components;

public class RenderComponent(Texture2D texture, Color color)
{
    public Texture2D Texture { get; set; } = texture;

    public Color Color { get; set; } = color;

    public float LayerDepth { get; set; } = 0f;
}