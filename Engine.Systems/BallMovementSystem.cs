using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Microsoft.Xna.Framework;

namespace Engine.Systems;

public class BallMovementSystem(EntityManager entityManager, int height) : ISystem
{
    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        
    }
}