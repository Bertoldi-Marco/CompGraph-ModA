using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    //Enum variable type for menu selection----probably not the best place
    public enum SelectMode { menu, trackerGame, hittingGame , pause};
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Screen dimensions
        private Point screenDim;
        private Point middleScreen;

        //Mode related variables
        private SelectMode mode;
        private SelectMode prevMode;
        private MouseState prevMouse;

        //Contents
        private SpriteFont font;
        private Texture2D background;
        private Texture2D cursor;
        private Texture2D target;
        private Texture2D trackButtonStart;
        private Texture2D hitButtonStart;
        private Texture2D mouseMenuPointer;

        //Shared entities derived from contents
        private Point backgroundStart;
        private Rectangle viewSource;
        private Rectangle viewDest;
        private Rectangle cursorRect;

        //Defining instance of HittingGame
        HittingGame hittingGame;

        //Defining instance of TrackerGame
        TrackerGame trackerGame;

        //Defining instance of StartMenu
        StartMenu startMenu;

        //Defining instance of Pause
        Pause pause;


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

            screenDim.X = _graphics.PreferredBackBufferWidth;
            screenDim.Y = _graphics.PreferredBackBufferHeight;
            middleScreen = new Point(screenDim.X / 2, screenDim.Y / 2);
            mode = SelectMode.menu;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Basic_font");
            background = Content.Load<Texture2D>("landscape");
            cursor = Content.Load<Texture2D>("cursor");
            target = Content.Load<Texture2D>("sphere");
            hitButtonStart = Content.Load<Texture2D>("hittingButtonMenu");
            trackButtonStart = Content.Load<Texture2D>("trackerButtonMenu");
            mouseMenuPointer = Content.Load<Texture2D>("mousePointer");

            //Shared Initialization
            backgroundStart = new Point((background.Width - screenDim.X) / 2, (background.Height - screenDim.Y) / 2); //view in the middle of background texture
            viewDest = new Rectangle(0, 0, screenDim.X, screenDim.Y);
            viewSource = new Rectangle(backgroundStart.X, backgroundStart.Y, screenDim.X, screenDim.Y);
            cursorRect = new Rectangle((screenDim.X - cursor.Width) / 2, (screenDim.Y - cursor.Height) / 2, cursor.Width, cursor.Height);

            //To update the contructors using a point instead xscreen yscreen
            trackerGame = new TrackerGame(viewSource, viewDest, cursorRect, screenDim, middleScreen, background, cursor, target, font);
            hittingGame = new HittingGame(viewSource, viewDest, cursorRect, screenDim, middleScreen, background, cursor, target);
            startMenu = new StartMenu(screenDim.X, screenDim.Y, GraphicsDevice, background, hitButtonStart, trackButtonStart, mouseMenuPointer);
            pause = new Pause(screenDim.X, screenDim.Y, GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.P))
            {
                if (mode == SelectMode.hittingGame || mode == SelectMode.trackerGame)
                {
                    prevMode = mode;
                    prevMouse = Mouse.GetState();
                    mode = SelectMode.pause;
                }
            }
            switch(mode)
            {
                case SelectMode.menu:
                    IsMouseVisible = true;
                    startMenu.Update(ref mode, middleScreen);
                    break;
                case SelectMode.trackerGame:
                    IsMouseVisible = false;
                    trackerGame.Update(gameTime);
                    break;
                case SelectMode.hittingGame:
                    IsMouseVisible = false;
                    hittingGame.Update(gameTime);
                    break;
                case SelectMode.pause:
                    IsMouseVisible = true;
                    pause.Update(ref mode, prevMode, prevMouse);
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            switch (mode)
            {
                case SelectMode.menu:
                    startMenu.Draw(_spriteBatch);
                    break;
                case SelectMode.trackerGame:
                    trackerGame.Draw(_spriteBatch);
                    break;
                case SelectMode.hittingGame:
                    hittingGame.Draw(gameTime, _spriteBatch, font);
                    break;
                case SelectMode.pause:
                    pause.Draw(_spriteBatch, font);
                    break;
            }            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
