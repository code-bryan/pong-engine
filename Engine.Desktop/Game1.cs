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
    private SpriteFont _font;
    
    private EntityManager _entityManager;
    private List<ISystem> _systems;
    private RenderSystem _renderSystem;
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
        
        var screenHeight = GraphicsDevice.Viewport.Height;
        var screenWidth = GraphicsDevice.Viewport.Width;
        const float initialSpeed = 250f;
        var ballStartPosition = new Vector2(screenWidth / 2 - 10, screenHeight / 2 - 10);
        
        player = _entityManager.CreateEntity();
        _entityManager.AddComponent(player, new InputComponent()
        {
            Speed = 400,
            upKey = Keys.W,
            downKey = Keys.S
        });
        _entityManager.AddComponent(player, new TransformComponent(
            position: new Vector2(50, screenHeight / 2 - 50),
            width: 20,
            height: 100
        ));
        _entityManager.AddComponent(player, new ScoreComponent()
        {
            Score = 0,
            TextPosition = new Vector2(screenWidth / 4, 20)
        });
        
        
        enemy = _entityManager.CreateEntity();
        _entityManager.AddComponent(enemy, new InputComponent()
        {
            Speed = 400,
            upKey = Keys.Up,
            downKey = Keys.Down
        });
        _entityManager.AddComponent(enemy, new TransformComponent(
            position: new Vector2(screenWidth - 70, screenHeight / 2 - 50),
            width: 20,
            height: 100
        ));
        _entityManager.AddComponent(enemy, new ScoreComponent()
        {
            Score = 0,
            TextPosition = new Vector2(screenWidth * 3 / 4, 20)
        });
        
        
        ball = _entityManager.CreateEntity();
        _entityManager.AddComponent(ball, new BallComponent());
        _entityManager.AddComponent(ball, new TransformComponent(
            position: ballStartPosition,
            width: 20,
            height: 20
        ));
        
        // Asignamos una velocidad inicial para que se mueva
        var ballTransform = _entityManager.GetComponent<TransformComponent>(ball);
        ballTransform.Velocity = new Vector2(initialSpeed, initialSpeed);

        _systems = [
            new InputSystem(_entityManager, screenHeight),
            new BallMovementSystem(_entityManager, screenHeight),
            new CollisionSystem(_entityManager, screenWidth, screenHeight, initialSpeed, ballStartPosition),
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
        _font = Content.Load<SpriteFont>("Fonts/Arial");
        
        _renderSystem = new RenderSystem(_entityManager, _spriteBatch);
        
        _entityManager.AddComponent(player, new RenderComponent(_whitePixel, Color.White));
        _entityManager.AddComponent(enemy, new RenderComponent(_whitePixel, Color.White));
        _entityManager.AddComponent(ball, new RenderComponent(_whitePixel, Color.White));
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
        
        _renderSystem.Draw();
        
        _spriteBatch.Begin();
        
        foreach (var entityId in _entityManager.GetEntitiesWith<ScoreComponent>())
        {
            var scoreComp = _entityManager.GetComponent<ScoreComponent>(entityId);
        
            _spriteBatch.DrawString(
                _font, 
                scoreComp.Score.ToString(), 
                scoreComp.TextPosition, 
                Color.White);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}