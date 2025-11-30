using System.Collections.Generic;
using Engine.Core.Components;
using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Engine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine.Desktop;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _whitePixel;
    
    private EntityManager _entityManager;
    private List<ISystem> _systems;
    private int player, ball, enemy;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _entityManager = new EntityManager();
        
        int screenHeight = GraphicsDevice.Viewport.Height;
        int screenWidth = GraphicsDevice.Viewport.Width;
        
        player = _entityManager.CreateEntity();
        _entityManager.AddComponent(player, new InputComponent() { Speed = 400,  upKey = Keys.W, downKey = Keys.S });
        _entityManager.AddComponent(player, new TransformComponent(
            position: new Vector2(50, screenHeight / 2 - 50),
            width: 20,
            height: 100
        ));
        
        
        enemy = _entityManager.CreateEntity();
        _entityManager.AddComponent(enemy, new InputComponent() { Speed = 400, upKey =  Keys.Up, downKey = Keys.Down });
        _entityManager.AddComponent(enemy, new TransformComponent(
            position: new Vector2(screenWidth - 70, screenHeight / 2 - 50),
            width: 20,
            height: 100
        ));
        
        ball = _entityManager.CreateEntity();
        _entityManager.AddComponent(ball, new TransformComponent(
            position: new Vector2(screenWidth / 2 - 10, screenHeight / 2 - 10),
            width: 20,
            height: 20
        ));
        

        _systems = [
            new InputSystem(_entityManager, screenHeight)
        ];

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        // Crea un pixel blanco para dibujar las figuras (Rectángulos)
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // 1. EJECUTAR SISTEMAS
        // // El InputSystem lee el teclado y MODIFICA los datos (Position) en el EntityManager.
        foreach (var system in _systems)
        {
            system.Update(gameTime);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        _spriteBatch.Begin();

        // TODO: Add your drawing code here
        // ** Dibujar Paletas (Entidades con InputComponent y TransformComponent) **
        foreach (var entityId in _entityManager.GetEntitiesWith<TransformComponent, InputComponent>())
        {
            var transform = _entityManager.GetComponent<TransformComponent>(entityId);
        
            _spriteBatch.Draw(_whitePixel, 
                new Rectangle(
                    (int)transform.Position.X, 
                    (int)transform.Position.Y, 
                    transform.Width, 
                    transform.Height), 
                Color.White);
        }
        
        // ** Dibujar la Bola **
        var ballTransform = _entityManager.GetComponent<TransformComponent>(ball);
        _spriteBatch.Draw(_whitePixel, 
            new Rectangle(
                (int)ballTransform.Position.X, 
                (int)ballTransform.Position.Y, 
                ballTransform.Width, 
                ballTransform.Height), 
            Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}