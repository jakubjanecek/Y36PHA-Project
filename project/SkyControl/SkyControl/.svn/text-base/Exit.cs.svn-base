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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Exit : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private double x;               // pozice exitu (v mílích)
        private double y;
        private int pixelX;             // pozice exitu (v pixelech)
        private int pixelY;
        private double dir;             // směr příletu
        private double trafficLoad = 1;  // modifikátor hustoty provozu

        private static int seed = 1;
        private Random random = new Random(seed++);

        private GraphicsDevice graphics;
        private SpriteBatch spriteBatch;
        private Texture2D exitSymbol;
        private ContentManager content;

        private Plane lastPlane6k = null;      // kontrolní ukazatele na naposledy vygenerované letadla (aby se mohlo hlídat dodržení vzdálenosti při vygenerování)
        private Plane lastPlane5k = null;
        private Plane lastPlane4k = null;
        private double planeGenerationDelay;

        public Exit(Game game, double posX, double posY, double dir, double trafficLoad)
            : base(game)
        {
            graphics = game.GraphicsDevice;
            content = game.Content;
            this.trafficLoad = trafficLoad;
            planeGenerationDelay = random.Next((int)(3 / trafficLoad), (int)(41 / trafficLoad));
            this.x = posX;
            this.y = posY;
            this.dir = dir;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics);
            exitSymbol = content.Load<Texture2D>("Exit");
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Main.state.CurrentState == GameState.GAME_2D || Main.state.CurrentState == GameState.GAME_3D)
            {
                generatePlane(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            switch (Main.state.CurrentState)
            {
                case GameState.GAME_2D:
                    // Skutečnou pozici přepočítáme na pozici v pixelech
                    pixelX = (int)(x * graphics.Viewport.Width / Main.sectorSize.X);
                    pixelY = (int)(y * graphics.Viewport.Height / Main.sectorSize.Y);

                    // Vykreslíme exit
                    spriteBatch.Begin();
                    spriteBatch.Draw(exitSymbol, new Rectangle(pixelX, pixelY, exitSymbol.Width, exitSymbol.Height), null, Color.White, MathHelper.ToRadians((float)dir), new Vector2(exitSymbol.Width / 2, exitSymbol.Height), new SpriteEffects(), 1);
                    spriteBatch.End();

                    break;
                case GameState.GAME_3D:
                    break;
                default:
                    break;
            }
            base.Draw(gameTime);
        }

        private void generatePlane(GameTime gameTime)
        {
            double dist1 = lastPlane6k != null ? Main.computeDistance(x, y, lastPlane6k.X, lastPlane6k.Y) : 4;
            double dist2 = lastPlane5k != null ? Main.computeDistance(x, y, lastPlane5k.X, lastPlane5k.Y) : 4;
            double dist3 = lastPlane4k != null ? Main.computeDistance(x, y, lastPlane4k.X, lastPlane4k.Y) : 4;
            bool dist = (dist1 > 3.0 && dist2 > 3.0 && dist3 > 3.0);
            if (dist)
            {
                planeGenerationDelay -= ((double)gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            if (dist && planeGenerationDelay <= 0)
            {
                int rand = random.Next(0, 1000);
                if (rand < 51)
                {
                    Random rand1 = new Random();
                    if (rand < 15)
                    {
                        // GENERATE PLANE in 6000 feet
                        int speed = random.Next(260, 301);
                        speed -= (speed % 10);
                        lastPlane6k = new Boeing(this.Game, x, y, 6000, speed, dir, GenerateBoeingCallSign(), rand1.Next(800, 1500));
                        Main.getInstance().AddPlane(lastPlane6k);
                    }
                    else if (rand >= 15 && rand < 45)
                    {
                        // GENERATE PLANE in 5000 feet
                        int speed = random.Next(240, 301);
                        speed -= (speed % 10);
                        lastPlane5k = new Airbus(this.Game, x, y, 5000, speed, dir, GenerateAirbusCallSign(), rand1.Next(800, 1500));
                        Main.getInstance().AddPlane(lastPlane5k);
                    }
                    else
                    {
                        // GENERATE PLANE in 4000 feet
                        int speed = random.Next(220, 271);
                        speed -= (speed % 10);
                        lastPlane4k = new Cesna(this.Game, x, y, 4000, speed, dir, GenerateCesnaCallSign(), rand1.Next(800, 1500));
                        Main.getInstance().AddPlane(lastPlane4k);
                    }
                    planeGenerationDelay = random.Next((int)(10 / trafficLoad), (int)(41 / trafficLoad)); // random.NextDouble() * 10 * random.Next(1, 11);
                }
            }
        }

        public string GenerateBoeingCallSign()
        {
            string[] signs = { "OCN", "BRI" };
            return signs[random.Next(signs.Length)] + " " + random.Next(10) + random.Next(10) + random.Next(10);
        }

        public string GenerateAirbusCallSign()
        {
            string[] signs = { "CSA", "DEL", "BRI" };
            return signs[random.Next(signs.Length)] + " " + random.Next(10) + random.Next(10) + random.Next(10);
        }

        public string GenerateCesnaCallSign()
        {
            string[] signs = { "ATL", "CSA", "DEL" };
            return signs[random.Next(signs.Length)] + " " + random.Next(10) + random.Next(10) + random.Next(10);
        }
    }
}