using Engine.Core.Components;
using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Microsoft.Xna.Framework;

namespace Engine.Systems;

public class AiSystem(EntityManager manager, GameplaySettings settings) : ISystem
{
    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        int ballId = manager.GetEntitiesWith<TagComponent, TransformComponent, MovementComponent, ShapeComponent>()
            .FirstOrDefault(id => manager.GetComponent<TagComponent>(id)?.Tag == "Ball");

        if (ballId == 0) return;

        var ballTransform = manager.GetComponent<TransformComponent>(ballId);
        var ballMovement = manager.GetComponent<MovementComponent>(ballId);
        var ballShape = manager.GetComponent<ShapeComponent>(ballId);
        
        float ballCenterY = ballTransform.Position.Y + (ballShape.Height / 2f);

        foreach (var paddleId in manager.GetEntitiesWith<AIComponent, TransformComponent, ShapeComponent, MovementComponent>())
        {
            var paddleTransform = manager.GetComponent<TransformComponent>(paddleId);
            var ai = manager.GetComponent<AIComponent>(paddleId);
            var shape = manager.GetComponent<ShapeComponent>(paddleId);
            var movement = manager.GetComponent<MovementComponent>(paddleId);
            

            float paddleCenterY = paddleTransform.Position.Y + (shape.Height / 2f);
            bool isRightPaddle = paddleTransform.Position.X > settings.Settings.ScreenWidth / 2;

            if (!isRightPaddle || !(ballMovement.Velocity.X > 0)) continue;
            
            
            float differenceY = ballCenterY - paddleCenterY;
            float maxMoveDistance = movement.Speed * ai.Difficulty * deltaTime;
                
            if (Math.Abs(differenceY) > maxMoveDistance)
            {
                float moveAmount = Math.Sign(differenceY) * maxMoveDistance;
                paddleTransform.Position =
                    paddleTransform.Position with { Y = paddleTransform.Position.Y + moveAmount };
            }
            else
            {
                paddleTransform.Position =
                    paddleTransform.Position with { Y = paddleTransform.Position.Y + differenceY };
            }

            paddleTransform.Position = paddleTransform.Position with
            {
                Y = MathHelper.Clamp(
                    paddleTransform.Position.Y,
                    0,
                    settings.Settings.ScreenHeight - shape.Height
                )
            };
        }
    }
}