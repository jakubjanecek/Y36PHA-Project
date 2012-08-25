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
    public class Hud : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private GraphicsDevice graphics;
        private SpriteBatch spriteBatch;
        private Texture2D controlPanel;
        private Texture2D courseSetter;
        private Texture2D scoreHud;
        private ContentManager content;

        Vector2 panelPosition;
        Vector2 scoreHudPosition;

        SpriteFont fontValues;            // font pro hodnoty
        SpriteFont fontCallSign;          // font pro název letadla (volací znak)

        Texture2D arrowTexture;
        VertexBuffer vertexBuffer;
        VertexDeclaration vertexDeclaration;

        private int wantedDir = 0;
        private int wantedSpeed = 0;
        private int wantedAlt = 0;
        private bool speakApproach = false;

        public int WantedAlt { get { return wantedAlt; } set { wantedAlt = (int)(checkAlt(value) ? value : wantedAlt); } }
        public int WantedSpeed { get { return wantedSpeed; } set { wantedSpeed = (int)(checkSpeed(value) ? value : wantedSpeed); } }
        public int WantedDir { get { return wantedDir; } set { wantedDir = checkDir((double)value) ? value : (int)wantedDir; } }

        public Hud(Game game)
            : base(game)
        {
            graphics = game.GraphicsDevice;
            content = game.Content;
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            this.DrawOrder = 100;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics);
            controlPanel = content.Load<Texture2D>("ControlPanel");
            courseSetter = content.Load<Texture2D>("CourseSetter");
            scoreHud = content.Load<Texture2D>("scorehud");
            fontValues = content.Load<SpriteFont>("HUD14");
            fontCallSign = content.Load<SpriteFont>("HUD20");

            panelPosition = new Vector2(0, graphics.Viewport.Height - controlPanel.Height);
            scoreHudPosition = new Vector2(Main.WIDTH - scoreHud.Width, 0);

            arrowTexture = content.Load<Texture2D>("RedArrow");
            VertexPositionTexture[] vertices = new VertexPositionTexture[12];
            vertices[0] = new VertexPositionTexture(new Vector3(-0.5f, 0, -1.0f), new Vector2(0, 0));
            vertices[2] = new VertexPositionTexture(new Vector3(0.5f, 0, 0), new Vector2(1, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(-0.5f, 0, 0), new Vector2(0, 1));
            vertices[3] = new VertexPositionTexture(new Vector3(-0.5f, 0, -1.0f), new Vector2(0, 0));
            vertices[5] = new VertexPositionTexture(new Vector3(0.5f, 0, -1.0f), new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(new Vector3(0.5f, 0, 0), new Vector2(1, 1));

            vertices[6] = new VertexPositionTexture(new Vector3(0.5f, 0, -1.0f), new Vector2(0, 0));
            vertices[8] = new VertexPositionTexture(new Vector3(-0.5f, 0, 0), new Vector2(1, 1));
            vertices[7] = new VertexPositionTexture(new Vector3(0.5f, 0, 0), new Vector2(0, 1));
            vertices[9] = new VertexPositionTexture(new Vector3(0.5f, 0, -1.0f), new Vector2(0, 0));
            vertices[11] = new VertexPositionTexture(new Vector3(-0.5f, 0, -1.0f), new Vector2(1, 0));
            vertices[10] = new VertexPositionTexture(new Vector3(-0.5f, 0, 0), new Vector2(1, 1));
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
                Plane plane = Main.getInstance().GetSelectedPlane();
                if (plane != null)
                {
                    if (!plane.IsLanding)
                    {
                        if (Main.getInstance().input.ReduceSpeed((int)panelPosition.X + 455, (int)panelPosition.Y + 77))
                        { ReduceSpeed(); }
                        if (Main.getInstance().input.IncreaseSpeed((int)panelPosition.X + 455, (int)panelPosition.Y + 61))
                        { IncreaseSpeed(); }
                    }
                    if (!plane.IsDescendingToRunway)
                    {
                        if (Main.getInstance().input.ReduceAlt((int)panelPosition.X + 455, (int)panelPosition.Y + 136))
                        { ReduceAlt(); }
                        if (Main.getInstance().input.IncreaseAlt((int)panelPosition.X + 455, (int)panelPosition.Y + 120))
                        { IncreaseAlt(); }
                    }
                    if (!plane.IsApproaching)
                    {
                        if (Main.getInstance().input.ReduceDir())
                        { ReduceDir(); }
                        if (Main.getInstance().input.IncreaseDir())
                        { IncreaseDir(); }
                        if (Main.getInstance().input.CourseChange((int)panelPosition.X + 78, (int)panelPosition.Y + 28, 125))
                        {
                            Vector2 relPosition = Main.getInstance().input.GetRelativeCursorPos((int)panelPosition.X + 139, (int)panelPosition.Y + 90);

                            int offset = 0;
                            if (relPosition.Y >= 0)
                            {
                                offset = 180;
                            }
                            else if (relPosition.Y < 0 && relPosition.X < 0)
                            {
                                offset = 360;
                            }
                            wantedDir = offset - (int)MathHelper.ToDegrees((float)Math.Atan(relPosition.X / relPosition.Y));
                        }
                    }
                    if (Main.getInstance().input.Approach((int)panelPosition.X + 574, (int)panelPosition.Y + 57, 690 - 574, 119 - 94))
                    {
                        if (plane.IsApproaching)
                        {
                            plane.cancelApproach();
                            plane.WantedAlt = WantedAlt;
                            plane.WantedSpeed = WantedSpeed;
                        }
                        else
                        {
                            speakApproach = true;
                            Speak(plane);
                            plane.WantedAlt = WantedAlt;
                            plane.WantedSpeed = WantedSpeed;
                            plane.IsApproaching = true;
                            Main.getInstance().SelectPlane(null);
                        }
                    }
                    if (Main.getInstance().input.ConfirmCommand((int)panelPosition.X + 574, (int)panelPosition.Y + 94, 690 - 574, 119 - 94))
                    {
                        Speak(plane);
                        plane.WantedAlt = WantedAlt;
                        plane.WantedDir = WantedDir;
                        plane.WantedSpeed = WantedSpeed;
                        Main.getInstance().SelectPlane(null);
                    }
                    if (Main.getInstance().input.CancelCommand((int)panelPosition.X + 574, (int)panelPosition.Y + 133, 690 - 574, 119 - 94))
                    {
                        Main.getInstance().SelectPlane(null);
                    }

                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Plane plane = Main.getInstance().GetSelectedPlane();

            if (Main.state.CurrentState == GameState.GAME_3D)
            {
                if (plane != null && WantedDir != plane.WantedDir)
                {
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
                    Matrix worldMatrix = Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(MathHelper.ToRadians(-(float)WantedDir)) * Matrix.CreateTranslation(plane.Get3dLocation());
                    Main.getInstance().effect.Parameters["xWorld"].SetValue(worldMatrix);
                    Main.getInstance().effect.Parameters["xView"].SetValue(viewMatrix);
                    Main.getInstance().effect.Parameters["xProjection"].SetValue(projectionMatrix);

                    Main.getInstance().effect.Parameters["xTexture"].SetValue(this.arrowTexture);
                    Main.getInstance().effect.Parameters["xEnableLighting"].SetValue(true);
                    Main.getInstance().effect.Parameters["xAmbient"].SetValue(0.4f);
                    Main.getInstance().effect.Parameters["xLightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

                    GraphicsDevice.RenderState.DepthBufferEnable = true;
                    Main.getInstance().effect.Begin();
                    foreach (EffectPass pass in Main.getInstance().effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        graphics.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionTexture.SizeInBytes);
                        graphics.VertexDeclaration = vertexDeclaration;
                        int noVertices = vertexBuffer.SizeInBytes / VertexPositionTexture.SizeInBytes;
                        graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, noVertices / 3);

                        pass.End();
                    }
                    Main.getInstance().effect.End();
                }
            }

            switch (Main.state.CurrentState)
            {
                case GameState.GAME_2D:
                case GameState.GAME_3D:
                    spriteBatch.Begin();
                    if (plane != null)
                    {
                        spriteBatch.Draw(controlPanel, new Rectangle((int)panelPosition.X, (int)panelPosition.Y, controlPanel.Width, controlPanel.Height), Color.White);
                        spriteBatch.Draw(courseSetter, new Rectangle((int)panelPosition.X + 139, (int)panelPosition.Y + 90, courseSetter.Width, courseSetter.Height), null, Color.White, MathHelper.ToRadians((float)WantedDir), new Vector2(courseSetter.Width / 2, courseSetter.Height / 2), new SpriteEffects(), 1);

                        string text = WantedDir + " deg";
                        Vector2 labelSize = fontValues.MeasureString(text);
                        spriteBatch.DrawString(fontValues, text, new Vector2(panelPosition.X + 190, panelPosition.Y + 10), Color.White);

                        text = WantedSpeed + " kn";
                        labelSize = fontValues.MeasureString(text);
                        spriteBatch.DrawString(fontValues, text, new Vector2(panelPosition.X + 355, panelPosition.Y + 55), Color.White);

                        text = WantedAlt + " fts";
                        labelSize = fontValues.MeasureString(text);
                        spriteBatch.DrawString(fontValues, text, new Vector2(panelPosition.X + 355, panelPosition.Y + 115), Color.White);

                        string type = "";
                        if (plane is Boeing)
                        {
                            type = "Boeing ";
                        }
                        else if (plane is Airbus)
                        {
                            type = "Airbus ";
                        }
                        else if (plane is Cesna)
                        {
                            type = "Cesna ";
                        }
                        text = type + plane.CallSign;
                        labelSize = fontValues.MeasureString(text);
                        spriteBatch.DrawString(fontCallSign, text, new Vector2(panelPosition.X + 485, panelPosition.Y + 10), Color.White);

                        spriteBatch.DrawString(fontValues, "Approach", new Vector2(panelPosition.X + 588, panelPosition.Y + 57), plane.IsApproaching || plane.IsLanding ? Color.DarkBlue : Color.Black);
                    }

                    spriteBatch.Draw(scoreHud, new Rectangle(Main.WIDTH - scoreHud.Width, 0, scoreHud.Width, scoreHud.Height), Color.White);
                    spriteBatch.DrawString(fontValues, Main.state.Level.ToString(), new Vector2(scoreHudPosition.X + (scoreHud.Width / 2), scoreHudPosition.Y + 6), Color.White);
                    spriteBatch.DrawString(fontValues, Main.state.Score.ToString(), new Vector2(scoreHudPosition.X + (scoreHud.Width / 2), scoreHudPosition.Y + 34), Color.White);
                    spriteBatch.DrawString(fontValues, Main.state.Mistakes.ToString(), new Vector2(scoreHudPosition.X + (scoreHud.Width / 2), scoreHudPosition.Y + 62), Color.White);
                    spriteBatch.End();
                    break;
                default:
                    break;
            }
            base.Draw(gameTime);
        }

        private bool checkSpeed(double speed)
        {
            if (speed >= 150 && speed <= 300)
            {
                return true;
            }
            return false;
        }

        private bool checkAlt(double alt)
        {
            if (alt >= 2000 && alt <= 6000)
            {
                return true;
            }
            return false;
        }

        private bool checkDir(double dir)
        {
            if (dir >= 0 && dir < 360)
            {
                return true;
            }
            return false;
        }


        private void ReduceAlt()
        {
            WantedAlt -= 500;

        }

        private void IncreaseAlt()
        {
            WantedAlt += 500;
        }

        private void ReduceSpeed()
        {
            WantedSpeed -= 10;
        }

        private void IncreaseSpeed()
        {
            WantedSpeed += 10;
        }

        private void ReduceDir()
        {
            wantedDir--;
            WantedDir = wantedDir % 360;
            if (wantedDir < 0)
            {
                WantedDir += 360;
            }
        }

        private void IncreaseDir()
        {
            wantedDir++;
            WantedDir = wantedDir % 360;
        }

        private void Speak(Plane plane)
        {
            if (WantedDir != plane.WantedDir || WantedAlt != plane.WantedAlt || WantedSpeed != plane.WantedSpeed || speakApproach)
            {
                Airport landingAirport = Main.getInstance().GetLandingAirport(plane.X, plane.Y);
                int targetAirport = (int)(landingAirport.Dir / 10);
                string targetAirportSide = landingAirport.Side;

                // Hlášení dispečera
                Main.getInstance().sound.playDispSequence(plane.CallSign);
                if (WantedDir != plane.WantedDir && !speakApproach)
                {
                    Main.getInstance().sound.play("disp_course");
                    if (WantedDir < 100)
                    {
                        Main.getInstance().sound.playDispSequence("0" + WantedDir.ToString());
                    }
                    else
                    {
                        Main.getInstance().sound.playDispSequence(WantedDir.ToString());
                    }
                    Main.getInstance().sound.play("space");
                }
                if (WantedSpeed != plane.WantedSpeed)
                {
                    Main.getInstance().sound.play("disp_speed");
                    Main.getInstance().sound.playDispSequence(WantedSpeed.ToString());
                    Main.getInstance().sound.play("disp_knots");
                    Main.getInstance().sound.play("space");
                }
                if (WantedAlt != plane.WantedAlt)
                {
                    if (WantedAlt < plane.Alt)
                    {
                        Main.getInstance().sound.play("disp_descent");
                    }
                    else
                    {
                        Main.getInstance().sound.play("disp_climb");
                    }
                    Main.getInstance().sound.play("space");
                    if (WantedAlt % 1000 == 0)
                    {
                        Main.getInstance().sound.playDispSequence(new String(WantedAlt.ToString()[0], 1));
                        Main.getInstance().sound.play("disp_000");
                    }
                    else
                    {
                        Main.getInstance().sound.playDispSequence(new String(WantedAlt.ToString()[0], 1) + new String(WantedAlt.ToString()[1], 1));
                        Main.getInstance().sound.play("disp_00");
                    }
                    Main.getInstance().sound.play("disp_feets");
                    Main.getInstance().sound.play("space");
                }
                if (speakApproach)
                {
                    Main.getInstance().sound.play("disp_approach");
                    Main.getInstance().sound.playDispSequence("" + targetAirport);
                    if (targetAirportSide != "")
                    {
                        Main.getInstance().sound.play("disp_" + targetAirportSide);
                    }
                }

                // Nastavení zpoždění reakce, aby reagoval až po příkazu dispečera.
                plane.ReactionOffset = Main.getInstance().sound.GetFilledTime();

                // Odpověď pilota
                if (WantedDir != plane.WantedDir && !speakApproach)
                {
                    Main.getInstance().sound.playNonEssential("heading");
                    if (WantedDir < 100)
                    {
                        Main.getInstance().sound.playNonEssentialSequence("0" + WantedDir.ToString());
                    }
                    else
                    {
                        Main.getInstance().sound.playNonEssentialSequence(WantedDir.ToString());
                    }
                    Main.getInstance().sound.playNonEssential("space");
                }
                if (WantedSpeed != plane.WantedSpeed)
                {
                    Main.getInstance().sound.playNonEssential("speed");
                    Main.getInstance().sound.playNonEssentialSequence(WantedSpeed.ToString());
                    Main.getInstance().sound.playNonEssential("space");
                }
                if (WantedAlt != plane.WantedAlt)
                {
                    if (WantedAlt < plane.Alt)
                    {
                        Main.getInstance().sound.playNonEssential("descent");
                    }
                    else
                    {
                        Main.getInstance().sound.playNonEssential("climb");
                    }
                    if (WantedAlt % 1000 == 0)
                    {
                        Main.getInstance().sound.playNonEssentialSequence(new String(WantedAlt.ToString()[0], 1));
                        Main.getInstance().sound.playNonEssential("000");
                    }
                    else
                    {
                        Main.getInstance().sound.playNonEssentialSequence(new String(WantedAlt.ToString()[0], 1) + new String(WantedAlt.ToString()[1], 1));
                        Main.getInstance().sound.playNonEssential("00");
                    }
                    Main.getInstance().sound.playNonEssential("feet");
                    Main.getInstance().sound.playNonEssential("space");
                }
                if (speakApproach)
                {
                    Main.getInstance().sound.playNonEssential("approach");
                    Main.getInstance().sound.playNonEssentialSequence("" + targetAirport);
                    if (targetAirportSide != "")
                    {
                        Main.getInstance().sound.playNonEssential(targetAirportSide);
                    }
                    speakApproach = false;
                }
                Main.getInstance().sound.playNonEssentialSequence(plane.CallSign);
            }
        }

    }
}