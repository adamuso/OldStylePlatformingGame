#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OldStylePlatformingGame.GFX;
using OldStylePlatformingGame.Engine;
#endregion

namespace OldStylePlatformingGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Resolution resolution;
        private TextureManager textureManager;
        private Level level;
        private SpriteFont defaultFont;
        private Camera2D camera;

        private Effect testEffect;
        private RenderTarget2D lightsMask, mainTarget;
        private Texture2D textMask;
        private Engine.Sprites.FrameCounter f;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            //this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

            base.Initialize();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            //resolution.ResolutionWidth = Window.ClientBounds.Width;
            //resolution.ResolutionHeight = Window.ClientBounds.Height;

            //resolution?.Apply();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            defaultFont = Content.Load<SpriteFont>("DefaultFont");
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Resolution.Initialize(graphics, 800, 450);
            Resolution.KeepAspect = true;
            Resolution.Fullscreen = false;
            resolution = Resolution.GetResolution(1280, 720);
            resolution.Apply();

            camera = new Camera2D(resolution);

            textureManager = new TextureManager(this);
            textureManager.LoadContent();

            level = new Level(this, new TileSet(TextureManager.tileset, TextureManager.tilesetmask, 32));
            level.DEBUG();
            Engine.Storage.LevelFile.SaveLevel(level, "test.lvl");
            level = Engine.Storage.LevelFile.LoadLevel("test.lvl", this, new TileSet(TextureManager.tileset, TextureManager.tilesetmask, 32));

            lightsMask = new RenderTarget2D(GraphicsDevice, 1280, 720);
            mainTarget = new RenderTarget2D(GraphicsDevice, 1280, 720);
            testEffect = Content.Load<Effect>("test");
            textMask = Content.Load<Texture2D>("lightmask");
            f = new Engine.Sprites.FrameCounter();
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            level.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            f.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.SetRenderTarget(lightsMask);
            GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera.Projection);
            spriteBatch.Draw(textMask, new Vector2(level.P.Position.X + 16 - textMask.Width / 2, level.P.Position.Y + 16 - textMask.Height / 2), Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(mainTarget);
            GraphicsDevice.Clear(Color.Black);
            level.Draw(spriteBatch);

            GraphicsDevice.SetRenderTarget(null);
            resolution.UpdateViewport();
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, resolution.Projection);
            //testEffect.Parameters["lightMask"].SetValue(lightsMask);
            testEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public Resolution Resolution { get { return resolution; } }
        public Level CurrentLevel { get { return level; } }
        public Camera2D Camera { get { return camera; } }
        public TextureManager TextureManager { get { return textureManager; } }
        public SpriteFont DefaultFont { get { return defaultFont; } }
        public Engine.Sprites.FrameCounter fr { get { return this.f; } }
    }
}
