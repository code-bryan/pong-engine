using Engine.Core.Components;
using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Systems;

public class InputSystem(EntityManager entityManager, int screenHeight, Keys upKey, Keys downKey) : ISystem
{
    public void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entityId in entityManager.GetEntitiesWith<TransformComponent, InputComponent>())
        {
            var transform =  entityManager.GetComponent<TransformComponent>(entityId);
            var input =  entityManager.GetComponent<InputComponent>(entityId);

            if (keyboardState.IsKeyDown(upKey))
            {
                transform.Position = transform.Position with { Y = transform.Position.Y - input.Speed * deltaTime };
            }
            
            if (keyboardState.IsKeyDown(downKey))
            {
                transform.Position = transform.Position with { Y = transform.Position.Y + input.Speed * deltaTime };
            }

            transform.Position = Vector2.Clamp(
                transform.Position,
                new Vector2(transform.Position.X, 0),
                new Vector2(transform.Position.X, screenHeight - transform.Height)
            );
        }
    }
}