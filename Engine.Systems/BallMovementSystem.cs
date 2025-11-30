using Engine.Core.Components;
using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Microsoft.Xna.Framework;

namespace Engine.Systems;

public class BallMovementSystem(EntityManager entityManager, GameplaySettings gs) : ISystem
{
    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entityId in entityManager.GetEntitiesWith<TagComponent, TransformComponent>())
        {
            var tag = entityManager.GetComponent<TagComponent>(entityId);
            
            if (tag.Tag != "Ball") continue;
            
            var transform =  entityManager.GetComponent<TransformComponent>(entityId);
            
            transform.Position += transform.Velocity * deltaTime;

            if (transform.Position.Y < 0)
            {
                transform.Position = transform.Position with { Y = 0 };
                transform.Velocity = new Vector2(transform.Velocity.X, -transform.Velocity.Y);
            }
            else if(transform.Position.Y + transform.Height > gs.Settings.ScreenHeight)
            {
                transform.Position = transform.Position with { Y = gs.Settings.ScreenHeight - transform.Height };
                transform.Velocity = new Vector2(transform.Velocity.X, -transform.Velocity.Y);
            }
        }
    }
}