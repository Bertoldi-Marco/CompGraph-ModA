using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//NOTES:
//-Trackergame is still a partial class, needs to be moved to public class
//-In the first call of update, the game window is not started yet. Since this, the setposition is referred to a previous condition and the first pointer read when the window is opened is false

namespace Game2Dprj
{
    //Enum variable type for menu selection----probably not the best place
    public enum SelectMode { menu, trackerGame, hittingGame };
    public partial class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //Screen dimensions
        private int xScreenDim;
        private int yScreenDim;
        private Point middleScreen;
        private SelectMode mode;
        SpriteFont font;
        //Defining instance of HittingGame
        HittingGame hittingGame;
        //Defining instance of StartMenu
        StartMenu startMenu;

     

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
            TrackerInitLoad();
            hittingGame = new HittingGame(backgroundStart, viewSource, viewDest, cursorRect, xScreenDim, yScreenDim, middleScreen, background, cursor, target);
            startMenu = new StartMenu(xScreenDim, yScreenDim, GraphicsDevice);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            switch(mode)
            {
                case SelectMode.menu:
                    IsMouseVisible = true;
                    startMenu.Update(ref mode, middleScreen);
                    break;
                case SelectMode.trackerGame:
                    IsMouseVisible = false;
                    TrackerUpdate(gameTime);
                    break;
                case SelectMode.hittingGame:
                    IsMouseVisible = false;
                    hittingGame.Update(gameTime);
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
                    startMenu.Draw(_spriteBatch, font);
                    break;
                case SelectMode.trackerGame:
                    TrackerDraw();
                    break;
                case SelectMode.hittingGame:
                    hittingGame.Draw(gameTime, _spriteBatch, font);
                    break;
            }            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
