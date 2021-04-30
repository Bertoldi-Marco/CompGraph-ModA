using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2Dprj
{
    public class Exit
    {
        private int[] backBuffer;
        private Texture2D screenFreezed;
        private Texture2D dialog;
        private Button quitButton;
        private Button backButton;
        private Rectangle quitRect;
        private Rectangle backRect;
        private Rectangle dialogRect;
        private SelectMode prevMode;
        //Mouse
        private MouseState newMouse;
        private MouseState oldMouse;

        public Exit(GraphicsDevice graphicsDevice, Point screenDim, SelectMode prevMode, Texture2D dialog, Texture2D quitButton, Texture2D backButton, SoundEffect onButton, SoundEffect clickButton)
        {
            this.dialog = dialog;
            this.prevMode = prevMode;
            graphicsDevice.GetBackBufferData(backBuffer);
            screenFreezed = new Texture2D(graphicsDevice, screenDim.X, screenDim.Y, false, graphicsDevice.PresentationParameters.BackBufferFormat);
            screenFreezed.SetData(backBuffer);
            dialogRect = new Rectangle((screenDim.X - dialog.Width) / 2, (screenDim.Y - dialog.Height) / 2, dialog.Width, dialog.Height);
            quitRect = new Rectangle(348 + dialogRect.X, 117 + dialogRect.Y, quitButton.Width, quitButton.Height);
            backRect = new Rectangle(50 + dialogRect.X, 117 + dialogRect.Y, backButton.Width, backButton.Height);
            this.quitButton = new Button(quitRect, quitButton, Color.White, onButton, clickButton);
            this.backButton = new Button(backRect, backButton, Color.White, onButton, clickButton);
        }

        public void Update(ref SelectMode mode, float volume, Game1 game)
        {
            oldMouse = newMouse;                            //added oldmouse and newmouse to check click on button
            newMouse = Mouse.GetState();

            if (backButton.IsPressed(newMouse, oldMouse, volume))
            {
                mode = prevMode;
            }

            if (quitButton.IsPressed(newMouse, oldMouse, volume))
            {
                game.Exit();
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(screenFreezed, new Vector2(0, 0), Color.Gray);
            _spriteBatch.Draw(dialog, dialogRect, Color.White);
            quitButton.Draw(_spriteBatch);
            backButton.Draw(_spriteBatch);
        }
    }
}
