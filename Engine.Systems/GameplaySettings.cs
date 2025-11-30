using Engine.Core.Configuration;
using Microsoft.Xna.Framework;

namespace Engine.Desktop;

public class GameplaySettings(EngineSettings settings)
{
    public readonly EngineSettings Settings = settings;
    
    private const float InitialSpeed = 250f;
    private Vector2 BallStartPosition => new Vector2(settings.ScreenWidth / 2, settings.ScreenHeight / 2);
}