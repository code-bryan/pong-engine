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
        foreach (var ballId in entityManager.GetEntitiesWith<TagComponent, TransformComponent>())
        {
            var tag = entityManager.GetComponent<TagComponent>(ballId);
            if (tag.Tag != "Ball") continue;
            
            var ballTransform = entityManager.GetComponent<TransformComponent>(ballId);

            if (ballTransform.Position.X < 0 || ballTransform.Position.X + + ballTransform.Width > gs.Settings.ScreenWidth)
            {
                HandleScoring(ballId, ballTransform);
                return;
            }

            foreach (var id in entityManager.GetEntitiesWith<InputComponent, TransformComponent, ScoreComponent>())
            {
                var transform = entityManager.GetComponent<TransformComponent>(id);

                var ballRect = new Rectangle(
                    (int)ballTransform.Position.X,
                    (int)ballTransform.Position.Y,
                    ballTransform.Width,
                    ballTransform.Height
                );
                
                var paddleRect = new Rectangle(
                    (int)transform.Position.X,
                    (int)transform.Position.Y,
                    transform.Width,
                    transform.Height
                );

                if (!ballRect.Intersects(paddleRect)) continue;
                // Invertir la dirección horizontal (rebote)
                ballTransform.Velocity = new Vector2(-ballTransform.Velocity.X, ballTransform.Velocity.Y);
                        
                // Opcional: Aumentar la velocidad
                ballTransform.Velocity *= 1.05f; 
                        
                // Corrección de posición (para evitar que la bola se pegue a la paleta)
                if (ballTransform.Velocity.X > 0) // Bola va hacia la derecha, golpeó la paleta izquierda
                {
                    ballTransform.Position = ballTransform.Position with
                    {
                        X = transform.Position.X + transform.Width
                    };
                }
                else // Bola va hacia la izquierda, golpeó la paleta derecha
                {
                    ballTransform.Position = ballTransform.Position with
                    {
                        X = transform.Position.X - ballTransform.Width
                    };
                }
            }
        }
    }

    private void HandleScoring(int ballId, TransformComponent component)
    {
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
        
        var newVelX = (component.Velocity.X > 0 ? -1 : 1) * gs.BallInitialSpeed;
        component.Velocity = new Vector2(newVelX, gs.BallInitialSpeed);
    }
}