using System.Collections.Generic;
using Engine.Core.Components;
using Engine.Core.Configuration;
using Engine.Core.ECS;
using Engine.Core.Interfaces;
using Engine.Desktop.Prefabs;
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

    private EngineSettings _settings;
    private GameplaySettings _gameplaySettings;
    
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
        _settings = new EngineSettings(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        _gameplaySettings = new GameplaySettings(_settings);

        _systems = [
            new InputSystem(_entityManager, _gameplaySettings),
            new BallMovementSystem(_entityManager, _gameplaySettings),
            new CollisionSystem(_entityManager, _gameplaySettings),
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

        new PlayerPrefab(_entityManager).Create(_whitePixel, _settings);
        new EnemyPrefab(_entityManager).Create(_whitePixel,_settings);
        new BallPrefab(_entityManager).Create(_whitePixel, _gameplaySettings);
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
                Color.White
            );
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}