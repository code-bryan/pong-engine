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
        foreach (var ballId in entityManager.GetEntitiesWith<TagComponent, MovementComponent, TransformComponent, ShapeComponent>())
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
                
                var ballRect = ballShape.GetRectangle(ballTransform);
                var paddleRect = shape.GetRectangle(transform);

                if (!ballRect.Intersects(paddleRect)) continue;
                
                float centerY = transform.Position.Y + (shape.Height / 2f);
                float ballCenterY = ballTransform.Position.Y + (ballShape.Height / 2f);

                float hit = ballCenterY - centerY;
                float normalizedHit = hit / (shape.Height / 2f);
                normalizedHit = MathHelper.Clamp(normalizedHit, -1f, 1f);
                
                
                const float maxBounceAngle = MathHelper.Pi / 3f; 
                float newAngle = normalizedHit * maxBounceAngle;
                float currentSpeed = movement.Velocity.Length(); 
                currentSpeed *= 1.05f;

                int directionX = ballTransform.Position.X < gs.Settings.ScreenWidth / 2 ? 1 : -1;

                movement.Velocity = new Vector2(
                    directionX * (float)Math.Cos(newAngle) * currentSpeed,
                    (float)Math.Sin(newAngle) * currentSpeed
                );
                
                if (movement.Velocity.X > 0) // Golpea Paleta 1 (Sale a la Derecha)
                {
                    // Posiciona en el borde DERECHO de la Paleta 1.
                    ballTransform.Position = ballTransform.Position with { X = transform.Position.X + shape.Width }; 
                }
                else // Golpea Paleta 2 (Sale a la Izquierda)
                {
                    // Posiciona en el borde IZQUIERDO de la Paleta 2.
                    ballTransform.Position = ballTransform.Position with { X = transform.Position.X - ballShape.Width };
                }
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