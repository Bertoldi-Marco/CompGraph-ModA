﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game2Dprj
{
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //Screen dimensions
        private int xScreenDim;
        private int yScreenDim;
        private Point middleScreen;

        SpriteFont font;
        //defining istance of hittingGame
        HittingGame hittingGame;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Set window dimensions to full screen
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            xScreenDim = _graphics.PreferredBackBufferWidth;
            yScreenDim = _graphics.PreferredBackBufferHeight;

            middleScreen = new Point(xScreenDim/2, yScreenDim/2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Basic_font");
            TrackerInitLoad();

            hittingGame = new HittingGame(backgroundStart, viewSource, viewDest, cursorRect, xScreenDim, yScreenDim, middleScreen, background, cursor, target);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            hittingGame.Update(gameTime);
            
            //TrackerUpdate();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            hittingGame.Draw(gameTime, _spriteBatch,font);
            //TrackerDraw();
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
