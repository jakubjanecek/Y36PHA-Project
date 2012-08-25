using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SkyControl
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Intro : Microsoft.Xna.Framework.DrawableGameComponent
    {
        GraphicsDevice graphics;
        ContentManager content;
        SpriteBatch spriteBatch;

        Texture2D background;           // textura pozadí
        Rectangle window;               // čtverec o rozměrech okna
        Color shadeFilter;              // barva ztmavovacího filtru
        const int shadingTime = 2000;   // kolik milisekund má ztmavování trvat

        public Intro(Game game)
            : base(game)
        {
            this.graphics = game.GraphicsDevice;
            content = game.Content;
            window = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            shadeFilter = new Color(0, 0, 0);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics);
            background = content.Load<Texture2D>("Main screen");
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.state.CurrentState == GameState.INTRO)
            {
                // Zjistíme, kolik času uběhlo od posledního updatu a podle toho nastavíme změnu zastínění
                int shadeChange = gameTime.ElapsedGameTime.Milliseconds * 255 / shadingTime;
                if (shadeFilter.R < 255)
                {
                    // Hodnota barvy může být maximálně 255, takže případný přebytek ořežeme
                    if (shadeChange > 255 - shadeFilter.R)
                    {
                        shadeChange = 255 - shadeFilter.R;
                    }
                    // A konečně změníme hodnotu zastínění
                    shadeFilter.R = shadeFilter.G = shadeFilter.B += (byte)shadeChange;
                }

                // Pokud se dokončila "animace", spusť menu a ukonči vlákno Intra
                if (shadeFilter.R == 255)
                {
                    Main.state.CurrentState = GameState.MENU_MAIN;
                }
            }

            base.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            if (Main.state.CurrentState == GameState.INTRO)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(background, window, shadeFilter);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
