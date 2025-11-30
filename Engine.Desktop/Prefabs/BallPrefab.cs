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
        manager.AddComponent(id, new BallComponent());
        manager.AddComponent(id, new TransformComponent(
            position: settings.BallStartPosition,
            width: 20,
            height: 20
        ));
        
        // Asignamos una velocidad inicial para que se mueva
        var ballTransform = manager.GetComponent<TransformComponent>(id);
        ballTransform.Velocity = Vector2.One * settings.BallInitialSpeed;

        return id;
    }
}