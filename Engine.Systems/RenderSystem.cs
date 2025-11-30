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

            var destinationRectangle = new Rectangle(
                (int)transform.Position.X,
                (int)transform.Position.Y,      
                shape.Width,
                shape.Height
            );
            
            spriteBatch.Draw(
                render.Texture,
                destinationRectangle,
                render.Color
            );
        }
        
        spriteBatch.End();
    }
}