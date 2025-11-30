using Engine.Core.Components;
using Engine.Core.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Desktop.Prefabs;

public class EnemyPrefab(EntityManager manager)
{
    public int Create(Texture2D texture, int screenWidth, int screenHeight)
    {
        int id = manager.CreateEntity();
        manager.AddComponent(id, new RenderComponent(texture, Color.White));
        manager.AddComponent(id, new InputComponent()
        {
            Speed = 400,
            upKey = Keys.Up,
            downKey = Keys.Down
        });
        manager.AddComponent(id, new TransformComponent(
            position: new Vector2(screenWidth - 70, screenHeight / 2 - 50),
            width: 20,
            height: 100
        ));
        manager.AddComponent(id, new ScoreComponent()
        {
            Score = 0,
            TextPosition = new Vector2(screenWidth * 3 / 4, 20)
        });

        return id;
    }
}