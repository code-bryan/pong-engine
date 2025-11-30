using Engine.Core.Components;
using Engine.Core.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Systems;

public class RenderSystem(EntityManager entityManager, SpriteBatch spriteBatch)
{
    public void Draw()
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, null);

        foreach (var entityId in entityManager.GetEntitiesWith<TransformComponent, RenderComponent, ShapeComponent>())
        {
            var transform = entityManager.GetComponent<TransformComponent>(entityId);
            var render = entityManager.GetComponent<RenderComponent>(entityId);
            var shape = entityManager.GetComponent<ShapeComponent>(entityId);
            
            spriteBatch.Draw(
                render.Texture,
                shape.GetRectangle(transform),
                render.Color
            );
        }
        
        spriteBatch.End();
    }
}