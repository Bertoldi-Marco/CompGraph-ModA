
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game2Dprj
{
    //Enum variable type for menu selection----probably not the best place
    public enum SelectMode { menu, trackerGame, hittingGame, pause, results, exiting }
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

        //tapullo momentaneo
        //private SelectMode oldMode;

        //Contents
        private SpriteFont font;
        private Texture2D help;
        private Texture2D help_info;
        private Texture2D help_info_stats;
        private Texture2D title;
        private Texture2D background;
        private Texture2D cursor;
        private Texture2D target;
        private Texture2D trackButtonStart;
        private Texture2D hitButtonStart;
        private Texture2D mouseMenuPointer;
        private Texture2D resumeButton;
        private Texture2D menuButton;
        private Texture2D menuButtonLarge;
        private Texture2D quitButton;
        private Texture2D goButton;
        private Texture2D freccia;
        private Texture2D pentagono;
        private Texture2D triangolo;
        private Texture2D sphereAtlas;
        private Texture2D explosionAtlas;
        private Texture2D backgroundResult;
        private Texture2D knob;
        private Texture2D slide;
        private Texture2D board;
        private Texture2D exit;
        private Texture2D dialog;
        private Texture2D quitButtonExit;
        private Texture2D backButtonExit;
        private Texture2D backgroundStatsInGame;
        private SoundEffect[] glassBreak;
        private Song menuSong;
        private Song gameSong;
        private SoundEffect ticking;
        

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

        //Defining instance of Results
        Results results;

        //Instance of Exit
        Exit exitGame;

        //Shared parameters
        float mouseSens;
        public float volume;

        //SoundEffect
        SoundEffect onButton;
        SoundEffect clickButton;
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
            //_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            //_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            //_graphics.IsFullScreen = true;
            //_graphics.ApplyChanges();
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            screenDim.X = _graphics.PreferredBackBufferWidth;
            screenDim.Y = _graphics.PreferredBackBufferHeight;
            middleScreen = new Point(screenDim.X / 2, screenDim.Y / 2);
            mode = SelectMode.menu;

            mouseSens = 1;
            volume = 0.5f;

            glassBreak = new SoundEffect[4];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Basic_font");
            background = Content.Load<Texture2D>("background_edited");
            cursor = Content.Load<Texture2D>("cursor");
            target = Content.Load<Texture2D>("sphere");
            hitButtonStart = Content.Load<Texture2D>("hittingButton");
            trackButtonStart = Content.Load<Texture2D>("trackingButton");
            board = Content.Load<Texture2D>("board");
            mouseMenuPointer = Content.Load<Texture2D>("mousePointer");
            resumeButton = Content.Load<Texture2D>("resumebutton");
            menuButton = Content.Load<Texture2D>("menubutton");
            goButton = Content.Load<Texture2D>("ceppo");
            freccia = Content.Load<Texture2D>("cone2");
            pentagono = Content.Load<Texture2D>("pentagonorec");
            triangolo = Content.Load<Texture2D>("triangoloTracker");
            sphereAtlas = Content.Load<Texture2D>("atlasSphere");
            explosionAtlas = Content.Load<Texture2D>("explosionAtlas");
            backgroundResult = Content.Load<Texture2D>("sfondoResult");
            knob = Content.Load<Texture2D>("steelKnob");
            slide = Content.Load<Texture2D>("slider");
            help = Content.Load<Texture2D>("help");
            help_info = Content.Load<Texture2D>("help_info");
            help_info_stats = Content.Load<Texture2D>("help_info_stats");
            title = Content.Load<Texture2D>("title_aim_trainer");
            backgroundStatsInGame = Content.Load<Texture2D>("background_stats_ingame");
            for (int i = 0; i< glassBreak.Length; i++)
            {
                glassBreak[i] = Content.Load<SoundEffect>("audio\\breakingLightBulb" + i);
            }
            menuSong = Content.Load<Song>("audio\\menuSong");        
            gameSong = Content.Load<Song>("audio\\gameSongCut");
            onButton = Content.Load<SoundEffect>("audio\\OnButton");
            clickButton = Content.Load<SoundEffect>("audio\\ClickButton");
            ticking = Content.Load<SoundEffect>("audio\\singleTick");
            dialog = Content.Load<Texture2D>("exitBoard");
            quitButtonExit = Content.Load<Texture2D>("quitButtonExit");
            backButtonExit = Content.Load<Texture2D>("backButtonExit");
            exit = Content.Load<Texture2D>("exit-button");
            menuButtonLarge = Content.Load<Texture2D>("menubuttonLarge");

            //Shared Initialization
            backgroundStart = new Point((background.Width - screenDim.X) / 2, (background.Height - screenDim.Y) / 2); //view in the middle of background texture
            viewDest = new Rectangle(0, 0, screenDim.X, screenDim.Y);
            viewSource = new Rectangle(backgroundStart.X, backgroundStart.Y, screenDim.X, screenDim.Y);
            cursorRect = new Rectangle((screenDim.X - cursor.Width / 2) / 2, (screenDim.Y - cursor.Height / 2) / 2, cursor.Width / 2, cursor.Height / 2);

            trackerGame = new TrackerGame(viewSource, viewDest, cursorRect, screenDim, middleScreen, background, cursor, target, font, sphereAtlas, explosionAtlas, ticking, goButton, onButton, clickButton, gameSong, backgroundStatsInGame);
            hittingGame = new HittingGame(viewSource, viewDest, cursorRect, screenDim, middleScreen, background, cursor, target, sphereAtlas, explosionAtlas, goButton, glassBreak, onButton, clickButton, gameSong, backgroundStatsInGame);
            startMenu = new StartMenu(screenDim, GraphicsDevice, background, hitButtonStart, trackButtonStart, mouseMenuPointer, knob, slide, font, volume, menuSong, onButton, clickButton, help, help_info, title, board, exit);           
            pause = new Pause(screenDim, GraphicsDevice, resumeButton, menuButtonLarge, exit, mouseMenuPointer, knob, slide, font, mouseSens, volume, onButton, clickButton);
            results = new Results(screenDim, GraphicsDevice, exit, menuButtonLarge, mouseMenuPointer, hittingGame, trackerGame, freccia, pentagono, triangolo, font, backgroundResult, help, help_info_stats, onButton, clickButton);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && mode != SelectMode.exiting && mode != SelectMode.trackerGame && mode != SelectMode.hittingGame && mode != SelectMode.pause)
            {
                CheckExit();
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.P) || (mode == SelectMode.hittingGame && Keyboard.GetState().IsKeyDown(Keys.Escape)) || (mode == SelectMode.trackerGame && Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                if (mode == SelectMode.hittingGame)
                {
                    MediaPlayer.Pause();
                    prevMode = mode;
                    prevMouse = Mouse.GetState();
                    mode = SelectMode.pause;
                    pause.FreezeScreen(GraphicsDevice, screenDim);
                    hittingGame.PauseSound();
                    hittingGame.oldMouse = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
                }
                if (mode == SelectMode.trackerGame)
                {
                    MediaPlayer.Pause();
                    prevMode = mode;
                    prevMouse = Mouse.GetState();
                    mode = SelectMode.pause;
                    pause.FreezeScreen(GraphicsDevice, screenDim);
                    trackerGame.PauseSound();
                    trackerGame.oldMouse = new MouseState(0, 0, 0, ButtonState.Pressed, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);
                }
            }

            switch(mode)
            {
                case SelectMode.menu:
                    IsMouseVisible = true;
                    startMenu.Update(ref mode, middleScreen, gameTime.ElapsedGameTime.TotalSeconds, ref volume, this);
                    if (mode == SelectMode.hittingGame || mode == SelectMode.trackerGame)       //menu -> re-initialize games
                    {
                        trackerGame = new TrackerGame(viewSource, viewDest, cursorRect, screenDim, middleScreen, background, cursor, target, font, sphereAtlas, explosionAtlas,ticking, goButton, onButton, clickButton, gameSong, backgroundStatsInGame);
                        hittingGame = new HittingGame(viewSource, viewDest, cursorRect, screenDim, middleScreen, background, cursor, target, sphereAtlas, explosionAtlas, goButton, glassBreak, onButton, clickButton, gameSong, backgroundStatsInGame);
                        results = new Results(screenDim, GraphicsDevice, exit, menuButtonLarge, mouseMenuPointer, hittingGame, trackerGame, freccia, pentagono, triangolo, font, backgroundResult, help, help_info_stats, onButton, clickButton);
                    }
                    break;
                case SelectMode.trackerGame:
                    IsMouseVisible = false;
                    trackerGame.Update(gameTime,ref mode, mouseSens, volume);
                    break;
                case SelectMode.hittingGame:
                    IsMouseVisible = false;
                    hittingGame.Update(gameTime,ref mode, mouseSens, volume);
                    break;
                case SelectMode.pause:
                    IsMouseVisible = true;
                    pause.Update(ref mode, ref mouseSens, ref volume, prevMode, prevMouse, this);
                    break;
                case SelectMode.results:
                    IsMouseVisible = true;
                    results.Update(ref mode, this);
                    break;
                case SelectMode.exiting:
                    IsMouseVisible = true;
                    exitGame.Update(ref mode, volume, this);
                    break;
            }

            //oldMode = mode;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);   //SpriteSortMode.BackToFront,BlendState.NonPremultiplied
            switch (mode)
            {
                case SelectMode.menu:
                    startMenu.Draw(_spriteBatch);
                    break;
                case SelectMode.trackerGame:
                    trackerGame.Draw(_spriteBatch);
                    break;
                case SelectMode.hittingGame:
                    hittingGame.Draw(_spriteBatch, font, gameTime);
                    break;
                case SelectMode.pause:
                    pause.Draw(_spriteBatch, font);
                    break;
                case SelectMode.results:
                    IsMouseVisible = true;
                    results.Draw(_spriteBatch,font);
                    break;
                case SelectMode.exiting:
                    exitGame.Draw(_spriteBatch);
                    break;
            }            
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void CheckExit()
        {
            exitGame = new Exit(GraphicsDevice, screenDim, mode, dialog, quitButtonExit, backButtonExit, onButton, clickButton);
            mode = SelectMode.exiting;
        }
    }
}
