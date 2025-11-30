using Engine.Core.Components;
using Engine.Core.Configuration;
using Engine.Core.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Desktop.Prefabs;

public class PlayerPrefab(EntityManager manager)
{
    public int Create(Texture2D texture, EngineSettings settings)
    {
        int id = manager.CreateEntity();
        
        manager.AddComponent(id, new RenderComponent(texture, Color.White));
        manager.AddComponent(id, new ShapeComponent(20, 100));
        manager.AddComponent(id, new TransformComponent(new Vector2(50, settings.ScreenHeight / 2 - 50)));
        
        manager.AddComponent(id, new InputComponent()
        {
            upKey = Keys.W,
            downKey = Keys.S
        });

        manager.AddComponent(id, new MovementComponent()
        {
            Velocity = Vector2.Zero,
            Speed = 400,
        });
        
        manager.AddComponent(id, new ScoreComponent()
        {
            Score = 0,
            TextPosition = new Vector2(settings.ScreenWidth / 4, 20)
        });

        return id;
    }   
}