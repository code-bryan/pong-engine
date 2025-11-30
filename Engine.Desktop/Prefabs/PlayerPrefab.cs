using Engine.Core.Components;
using Engine.Core.Configuration;
using Engine.Core.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Desktop.Prefabs;

public class PlayerPrefab(EntityManager manager)
{
    public int Create(Texture2D texture, EngineSettings settings)
    {
        int id = manager.CreateEntity();
        
        manager.AddComponent(id, new RenderComponent(texture, Color.White));
        
        manager.AddComponent(id, new InputComponent()
        {
            Speed = 400,
            upKey = Keys.W,
            downKey = Keys.S
        });
        
        manager.AddComponent(id, new TransformComponent(
            position: new Vector2(50, settings.ScreenHeight / 2 - 50),
            width: 20,
            height: 100
        ));
        
        manager.AddComponent(id, new ScoreComponent()
        {
            Score = 0,
            TextPosition = new Vector2(settings.ScreenWidth / 4, 20)
        });

        return id;
    }   
}