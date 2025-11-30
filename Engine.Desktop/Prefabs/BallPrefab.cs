using Engine.Core.Components;
using Engine.Core.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Desktop.Prefabs;

public class BallPrefab(EntityManager manager)
{
    public int Create(Texture2D texture2D, float initialSpeed, Vector2 position)
    {
        var id = manager.CreateEntity();
        
        manager.AddComponent(id, new RenderComponent(texture2D, Color.White));
        manager.AddComponent(id, new BallComponent());
        manager.AddComponent(id, new TransformComponent(
            position: position,
            width: 20,
            height: 20
        ));
        
        // Asignamos una velocidad inicial para que se mueva
        var ballTransform = manager.GetComponent<TransformComponent>(id);
        ballTransform.Velocity = new Vector2(initialSpeed, initialSpeed);

        return id;
    }
}