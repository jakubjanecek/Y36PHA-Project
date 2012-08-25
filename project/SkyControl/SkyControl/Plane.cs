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
    public class Plane : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private double x;              // pozice v mílích
        private double y;
        private int pixelX;              // pozice v pixelech
        private int pixelY;
        private double alt;            // výška ve stopách
        private int wantedAlt;
        private int displayAlt;
        private double speed;          // rychlost v uzlech
        private int wantedSpeed;
        private int displaySpeed;
        private double dir;         // kurs ve stupních
        private int wantedDir;
        private float roll;         // náklon letadla
        private float pitch;        // sklon letadla

        private string callSign;
        private double fuel;
        private bool colliding = false;
        private int radarRefreshTimer = 0;  // odpočet refreshe radaru (milisekundy)
        private bool isApproaching = false;
        private bool isDescendingToRunway = false;
        private bool isLanding = false;
        private Airport landingAirport = null;
        private bool landed = false;
        private bool falling = false;
        private bool crashed = false;
        private bool fell = false;
        private bool lowFuel = false;

        private bool _10minFuelReported = false;
        private bool _5minFuelReported = false;
        private bool fallReported = false;
        private bool landReported = false;

        private long timeFromAppearance = 0;
        private bool alwaysFarEnough = true;

        private double dirChange = 5.0; // stupne za sekundu
        private double altChange = 100.0; // stopy za sekundu
        private double speedChange = 10.0;  // uzly za sekundu
        protected double fuelChange = 1.0; // litry za sekundu

        protected double reactionTime = 0.0;
        private double reactionOffset = 0.0;

        private double speedWaitingTime = 0.0;
        private bool speedChangeFinished = true;
        private double altWaitingTime = 0.0;
        private bool altChangeFinished = true;
        private double dirWaitingTime = 0.0;
        private bool dirChangeFinished = true;

        private Vector2[] tracePoints;      // trasové body za letadlem

        private GraphicsDevice graphics;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private ContentManager content;
        private SpriteFont font;

        private Vector2 signSize = new Vector2(0, 0);
        private Vector2 infoSize = new Vector2(0, 0);

        private double navX = -1;
        private double navY = -1;

        public double X { get { return x; } set { x = value; } }
        public double Y { get { return y; } set { y = value; } }
        public int PixelX { get { return pixelX; } }
        public int PixelY { get { return pixelY; } }
        public double Alt { get { return alt; } set { alt = value; } }
        public int WantedAlt { get { return wantedAlt; } set { wantedAlt = (int)(checkAlt(value) ? value : alt); } }
        public double Speed { get { return speed; } set { speed = value; } }
        public int WantedSpeed { get { return wantedSpeed; } set { wantedSpeed = (int)(checkSpeed(value) ? value : speed); } }
        public double Dir { get { return dir; } set { dir = checkDir(value) ? value : dir; } }
        public int WantedDir { get { return wantedDir; } set { wantedDir = checkDir((double)value) ? value : (int)dir; } }
        public float Roll { get { return roll; } set { roll = value; } }
        public float Pitch { get { return pitch; } set { pitch = value; } }
        public double ReactionOffset { get { return reactionOffset; } set { reactionOffset = value; } }
        public string CallSign { get { return callSign; } set { callSign = value; signSize = font.MeasureString(CallSign); } }
        public double Fuel { get { return fuel; } set { fuel = value < fuel ? value : fuel; } }
        public bool Colliding { get { return colliding; } set { colliding = value; } }
        public bool IsApproaching { get { return isApproaching; } set { isApproaching = value; if (!isApproaching) { isDescendingToRunway = isLanding = false; } } }
        public bool IsDescendingToRunway { get { return isDescendingToRunway; } set { isDescendingToRunway = value; } }
        public bool IsLanding { get { return isLanding; } set { isLanding = value; } }
        public bool Landed { get { return landed; } }
        public long TimeFromAppearance { get { return timeFromAppearance; } }
        public bool AlwaysFarEnough { get { return alwaysFarEnough; } set { alwaysFarEnough = value; } }
        public bool Falling { get { return falling; } }
        public bool Crashed { get { return crashed; } }
        public bool Fell { get { return fell; } }
        public Airport LandingAirport { get { return landingAirport; } }

        public Plane(Game game, double x, double y, int alt, int speed, double dir, string callSign, double fuel)
            : base(game)
        {
            if (!checkSpeed(speed) || !checkAlt(alt) || !checkDir(dir))
            {
                throw new ArgumentException();
            }
            graphics = game.GraphicsDevice;
            content = game.Content;
            X = x;
            Y = y;
            Alt = alt;
            WantedAlt = alt;
            Speed = speed;
            WantedSpeed = speed;
            Dir = dir;
            WantedDir = (int)dir;
            this.callSign = callSign;
            this.fuel = fuel;
            tracePoints = new Vector2[5];
            for (int i = 0; i < 5; i++)
            {
                tracePoints[i].X = tracePoints[i].Y = -1;
            }
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
            texture = content.Load<Texture2D>("white");
            font = content.Load<SpriteFont>("CallSign");
            signSize = font.MeasureString(CallSign);
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
                timeFromAppearance += gameTime.ElapsedGameTime.Milliseconds;
                if (Falling)
                {
                    fall();
                }
                if (IsApproaching)
                {
                    Airport a = Main.getInstance().GetLandingAirport(this.X, this.Y);
                    if (landingAirport == null)
                    {
                        landingAirport = a;
                    }
                    approach(landingAirport);
                }
                else
                {
                    navX = -1;
                    navY = -1;
                }

                changeSpeed(gameTime);
                changeDir(gameTime);
                changeAlt(gameTime);
                changeFuel(gameTime);

                if (fuel <= 0 && !fallReported)
                {
                    fallReported = true;
                    Main.getInstance().sound.play("mayday");
                    // Main.getInstance().sound.play("space");
                    // Main.getInstance().sound.playSequence(CallSign);
                }
                if (fuel < fuelChange * 300 && !_5minFuelReported)
                {
                    _10minFuelReported = _5minFuelReported = true;
                    Main.getInstance().sound.playSequence(CallSign);
                    Main.getInstance().sound.play("space");
                    Main.getInstance().sound.play("fuel5");
                    //lowFuel = true;   // <- už není potřeba, stačí používat _5minFuelReported a _10minFuelReported
                }
                else if (fuel < fuelChange * 600 && !_10minFuelReported)
                {
                    _10minFuelReported = true;
                    Main.getInstance().sound.playSequence(CallSign);
                    Main.getInstance().sound.play("space");
                    Main.getInstance().sound.play("fuel10");
                }


                /*
                 * X = X[mil] + speed[knts = 1.151 mil/h] * time[ms]
                 * X = X[mil] + (1.151*speed)[mil/h] * time[ms]
                 * X = X[mil] + (1.151*speed/60)[mil/min] * time[ms]
                 * X = X[mil] + (1.151*speed/3600)[mil/s] * time[ms]
                 * X = X[mil] + (1.151*speed/3600000)[mil/ms] * time[ms]
                 * */
                double speedX = 1.151 * Math.Sin(MathHelper.ToRadians((float)Dir)) * Speed / 3600000;
                double speedY = 1.151 * Math.Cos(MathHelper.ToRadians((float)Dir)) * Speed * (-1) / 3600000;
                X += speedX * gameTime.ElapsedGameTime.Milliseconds;
                Y += speedY * gameTime.ElapsedGameTime.Milliseconds;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Skutečnou pozici přepočítáme na pozici v pixelech, ale jenom každé 3 vteřiny
            if (radarRefreshTimer <= 0)
            {
                StoreTracePoint(pixelX, pixelY);
                pixelX = (int)(X * graphics.Viewport.Width / Main.sectorSize.X);
                pixelY = (int)(Y * graphics.Viewport.Height / Main.sectorSize.Y);
                displayAlt = (int)(alt / 100);
                displaySpeed = (int)(speed / 10);
                radarRefreshTimer = 2000;
            }
            else
            {
                radarRefreshTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            switch (Main.state.CurrentState)
            {
                case GameState.GAME_2D:
                    spriteBatch.Begin();
                    Color color;
                    color = _10minFuelReported ? Color.Yellow : Color.White;
                    color = _5minFuelReported ? Color.Orange : color;
                    color = colliding || falling ? Color.Red : color;
                    spriteBatch.Draw(texture, new Rectangle(pixelX - 2, pixelY - 2, 5, 5), color);
                    spriteBatch.DrawString(font, CallSign, new Vector2(pixelX - (signSize.X / 2), pixelY + signSize.Y / 6), color);
                    infoSize = font.MeasureString(displayAlt + "      " + displaySpeed);
                    spriteBatch.DrawString(font, displayAlt + "      " + displaySpeed, new Vector2(pixelX - (infoSize.X / 2), pixelY + infoSize.Y), color);

                    if (Main.settings.TracePoints)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            spriteBatch.Draw(texture, new Rectangle((int)tracePoints[i].X - 1, (int)tracePoints[i].Y - 1, 2, 2), color);
                        }
                    }

                    spriteBatch.End();
                    break;
                case GameState.GAME_3D:
                    Main.getInstance().effect.CurrentTechnique = Main.getInstance().effect.Techniques["Textured"];
                    Main.getInstance().effect.Begin();
                    foreach (EffectPass pass in Main.getInstance().effect.CurrentTechnique.Passes)
                    {
                        pass.Begin();

                        GraphicsDevice.RenderState.DepthBufferEnable = true;
                        if (this is Boeing)
                        {
                            Model model = Main.getInstance().Boeing;
                            if (callSign.StartsWith("ATL")) model = Main.getInstance().Boeing_ATL;
                            else if (callSign.StartsWith("BRI")) model = Main.getInstance().Boeing_BRI;
                            else if (callSign.StartsWith("CSA")) model = Main.getInstance().Boeing_CSA;
                            else if (callSign.StartsWith("DEL")) model = Main.getInstance().Boeing_DEL;
                            else if (callSign.StartsWith("OCN")) model = Main.getInstance().Boeing_OCN;
                            Main.getInstance().DrawModel(model, Get3dLocation(), -(float)dir, pitch, -roll, 0.0005f, new Vector3(0.01f, 0.005f, 0), Matrix.CreateRotationY(MathHelper.ToRadians(180)));
                        }
                        else if (this is Airbus)
                        {
                            Model model = Main.getInstance().Airbus;
                            if (callSign.StartsWith("ATL")) model = Main.getInstance().Airbus_ATL;
                            else if (callSign.StartsWith("BRI")) model = Main.getInstance().Airbus_BRI;
                            else if (callSign.StartsWith("CSA")) model = Main.getInstance().Airbus_CSA;
                            else if (callSign.StartsWith("DEL")) model = Main.getInstance().Airbus_DEL;
                            //else if (callSign.StartsWith("OCN")) model = Main.getInstance().Boeing_OCN;
                            Main.getInstance().DrawModel(model, Get3dLocation(), -(float)dir, pitch, -roll, 0.00002f, new Vector3(0, -0.01f, 0.06f), Matrix.CreateRotationY(MathHelper.ToRadians(180)));
                        }
                        else if (this is Cesna)
                        {
                            Main.getInstance().DrawModel(Main.getInstance().Cesna, Get3dLocation(), -(float)dir, pitch, -roll, 0.000005f, new Vector3(0, 0, 0), Matrix.CreateRotationZ(MathHelper.ToRadians(-5)) * Matrix.CreateRotationY(MathHelper.ToRadians(90)));
                        }

                        Vector2 positionOnScreen = Main.getInstance().Transfer3to2d(Get3dLocation());
                        spriteBatch.Begin();
                        color = _10minFuelReported ? Color.Yellow : Color.White;
                        color = _5minFuelReported ? Color.Orange : color;
                        color = colliding || falling ? Color.Red : color;
                        spriteBatch.DrawString(font, CallSign, new Vector2(positionOnScreen.X - (signSize.X / 2), positionOnScreen.Y - 2 * signSize.Y), color);
                        infoSize = font.MeasureString(displayAlt + "      " + displaySpeed);
                        spriteBatch.DrawString(font, displayAlt + "      " + displaySpeed, new Vector2(positionOnScreen.X - (infoSize.X / 2), positionOnScreen.Y - infoSize.Y), color);
                        if (Main.getInstance().GetFocusPlane() != null)
                        {
                            Vector2 selectedPlaneCoordinates = new Vector2((float)Main.getInstance().GetFocusPlane().X, (float)Main.getInstance().GetFocusPlane().Y);
                            Vector2 thisPlaneCoordinates = new Vector2((float)this.X, (float)this.Y);
                            spriteBatch.DrawString(font, "" + 0.1 * (float)(int)(10 * Main.GetDistance(thisPlaneCoordinates, selectedPlaneCoordinates)), new Vector2(positionOnScreen.X - (signSize.X / 2) - 25, positionOnScreen.Y - 2 * signSize.Y), Color.Yellow);
                        }
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

        /************************************************************
         * Vrátí souřadnice letadla pro 3D zobrazení
         * **********************************************************/
        public Vector3 Get3dLocation()
        {
            return new Vector3((float)X, (float)Alt / 5280 * 2 + 0.007f, (float)Y);
        }


        /************************************************************
         * Provádí instrukce autopilota při přiblížení na letiště
         * **********************************************************/
        public void approach(Airport airport)
        {
            float distanceOnAxis = airport.GetDistanceOnAxis((float)this.X, (float)this.Y);
            if (distanceOnAxis < 2.0 && !isLanding)
            {
                cancelApproach();
            }
            else
            {
                Vector2 navigationPoint = distanceOnAxis > 12 ? airport.GetPointOnAxis(10) : airport.GetPointOnAxis(distanceOnAxis - 2);
                navX = navigationPoint.X;
                navY = navigationPoint.Y;
                WantedDir = Main.getCourse((float)this.X, (float)this.Y, navigationPoint.X, navigationPoint.Y);

                // sestupová fáze
                if (distanceOnAxis <= 10.0)
                {
                    if (airport.GetAngleFromAxis((float)this.X, (float)this.Y) <= 2.5)
                    {
                        IsDescendingToRunway = true;
                    }
                    if (IsDescendingToRunway)     // Tento a ten horní IF musí zůstat oddělené, jinak nefunguje správně sestup na výšku 0
                    {
                        int newWantedAlt = (int)MathHelper.Lerp(3000, 0, 1.0f - distanceOnAxis / 10.0f);
                        wantedAlt = newWantedAlt < wantedAlt ? newWantedAlt : wantedAlt;
                        if (wantedAlt < 0)
                        {
                            wantedAlt = 0;
                        }
                    }
                }
                if (distanceOnAxis <= 5.0)
                {
                    if (airport.GetAngleFromAxis((float)this.X, (float)this.Y) <= 2.5 && CheckLandingAvailability(airport))
                    {
                        IsLanding = true;
                    }
                    if (IsLanding)  // Tento a ten horní IF musí zůstat oddělené!
                    {
                        int newWantedSpeed = (int)MathHelper.Lerp(200, 130, 1.0f - distanceOnAxis / 5.0f);
                        wantedSpeed = newWantedSpeed < wantedSpeed ? newWantedSpeed : wantedSpeed;
                        if (wantedSpeed < 130)
                        {
                            wantedSpeed = 130;
                        }
                    }
                }
                // Runway touch
                if (!this.falling && Alt == 0)
                {
                    wantedSpeed = (int)MathHelper.Lerp(130, 100, distanceOnAxis == 0 ? 0 : -distanceOnAxis / (float)airport.Length);
                }
                // Landing finished
                if (-distanceOnAxis > 0.5 * airport.Length)
                {
                    if (isLanding && Alt == 0)
                    {
                        landed = true;
                    }
                    else
                    {
                        cancelApproach();
                    }
                }
            }
        }

        /***********************************************
         * Zkontroluje vhodnost letiště pro letadlo.
         * Pokud je letadlo nevhodné, zruší approach.
         * *********************************************/
        private bool CheckLandingAvailability(Airport landingAirport)
        {
            int targetAirport = (int)(landingAirport.Dir / 10);
            string targetAirportSide = landingAirport.Side;
            if (!landReported)
            {
                Main.getInstance().sound.playSequence(CallSign);
                Main.getInstance().sound.play("request");
                Main.getInstance().sound.playSequence("" + targetAirport);
                if (targetAirportSide != "")
                {
                    Main.getInstance().sound.play(targetAirportSide);
                }
                Main.getInstance().sound.playTowerSequence(this.CallSign);
            }
            if (!landingAirport.Working && landingAirport.GetDistanceOnAxis((float)this.X, (float)this.Y) > 4.5)
            {
                //Main.getInstance().sound.playTowerSequence(CallSign);
                Main.getInstance().sound.play("t_cancel");
                ReactionOffset = Main.getInstance().sound.GetFilledTime();
                //Main.getInstance().sound.playSequence(CallSign);
                //Main.getInstance().sound.play("around");
                cancelApproach();
                return false;
            }
            if ((this is Boeing && landingAirport.Length < 2.5) || (this is Airbus && landingAirport.Length < 2.0))
            {
                //Main.getInstance().sound.playTowerSequence(CallSign);
                Main.getInstance().sound.play("t_cancel");
                Main.getInstance().sound.play("t_toobig");
                Main.getInstance().sound.playTowerSequence("" + targetAirport);
                if (targetAirportSide != "")
                {
                    Main.getInstance().sound.play("t_" + targetAirportSide);
                }
                ReactionOffset = Main.getInstance().sound.GetFilledTime();
                //Main.getInstance().sound.playSequence(CallSign);
                //Main.getInstance().sound.play("around");
                cancelApproach();
                return false;
            }

            if (!landReported)
            {
                Main.getInstance().sound.play("t_cleared");
                landReported = true;
            }
            return true;
        }

        /***********************************************
         * Zruší přiblížení na letiště a provede potřebná
         * doprovodná nastavení
         * *********************************************/
        public void cancelApproach()
        {
            Main.getInstance().sound.playSequence(CallSign);
            Main.getInstance().sound.play("around");
            this.isApproaching = this.isDescendingToRunway = this.isLanding = false;
            landingAirport = null;
            landReported = false;
            if (speed < 200)
            {
                wantedSpeed = 200;
            }
            if (alt < 3000)
            {
                wantedAlt = 3000;
            }
        }

        public void crash()
        {
            //speed = wantedSpeed = 0;
            //alt = wantedAlt = 0;
            //fell = true;
            if (!this.crashed)
            {
                fall();
                Main.getInstance().sound.play("mayday");
            }
            this.crashed = true;
        }

        public void fall()
        {
            falling = true;
            int newWantedSpeed = 60;//(int)MathHelper.Lerp(300, 10, 4);
            /*if (newWantedSpeed < 60)
            {
                newWantedSpeed = 60;
            }*/
            int newWantedAlt = (int)MathHelper.Lerp(6000, 0, (float)4 * (1 - (float)(speed / 300)));
            wantedSpeed = newWantedSpeed < wantedSpeed ? newWantedSpeed : wantedSpeed;
            wantedAlt = newWantedAlt < wantedAlt ? newWantedAlt : wantedAlt;
            if (Alt <= 0)
            {
                fell = true;
            }
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

        private void changeSpeed(GameTime gameTime)
        {
            if (Speed != WantedSpeed && speedChangeFinished)
            {
                speedChangeFinished = false;
                speedWaitingTime = reactionTime;
            }
            if (Speed != WantedSpeed && !speedChangeFinished)
            {
                if (reactionOffset > 0)
                {
                    reactionOffset -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else if (speedWaitingTime <= 0)
                {
                    int sign = 1;
                    if (WantedSpeed < Speed)
                    {
                        sign = -1;
                    }
                    Speed += sign * speedChange * gameTime.ElapsedGameTime.Milliseconds / 1000;
                    if ((Speed > WantedSpeed && sign == 1) || (Speed < WantedSpeed && sign == -1))
                    {
                        Speed = WantedSpeed;
                    }
                }
                else
                {
                    speedWaitingTime -= gameTime.ElapsedGameTime.Milliseconds;
                }

            }
            if (Speed == WantedSpeed && !this.IsLanding)
            {
                speedChangeFinished = true;
            }
        }

        private void changeAlt(GameTime gameTime)
        {
            if (Alt == WantedAlt && pitch != 0.0f)
            {
                pitch -= (pitch > 0.0f ? 1 : -1) * 5 * (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                if (Math.Abs(pitch) < 0.5)
                {
                    pitch = 0;
                }
            }
            if (Alt != WantedAlt && altChangeFinished)
            {
                altChangeFinished = false;
                altWaitingTime = reactionTime;
            }
            if (Alt != WantedAlt && !altChangeFinished)
            {
                if (reactionOffset > 0)
                {
                    reactionOffset -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else if (altWaitingTime <= 0)
                {
                    int sign = 1;
                    if (WantedAlt < Alt)
                    {
                        sign = -1;
                    }
                    Alt += sign * altChange * gameTime.ElapsedGameTime.Milliseconds / 1000;
                    if ((Alt > WantedAlt && sign == 1) || (Alt < WantedAlt && sign == -1))
                    {
                        Alt = WantedAlt;
                    }
                    pitch += sign * 5 * (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

                    if (this.crashed)   // pokud letadlo crashlo, tak se kromě klasického ubrání výšky odebere výška ještě jednou, aby letadlo rychleji padalo
                    {
                        Alt -= altChange * gameTime.ElapsedGameTime.Milliseconds / 1000;
                    }

                    if (this.crashed && Math.Abs(pitch) > 60.0f)
                    {
                        pitch = sign * 60.0f;
                    }
                    if (Math.Abs(pitch) > 10.0f && !this.crashed)
                    {
                        pitch = sign * 10.0f;
                    }


                }
                else
                {
                    altWaitingTime -= gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            if (Alt == WantedAlt && !this.isDescendingToRunway)
            {
                altChangeFinished = true;
            }
        }

        private void changeDir(GameTime gameTime)
        {
            if (Dir == WantedDir && roll != 0.0f && !this.crashed)
            {
                roll -= (roll > 0.0f ? 1 : -1) * 5 * (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                if (Math.Abs(roll) < 0.5)
                {
                    roll = 0;
                }
            }
            if (Dir != WantedDir && dirChangeFinished)
            {
                dirChangeFinished = false;
                dirWaitingTime = reactionTime;
                // prudky obrat!
                // Ruším efekt prudkého obratu. Je to divné. V originále to fungovalo trošku jinak...
                /*
                double requestedChange = Math.Abs(Dir - WantedDir);
                if (requestedChange > 130 && requestedChange < 180)
                {
                    Main.state.changeScore(-20);
                }
                else if (requestedChange > 180 && 360 - requestedChange > 130)
                {
                    Main.state.changeScore(-20);
                }
                 * */
            }
            if (Dir != WantedDir && !dirChangeFinished)
            {
                if (reactionOffset > 0)
                {
                    reactionOffset -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else if (dirWaitingTime <= 0)
                {
                    if (Math.Abs(Dir - WantedDir) > 0.5)
                    {
                        int sign = 1;
                        if (Dir > WantedDir)
                        {
                            sign = -1;
                        }
                        double diff = Math.Abs(Dir - wantedDir);
                        if (diff > 180)
                        {
                            sign = -sign;
                        }
                        roll += sign * 5 * (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                        if (Math.Abs(roll) > 20.0f)
                        {
                            roll = sign * 20.0f;
                        }
                        dir += sign * 1 * dirChange * (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                        if (dir >= 360) { dir -= 360; }
                        else if (dir < 0) { dir += 360; }
                        if ((Dir > WantedDir && sign == 1) && diff <= 180 || (Dir < WantedDir && sign == -1) && diff <= 180)
                        {
                            Dir = WantedDir;
                        }
                    }
                    else
                    {
                        Dir = WantedDir;
                    }
                }
                else
                {
                    dirWaitingTime -= gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            if (Dir == WantedDir && !this.isApproaching)
            {
                dirChangeFinished = true;
            }
            if (this.crashed) {
                roll += 8 * (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            }
        }

        private void changeFuel(GameTime gameTime)
        {
            if (Main.settings.Fuel)
            {
                Fuel -= fuelChange * gameTime.ElapsedGameTime.Milliseconds / 1000;
                if (Fuel <= 0)
                {
                    fall();
                }
            }
        }

        /// <summary>
        /// Uloží nový bod do řady trace pointů za letadlem. Poslední tracepoint se zapomene.
        /// </summary>
        /// <param name="x">Souřadnice nového trace pointu</param>
        /// <param name="y">Souřadnice nového trace pointu</param>
        private void StoreTracePoint(int x, int y)
        {
            for (int i = 4; i > 0; i--)
            {
                tracePoints[i].X = tracePoints[i - 1].X;
                tracePoints[i].Y = tracePoints[i - 1].Y;
            }
            tracePoints[0].X = x;
            tracePoints[0].Y = y;
        }
    }
}