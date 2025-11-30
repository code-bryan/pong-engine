using Engine.Core.Components;
using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Microsoft.Xna.Framework;

namespace Engine.Systems;

public class BallMovementSystem(EntityManager entityManager, GameplaySettings gs) : ISystem
{
    public void Update(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entityId in entityManager.GetEntitiesWith<TagComponent, MovementComponent, ShapeComponent, TransformComponent>())
        {
            var tag = entityManager.GetComponent<TagComponent>(entityId);
            
            if (tag.Tag != "Ball") continue;
            
            var transform =  entityManager.GetComponent<TransformComponent>(entityId);
            var movement = entityManager.GetComponent<MovementComponent>(entityId);
            var shape = entityManager.GetComponent<ShapeComponent>(entityId);
            
            transform.Position += movement.Velocity * deltaTime;

            if (transform.Position.Y < 0)
            {
                transform.Position = transform.Position with { Y = 0 };
                movement.Velocity = new Vector2(movement.Velocity.X, -movement.Velocity.Y);
            }
            else if(transform.Position.Y + shape.Height > gs.Settings.ScreenHeight)
            {
                transform.Position = transform.Position with { Y = gs.Settings.ScreenHeight - shape.Height };
                movement.Velocity = new Vector2(movement.Velocity.X, -movement.Velocity.Y);
            }
        }
    }
}