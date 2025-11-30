using Engine.Core.Components;
using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Microsoft.Xna.Framework;

namespace Engine.Systems;

public class CollisionSystem(
    EntityManager entityManager, GameplaySettings gs) : ISystem
{
    public void Update(GameTime gameTime)
    {
        foreach (var ballId in entityManager.GetEntitiesWith<TagComponent, MovementComponent, TransformComponent>())
        {
            var tag = entityManager.GetComponent<TagComponent>(ballId);
            if (tag.Tag != "Ball") continue;
            
            var ballTransform = entityManager.GetComponent<TransformComponent>(ballId);
            var ballShape = entityManager.GetComponent<ShapeComponent>(ballId);
            
            var movement = entityManager.GetComponent<MovementComponent>(ballId);

            if (ballTransform.Position.X < 0 || ballTransform.Position.X + + ballShape.Width > gs.Settings.ScreenWidth)
            {
                HandleScoring(ballId, ballTransform);
                return;
            }

            foreach (var id in entityManager.GetEntitiesWith<InputComponent, TransformComponent, ScoreComponent, ShapeComponent>())
            {
                var transform = entityManager.GetComponent<TransformComponent>(id);
                var shape = entityManager.GetComponent<ShapeComponent>(id);

                var ballRect = shape.GetRectangle(ballTransform);
                var paddleRect = shape.GetRectangle(transform);

                if (!ballRect.Intersects(paddleRect)) continue;
                
                movement.Velocity = new Vector2(-movement.Velocity.X, movement.Velocity.Y);
                movement.Velocity *= 1.05f;
            }
        }
    }

    private void HandleScoring(int ballId, TransformComponent component)
    {
        var ballMovement = entityManager.GetComponent<MovementComponent>(ballId);
        
        var enemyScore = component.Position.X < 0;

        foreach (var id in entityManager.GetEntitiesWith<ScoreComponent>())
        {
            var scoreComponent = entityManager.GetComponent<ScoreComponent>(id);
            var transform = entityManager.GetComponent<TransformComponent>(id);

            var isPlayer = transform.Position.X > gs.Settings.ScreenWidth / 2;

            if (isPlayer && enemyScore || !isPlayer && !enemyScore)
            {
                scoreComponent.Score++;
            }
        }

        component.Position = gs.BallStartPosition;
        float newVelocityX = (ballMovement.Velocity.X > 0 ? -1 : 1) * gs.BallInitialSpeed;
        ballMovement.Velocity = new Vector2(newVelocityX, gs.BallInitialSpeed);
    }
}