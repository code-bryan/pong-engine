using Engine.Core.Components;
using Engine.Core.ECS;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Desktop.Prefabs;

public class BallPrefab(EntityManager manager)
{
    public int Create(Texture2D texture2D, GameplaySettings settings)
    {
        var id = manager.CreateEntity();
        
        manager.AddComponent(id, new RenderComponent(texture2D, Color.White));
        manager.AddComponent(id, new TagComponent("Ball"));
        manager.AddComponent(id, new TransformComponent(settings.BallStartPosition));
        manager.AddComponent(id, new ShapeComponent(20, 20));
        manager.AddComponent(id, new MovementComponent()
        {
            Velocity = Vector2.One * settings.BallInitialSpeed,
            Speed = settings.BallInitialSpeed,
        });

        return id;
    }
}