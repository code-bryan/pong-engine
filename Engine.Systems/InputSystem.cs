using Engine.Core.Components;
using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Systems;

public class InputSystem(EntityManager entityManager, GameplaySettings gs) : ISystem
{
    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entityId in entityManager.GetEntitiesWith<TransformComponent, InputComponent, ShapeComponent>())
        {
            var transform =  entityManager.GetComponent<TransformComponent>(entityId);
            var input =  entityManager.GetComponent<InputComponent>(entityId);
            var shape = entityManager.GetComponent<ShapeComponent>(entityId);

            if (keyboardState.IsKeyDown(input.upKey))
            {
                transform.Position = transform.Position with { Y = transform.Position.Y - input.Speed * deltaTime };
            }
            
            if (keyboardState.IsKeyDown(input.downKey))
            {
                transform.Position = transform.Position with { Y = transform.Position.Y + input.Speed * deltaTime };
            }

            transform.Position = Vector2.Clamp(
                transform.Position,
                new Vector2(transform.Position.X, 0),
                new Vector2(transform.Position.X, gs.Settings.ScreenHeight - shape.Height)
            );
        }
    }
}