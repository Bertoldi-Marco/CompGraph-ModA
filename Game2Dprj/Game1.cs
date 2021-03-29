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
        private int xScreenDim;
        private int yScreenDim;
        private Point middleScreen;
        private SelectMode mode;

        //Contents
        private SpriteFont font;
        private Texture2D background;
        private Texture2D cursor;
        private Texture2D target;
        private Texture2D trackButtonStart;
        private Texture2D hitButtonStart;

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

            xScreenDim = _graphics.PreferredBackBufferWidth;
            yScreenDim = _graphics.PreferredBackBufferHeight;
            middleScreen = new Point(xScreenDim/2, yScreenDim/2);
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

            //Shared Initialization
            backgroundStart = new Point((background.Width - xScreenDim) / 2, (background.Height - yScreenDim) / 2); //view in the middle of background texture
            viewDest = new Rectangle(0, 0, xScreenDim, yScreenDim);
            viewSource = new Rectangle(backgroundStart.X, backgroundStart.Y, xScreenDim, yScreenDim);
            cursorRect = new Rectangle((xScreenDim - cursor.Width) / 2, (yScreenDim - cursor.Height) / 2, cursor.Width, cursor.Height);

            trackerGame = new TrackerGame(viewSource, viewDest, cursorRect, xScreenDim, yScreenDim, middleScreen, background, cursor, target, font);
            hittingGame = new HittingGame(viewSource, viewDest, cursorRect, xScreenDim, yScreenDim, middleScreen, background, cursor, target);
            startMenu = new StartMenu(xScreenDim, yScreenDim, GraphicsDevice, background, hitButtonStart, trackButtonStart);
            pause = new Pause(xScreenDim, yScreenDim, GraphicsDevice);
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
                    pause.Update(ref mode, middleScreen);
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
