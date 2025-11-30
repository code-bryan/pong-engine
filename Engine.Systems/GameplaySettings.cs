using Engine.Core.Configuration;
using Microsoft.Xna.Framework;

namespace Engine.Systems;

public class GameplaySettings(EngineSettings settings)
{
    public readonly EngineSettings Settings = settings;
    
    public readonly float BallInitialSpeed = 250f;
    public Vector2 BallStartPosition => new Vector2(settings.ScreenWidth / 2, settings.ScreenHeight / 2);
}