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
    public class Airport : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private double x;               // pozice paty dráhy v mílích
        private double y;
        private int pixelX;             // pozice paty dráhy v pixelech
        private int pixelY;
        private double length;          // délka dráhy v mílích
        private double dir;             // směr dráhy ve stupních
        private string side = "";
        private bool working = true;
        private int delay = 0;

        private GraphicsDevice graphics;
        private SpriteBatch spriteBatch;
        private ContentManager content;

        private SpriteFont font;
        private Vector2 signSize = new Vector2(0, 0);

        private Texture2D runway2M5Symbol;
        private Texture2D runway2M0Symbol;
        private Texture2D runway1M5Symbol;
        Texture2D texture;
        VertexBuffer vertexBuffer;
        VertexDeclaration vertexDeclaration;

        private Random rand;

        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public double Length { get { return length; } set { length = value; } }
        public double Dir { get { return dir; } set { dir = value; } }
        public string Side { get { return side; } set { side = (value == "left" || value == "right" ? value : side); } }
        public bool Working { get { return working; } }

        public Airport(Game game, double posX, double posY, double length, double dir)
            : base(game)
        {
            graphics = game.GraphicsDevice;
            content = game.Content;
            X = posX;
            Y = posY;
            Length = length;
            Dir = dir;
            rand = new Random((int)(x + y));
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

            font = content.Load<SpriteFont>("CallSign");
            signSize = font.MeasureString("" + (int)dir / 10 + (side == "left" ? "L" : (side == "right") ? "R" : ""));

            runway1M5Symbol = content.Load<Texture2D>("Runway1m5");
            runway2M0Symbol = content.Load<Texture2D>("Runway2m");
            runway2M5Symbol = content.Load<Texture2D>("Runway2m5");
            texture = content.Load<Texture2D>("RunwayTexture");

            VertexPositionTexture[] vertices = new VertexPositionTexture[6];

            vertices[0] = new VertexPositionTexture(new Vector3(-0.06f * (float)length / 2.5f, 0, (float)length), new Vector2(0, 1));
            vertices[2] = new VertexPositionTexture(new Vector3(0.06f * (float)length / 2.5f, 0, 0), new Vector2(1, 0));
            vertices[1] = new VertexPositionTexture(new Vector3(-0.06f * (float)length / 2.5f, 0, 0), new Vector2(0, 0));

            vertices[3] = new VertexPositionTexture(new Vector3(-0.06f * (float)length / 2.5f, 0, (float)length), new Vector2(0, 1));
            vertices[5] = new VertexPositionTexture(new Vector3(0.06f * (float)length / 2.5f, 0, (float)length), new Vector2(1, 1));
            vertices[4] = new VertexPositionTexture(new Vector3(0.06f * (float)length / 2.5f, 0, 0), new Vector2(1, 0));

            vertexBuffer = new VertexBuffer(graphics, vertices.Length * VertexPositionTexture.SizeInBytes, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            vertexDeclaration = new VertexDeclaration(graphics, VertexPositionTexture.VertexElements);

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
                if (delay <= 0)
                {
                    EnableRunway();

                    if (rand.Next(0, 1000) <= 10)
                    {
                        DisableRunway();
                    }
                }
                else
                {
                    delay -= gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            switch (Main.state.CurrentState)
            {
                case GameState.GAME_2D:
                    // Skutečnou pozici přepočítáme na pozici v pixelech
                    pixelX = (int)(X * graphics.Viewport.Width / Main.sectorSize.X);
                    pixelY = (int)(Y * graphics.Viewport.Height / Main.sectorSize.Y);
                    // runway1M5Symbol.Width = 30 + length
                    // length/sectorSizeX = width/screenWidth
                    // width = length/sectorSizeX * screenWidth
                    int pixelWidth = (int)(graphics.Viewport.Width /* *(1 míle) */ / Main.sectorSize.X);
                    int pixelHeight = (int)(graphics.Viewport.Height * (30 + length) / Main.sectorSize.Y);

                    // Pata dráhy je jinde, než začátek textury, takže nastavíme offset, aby na pozici X, Y byla pata dráhy
                    Texture2D texture;
                    Vector2 runwayStartOffset;
                    if (length < 1.75) { texture = runway1M5Symbol; runwayStartOffset = new Vector2(16, 47); }
                    else if (length < 2.25) { texture = runway2M0Symbol; runwayStartOffset = new Vector2(16, 61); }
                    else { texture = runway2M5Symbol; runwayStartOffset = new Vector2(16, 78); }

                    // Vykreslíme dráhu
                    spriteBatch.Begin();
                    spriteBatch.Draw(texture, new Rectangle(pixelX, pixelY, pixelWidth, pixelHeight), null, working ? Color.White : Color.Red, MathHelper.ToRadians((float)Dir), runwayStartOffset, new SpriteEffects(), 1);
                    spriteBatch.End();
                    break;
                case GameState.GAME_3D:
                    Matrix viewMatrix = Matrix.CreateLookAt(
                            Main.getInstance().cameraPosition,
                            Main.getInstance().cameraLookAt,
                            Vector3.Up);
                    Matrix projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f),
                        graphics.Viewport.AspectRatio,
                        0.1f,
                        100.0f);

                    Main.getInstance().effect.CurrentTechnique = Main.getInstance().effect.Techniques["Textured"];
                    Matrix worldMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(180 - (float)dir)) * Matrix.CreateTranslation((float)X, 0, (float)Y);
                    Main.getInstance().effect.Parameters["xWorld"].SetValue(worldMatrix);
                    Main.getInstance().effect.Parameters["xView"].SetValue(viewMatrix);
                    Main.getInstance().effect.Parameters["xProjection"].SetValue(projectionMatrix);

                    Main.getInstance().effect.Parameters["xTexture"].SetValue(this.texture);
                    Main.getInstance().effect.Parameters["xEnableLighting"].SetValue(true);
                    Main.getInstance().effect.Parameters["xAmbient"].SetValue(0.4f);
                    Main.getInstance().effect.Parameters["xLightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

                    Main.getInstance().effect.Begin();
                    foreach (EffectPass pass in Main.getInstance().effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        graphics.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionTexture.SizeInBytes);
                        //vertexDeclaration = new VertexDeclaration(graphics, VertexPositionTexture.VertexElements);
                        graphics.VertexDeclaration = vertexDeclaration;
                        int noVertices = vertexBuffer.SizeInBytes / VertexPositionTexture.SizeInBytes;
                        graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, noVertices / 3);

                        Vector2 positionOnScreen = Main.getInstance().Transfer3to2d(new Vector3((float)X, 0, (float)Y));
                        spriteBatch.Begin();
                        Color color;
                        color = this.working ? Color.White : Color.Red;
                        spriteBatch.DrawString(font, "" + (int)dir / 10 + (side == "left" ? "L" : (side == "right") ? "R" : ""), new Vector2(positionOnScreen.X - (signSize.X / 2), positionOnScreen.Y - signSize.Y), color);
                        spriteBatch.End();

                        pass.End();
                    }
                    Main.getInstance().effect.End();
                    break;
                default:
                    break;
            }
            base.Draw(gameTime);
        }

        private void speak(bool before, bool now)
        {
            if (before != now)
            {
                Main.getInstance().sound.play("t_runway");
                Main.getInstance().sound.playTowerSequence("" + (int)(this.dir/10));
                if (this.side != "")
                {
                    Main.getInstance().sound.play("t_" + this.side);
                }
                if (now)
                {
                    Main.getInstance().sound.play("t_runwayback");
                }
                else
                {
                    Main.getInstance().sound.play("t_runwayout");
                }
            }
        }


        /********************************************************
         * Postaví runway mimo provoz
         * ******************************************************/
        private void DisableRunway()
        {
            speak(working, false);
            working = false;
            delay = rand.Next(60000, 600000);
        }

        /********************************************************
         * Znovu zprovozní runway
         * ******************************************************/
        private void EnableRunway()
        {
            speak(working, true);
            working = true;
            delay = rand.Next(15000, 60000);
        }

        /********************************************************
         * Vrátí směr osy (tj. obrácený směr než pro přistání)
         * ******************************************************/
        public float GetAxisDirection()
        {
            return dir >= 180.0 ? (float)dir - 180 : (float)dir + 180;
        }


        /********************************************************
         * Vrátí úhel od osy letiště, pod kterým se nachází zadaný
         * bod
         * ******************************************************/
        public float GetAngleFromAxis(float pointX, float pointY)
        {
            return Main.GetAngleBetween(GetAxisDirection(), Main.getCourse((float)this.X, (float)this.Y, pointX, pointY));
        }


        /********************************************************
         * Vrátí informaci o vzdálenosti bodu na ose letiště.
         * Bod může být i mimo osu. Pak vrací vzdálenost jeho
         * promítnutého obrazu na ose.
         * ******************************************************/
        public float GetDistanceOnAxis(float pointX, float pointY)
        {
            float angleFromAxis = GetAngleFromAxis(pointX, pointY);
            return (float)Main.GetDistance((float)this.X, (float)this.Y, pointX, pointY) * (float)Math.Cos(MathHelper.ToRadians(angleFromAxis));
        }

        public float GetDistanceOnAxis(Vector2 point)
        {
            return GetDistanceOnAxis(point.X, point.Y);
        }



        /********************************************************
         * Vrátí souřadnice bodu ležící na ose letiště.
         * 
         * distance - vzdálenost bodu od paty letiště
         * ******************************************************/
        public Vector2 GetPointOnAxis(float distance)
        {
            double relPosX = Math.Sin(MathHelper.ToRadians(GetAxisDirection())) * distance;
            double relPosY = -Math.Cos(MathHelper.ToRadians(GetAxisDirection())) * distance;

            return new Vector2((float)(relPosX + this.X), (float)(relPosY + this.Y));
        }

        /********************************************************
         * Přepne stav runwaye (working <-> out of service)
         * ******************************************************/
        public void cheat()
        {
            if (working)
            {
                DisableRunway();
            }
            else
            {
                EnableRunway();
            }
        }
    }
}