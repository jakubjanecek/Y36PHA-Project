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
    public class Main : Microsoft.Xna.Framework.Game
    {
        public static State state = State.getInstance();
        public static Settings settings = Settings.getInstance();
        public Sound sound;
        public Input input;
        public Effect effect;
        public static Vector2 sectorSize = new Vector2(40, 30);
        public const int WIDTH = 987;
        public const int HEIGHT = 740;
        public GameState lastState = settings.DefaultMode;

        private static Main main = null;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Color backgroundColor;
        private Intro intro;
        private MainMenu mainMenu;
        private Hud hud;
        private Weather weather;
        private Terrain terrain;

        private Plane selectedPlane = null;    // právě vybrané letadlo
        private Plane focusPlane = null;    // letadlo, které je právě sledováno kamerou
        private List<Plane> planes;     // kolekce letadel na hrací ploše
        private List<Airport> runways;  // kolekce přistávacích drah
        private List<Exit> sectorExits; // kolekce exitů ze sektoru
        public Level[] levels = new Level[10];

        private Texture2D gameOver;
        private Texture2D nextLevel;
        private Texture2D congrats;

        private int delay = 0;

        private HashSet<Plane> inCollision = new HashSet<Plane>();
        private HashSet<Plane> outOfSector = new HashSet<Plane>();
        private HashSet<Plane> inStorm = new HashSet<Plane>();
        private HashSet<Plane> planesToRemove = new HashSet<Plane>();

        Model boeing;
        Model boeing_BRI;
        Model boeing_OCN;
        Model boeing_ATL;
        Model boeing_CSA;
        Model boeing_DEL;

        Model airbus;
        Model airbus_BRI;
        Model airbus_OCN;
        Model airbus_ATL;
        Model airbus_CSA;
        Model airbus_DEL;

        Model cesna;

        public Vector3 cameraPosition = new Vector3(20.0f, 30.0f, 15.0f);
        public Vector3 cameraLookAt = new Vector3(20.0f, 0.0f, 15.0f);
        private Vector3 cameraWantedLookAt = new Vector3(20.0f, 0.0f, 15.0f);
        private Vector3 cameraWantedPosition = new Vector3(20.0f, 30.0f, 15.0f);
        private float cameraInterpolation = 1.0f;

        private Matrix cameraProjectionMatrix;
        private Matrix cameraViewMatrix;
        public int cameraVAngle = 0;
        public int cameraHAngle = 0;
        float cameraZoom = 7.0f;

        public Model Boeing { get { return boeing; } }
        public Model Boeing_ATL { get { return boeing_ATL; } }
        public Model Boeing_BRI { get { return boeing_BRI; } }
        public Model Boeing_CSA { get { return boeing_CSA; } }
        public Model Boeing_DEL { get { return boeing_DEL; } }
        public Model Boeing_OCN { get { return boeing_OCN; } }

        public Model Airbus { get { return airbus; } }
        public Model Airbus_ATL { get { return airbus_ATL; } }
        public Model Airbus_BRI { get { return airbus_BRI; } }
        public Model Airbus_CSA { get { return airbus_CSA; } }
        public Model Airbus_DEL { get { return airbus_DEL; } }
        //public Model Airbus_OCN { get { return airbus_OCN; } }

        public Model Cesna { get { return cesna; } }

        public Terrain Terrain { get { return terrain;  } }

        private Main()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = WIDTH;
            this.graphics.PreferredBackBufferHeight = HEIGHT;
            // this.graphics.IsFullScreen = true;

            backgroundColor = new Color(0, 0, 85, 255);
            planes = new List<Plane>();
            runways = new List<Airport>();
            sectorExits = new List<Exit>();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            state.CurrentState = GameState.INTRO;

            intro = new Intro(this);
            input = new Input(this);
            sound = new Sound(this);
            mainMenu = new MainMenu(this);
            hud = new Hud(this);
            terrain = new Terrain(this, "heightmap1", 0, 0);
            weather = new Weather(this);
            Components.Add(intro);
            Components.Add(input);
            Components.Add(sound);
            Components.Add(mainMenu);
            Components.Add(hud);
            Components.Add(terrain);
            Components.Add(weather);

            levels[0] = new Level01();
            levels[1] = new Level02();
            levels[2] = new Level03();
            levels[3] = new Level04();
            levels[4] = new Level05();
            levels[5] = new Level06();
            levels[6] = new Level07();
            levels[7] = new Level08();
            levels[8] = new Level09();
            levels[9] = new Level10();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            effect = this.Content.Load<Effect>("effect");
            gameOver = this.Content.Load<Texture2D>("gameover");
            nextLevel = this.Content.Load<Texture2D>("levelfinished");
            congrats = this.Content.Load<Texture2D>("congrats");

            boeing = Content.Load<Model>("Modely\\747\\747");
            boeing_ATL = Content.Load<Model>("Modely\\747_ATL\\747");
            boeing_BRI = Content.Load<Model>("Modely\\747_BRI\\747");
            boeing_CSA = Content.Load<Model>("Modely\\747_CSA\\747");
            boeing_DEL = Content.Load<Model>("Modely\\747_DEL\\747");
            boeing_OCN = Content.Load<Model>("Modely\\747_OCN\\747");

            airbus = Content.Load<Model>("Modely\\A340\\a340");
            airbus_ATL = Content.Load<Model>("Modely\\A340_ATL\\a340");
            airbus_BRI = Content.Load<Model>("Modely\\A340_BRI\\a340");
            airbus_CSA = Content.Load<Model>("Modely\\A340_CSA\\a340");
            airbus_DEL = Content.Load<Model>("Modely\\A340_DEL\\a340");

            cesna = Content.Load<Model>("Modely\\Cesna\\cessna");

            cameraViewMatrix = Matrix.CreateLookAt(
                cameraPosition,
                cameraLookAt,
                Vector3.Up);

            cameraProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                graphics.GraphicsDevice.Viewport.AspectRatio,
                1.0f,
                10000.0f);

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (state.CurrentState == GameState.GAME_2D || state.CurrentState == GameState.GAME_3D)
            {
                if (Main.getInstance().input.SwitchDisplayMode())
                {
                    state.CurrentState = state.CurrentState == GameState.GAME_2D ? GameState.GAME_3D : GameState.GAME_2D;
                }
                if (Main.getInstance().input.CameraUp())
                {
                    if (cameraVAngle < 90) { cameraVAngle++; }
                }
                if (Main.getInstance().input.CameraDown())
                {
                    if (cameraVAngle > -90) { cameraVAngle--; }
                }
                if (Main.getInstance().input.CameraLeft())
                {
                    cameraHAngle++;
                    cameraHAngle %= 360;
                }
                if (Main.getInstance().input.CameraRight())
                {
                    cameraHAngle--;
                    cameraHAngle %= 360;
                }
                if (Main.getInstance().input.CameraZoomOut())
                {
                    if (cameraZoom < 37)
                    {
                        cameraZoom += 0.1f;
                    }
                }
                if (Main.getInstance().input.CameraZoomIn())
                {
                    if (cameraZoom >= 0.5f)
                    {
                        cameraZoom -= 0.1f;
                    }
                }
                /******************************************************
                 * ****************************************************
                 * ****************************************************/

                // Vstup do herního menu
                if (input.AccessedGameMenu() && selectedPlane == null)
                {
                    lastState = state.CurrentState;
                    state.CurrentState = GameState.MENU_GAME;
                }

                foreach (Plane plane in planes)
                {
                    if (!plane.Falling && input.SelectPlane(plane))
                    {
                        SelectPlane(plane);
                    }
                }
                gameRules();
                detectPlaneCollisions();
                cheats();
            }

            if (state.CurrentState == GameState.GAMEOVER || state.CurrentState == GameState.CONGRATS)
            {
                if (delay < 0 && input.Any())
                {
                    resetGame();
                    state.resetScore();
                    state.resetMistakes();
                    state.resetLevel();
                    state.CurrentState = GameState.MENU_MAIN;
                }
                else
                {
                    if (delay == 0)
                    {
                        delay = 1000;
                    }
                    else
                    {
                        delay -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                }
            }

            if (state.CurrentState == GameState.NEXT_LEVEL)
            {
                if (delay < 0 && input.Any())
                {
                    resetGame();
                    levels[state.Level - 1].run();
                    state.CurrentState = lastState;
                }
                else
                {
                    if (delay == 0)
                    {
                        delay = 1000;
                    }
                    else
                    {
                        delay -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                }
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(backgroundColor);
            if (Main.state.CurrentState == GameState.GAMEOVER)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(gameOver, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White);
                spriteBatch.End();
            }
            if (Main.state.CurrentState == GameState.NEXT_LEVEL)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(nextLevel, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White);
                spriteBatch.End();
            }
            if (Main.state.CurrentState == GameState.CONGRATS)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(congrats, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White);
                spriteBatch.End();
            }
            if (Main.state.CurrentState == GameState.GAME_3D)
            {
                // Focus na vybrané letadlo
                if (selectedPlane != null)
                {
                    if (focusPlane != selectedPlane)
                    {
                        cameraInterpolation = 0.0f;
                    }
                    focusPlane = selectedPlane;
                }

                if (focusPlane != null)
                {
                    cameraWantedLookAt = focusPlane.Get3dLocation();
                }

                cameraWantedPosition.X = cameraWantedLookAt.X + (cameraZoom) * (float)Math.Sin(MathHelper.ToRadians(cameraHAngle));
                cameraWantedPosition.Z = cameraWantedLookAt.Z - (cameraZoom) * (float)Math.Cos(MathHelper.ToRadians(cameraHAngle));
                cameraWantedPosition.Y = cameraWantedLookAt.Y + (cameraZoom) * (float)Math.Sin(MathHelper.ToRadians(cameraVAngle));

                if (cameraInterpolation < 1.0f)
                {
                    cameraInterpolation += 0.01f;
                }
                cameraPosition.X = MathHelper.Lerp(cameraPosition.X, cameraWantedPosition.X, cameraInterpolation);
                cameraPosition.Y = MathHelper.Lerp(cameraPosition.Y, cameraWantedPosition.Y, cameraInterpolation);
                cameraPosition.Z = MathHelper.Lerp(cameraPosition.Z, cameraWantedPosition.Z, cameraInterpolation);
                cameraLookAt.X = MathHelper.Lerp(cameraLookAt.X, cameraWantedLookAt.X, cameraInterpolation);
                cameraLookAt.Y = MathHelper.Lerp(cameraLookAt.Y, cameraWantedLookAt.Y, cameraInterpolation);
                cameraLookAt.Z = MathHelper.Lerp(cameraLookAt.Z, cameraWantedLookAt.Z, cameraInterpolation);

                cameraViewMatrix = CalculateViewMatrix();
                cameraProjectionMatrix = CalculateProjectionMatrix();
            }
            base.Draw(gameTime);
        }


        /*****************************************************************
         * ***************************************************************
         * 
         *                      VLASTNÍ METODY
         *
         * ***************************************************************
         * ***************************************************************/


        /// <summary>
        /// Získá instanci třídy Main
        /// </summary>
        /// <returns>Instance třídy Main</returns>
        public static Main getInstance()
        {
            if (main == null)
            {
                main = new Main();
            }
            return main;
        }

        private void gameRules()
        {
            foreach (Plane p in planesToRemove)
            {
                RemovePlane(p);
            }
            foreach (Plane p in planes)
            {
                if (p.Fell)
                {
                    if (!p.Crashed)
                    {
                        state.mistake();
                    }
                    planesToRemove.Add(p);
                }
                if (p.Landed)
                {
                    planesToRemove.Add(p);
                    state.changeScore(100);
                    if (p.TimeFromAppearance <= 240000)
                    {
                        state.changeScore(20);
                    }
                    if (p.AlwaysFarEnough)
                    {
                        state.changeScore(50);
                    }
                }
                if (weather.inStorm(p.X, p.Y))
                {
                    if (!inStorm.Contains(p))
                    {
                        inStorm.Add(p);
                        state.changeScore(-10);
                    }
                }
                else if (inStorm.Contains(p))
                {
                    inStorm.Remove(p);
                }
                if (p.X < 0 || p.X > sectorSize.X || p.Y < 0 || p.Y > sectorSize.Y)
                {
                    if (!outOfSector.Contains(p))
                    {
                        state.changeScore(-50);
                        outOfSector.Add(p);
                    }
                    else
                    {
                        if (p.X < -1 || p.X > sectorSize.X + 1 || p.Y < -1 || p.Y > sectorSize.Y + 1)
                        {
                            outOfSector.Remove(p);
                            planesToRemove.Add(p);
                        }
                    }
                }
                else if (outOfSector.Contains(p))
                {
                    outOfSector.Remove(p);
                }
            }
        }

        private void detectPlaneCollisions()
        {
            bool setBefore = false;
            foreach (Plane p1 in planes)
            {
                foreach (Plane p2 in planes)
                {
                    double distance = Double.PositiveInfinity;
                    double vertDistance = Double.PositiveInfinity;
                    if (p1 != p2 && (distance = computeDistance(p1.X, p1.Y, p2.X, p2.Y)) < 3.0 && (vertDistance = Math.Abs(p1.Alt - p2.Alt)) < 1000)
                    {
                        if (distance <= 0.1 && vertDistance < 100)
                        {
                            p1.crash();
                            p2.crash();
                        }
                        p1.Colliding = true;
                        p2.Colliding = true;
                        if (!(inCollision.Contains(p1) && inCollision.Contains(p2)))
                        {
                            state.mistake();
                            state.changeScore(-100);
                            sound.playPreferred("alarm");
                            inCollision.Add(p1);
                            inCollision.Add(p2);
                        }
                        setBefore = true;
                    }
                    else if (p1 != p2 && !setBefore)
                    {
                        p1.Colliding = false;
                        p2.Colliding = false;
                        if (inCollision.Contains(p1) && inCollision.Contains(p2))
                        {
                            inCollision.Remove(p1);
                            inCollision.Remove(p2);
                        }
                    }
                    if (distance < 5.0 || vertDistance < 1500)
                    {
                        p1.AlwaysFarEnough = false;
                        p2.AlwaysFarEnough = false;
                    }
                }
            }
        }

        public static double computeDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(x1 - x2), 2) + Math.Pow(Math.Abs(y1 - y2), 2));
        }

        public void AddPlane(Plane plane)
        {
            Components.Add(plane);
            planes.Add(plane);
        }

        public void RemovePlane(Plane plane)
        {
            if (focusPlane == plane)
            {
                focusPlane = null;
            }
            Components.Remove(plane);
            planes.Remove(plane);
        }

        public void AddTerrain(Terrain terrain)
        {
            this.terrain = terrain;
            Components.Add(this.terrain);
        }

        public void AddAirport(Airport airport)
        {
            Components.Add(airport);
            runways.Add(airport);
        }

        public void AddExit(Exit exit)
        {
            Components.Add(exit);
            sectorExits.Add(exit);
        }

        public Plane GetSelectedPlane()
        {
            return selectedPlane;
        }

        public Plane GetFocusPlane()
        {
            return focusPlane;
        }

        public List<Plane> getPlanes()
        {
            return planes;
        }

        public void SelectPlane(Plane plane)
        {
            selectedPlane = plane;
            if (plane != null)
            {
                hud.WantedDir = plane.WantedDir;
                hud.WantedAlt = plane.WantedAlt;
                hud.WantedSpeed = plane.WantedSpeed;
            }
        }

        /**
         * x and y in miles
         * 
         */
        public Airport GetLandingAirport(double x, double y)
        {
            Airport closest = null; // runways[0];

            foreach (Airport a in runways)
            {
                if (closest == null || a.GetAngleFromAxis((float)x, (float)y) < closest.GetAngleFromAxis((float)x, (float)y))
                {
                    closest = a;
                }
            }
            return closest;
        }

        public void resetGame()
        {
            /*
            if (terrain != null)
            {
                terrain.Visible = false;
                terrain.Enabled = false;
                terrain.Dispose();
            }
            Components.Remove(terrain);
             * */
            foreach (Plane p1 in planes)
            {
                Components.Remove(p1);
            }
            foreach (Airport a1 in runways)
            {
                Components.Remove(a1);
            }
            foreach (Exit e in sectorExits)
            {
                Components.Remove(e);
            }
            planes.Clear();
            runways.Clear();
            sectorExits.Clear();
            selectedPlane = null;
            weather.Clear();
            sound.Clear();
            delay = 0;
        }

        /********************************************************************
         * Vrátí relativní pozici bodu 2 vůči bodu 1
         * ******************************************************************/
        public static Vector2 getRelativePosition(float x1, float y1, float x2, float y2)
        {
            return new Vector2(x2 - x1, y2 - y1);
        }

        public static Vector2 getRelativePosition(Vector2 point1, Vector2 point2)
        {
            return getRelativePosition(point1.X, point1.Y, point2.X, point2.Y);
        }



        /********************************************************************
         * Vrátí kurz (ve stupních) pro bod umístěný na relativních souřadnicích
         * 
         * relativePosX/Y - relativní umístění cílového bodu
         * ******************************************************************/
        public static int getCourse(float relativePosX, float relativePosY)
        {
            int offset = 0;
            if (relativePosY >= 0)
            {
                offset = 180;
            }
            else if (relativePosY < 0 && relativePosX < 0)
            {
                offset = 360;
            }
            return offset - (int)MathHelper.ToDegrees((float)Math.Atan(relativePosX / relativePosY));
        }

        public static int getCourse(Vector2 relativePos)
        {
            return getCourse(relativePos.X, relativePos.Y);
        }



        /********************************************************************
         * Vrátí kurz (ve stupních) od bodu 1 do bodu 2
         * ******************************************************************/
        public static int getCourse(float x1, float y1, float x2, float y2)
        {
            return getCourse(getRelativePosition(x1, y1, x2, y2));
        }

        public static int getCourse(Vector2 point1, Vector2 point2)
        {
            return getCourse(point1.X, point1.Y, point2.X, point2.Y);
        }



        /********************************************************************
         * Získá nejmenší úhel mezi dvěma kurzy. Vrací absolutní hodnotu.
         * ******************************************************************/
        public static float GetAngleBetween(float course1, float course2)
        {
            float basicAngle = Math.Abs(course1 - course2);
            return basicAngle > 180 ? 360 - basicAngle : basicAngle;
        }



        /********************************************************************
         * Vrátí vzdálenost dvou bodů
         * ******************************************************************/
        public static double GetDistance(float x1, float y1, float x2, float y2)
        {
            float x = Math.Abs(x1 - x2);
            float y = Math.Abs(y1 - y2);
            return Math.Sqrt((double)x * x + y * y);
        }

        public static double GetDistance(Vector2 point1, Vector2 point2)
        {
            return GetDistance(point1.X, point1.Y, point2.X, point2.Y);
        }


        /*********************************************************************
         * Vykreslí 3D model
         * *******************************************************************/
        public void DrawModel(Model model, Vector3 modelPosition, float rotation, float pitch, float roll, float scale, Vector3 modelCenterCorrection, Matrix rotationCorrection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in mesh.Effects)
                {
                    basicEffect.EnableDefaultLighting();
                    basicEffect.PreferPerPixelLighting = true;

                    basicEffect.World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(modelCenterCorrection) * rotationCorrection * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(rotation), MathHelper.ToRadians(pitch), MathHelper.ToRadians(roll)) * Matrix.CreateTranslation(modelPosition);
                    /*if (scale != 1.0f)
                    {
                        effect.World = Matrix.CreateScale(scale);
                    }*/
                    basicEffect.Projection = cameraProjectionMatrix;
                    basicEffect.View = cameraViewMatrix;
                }
                mesh.Draw();
            }
        }

        /*****************************************************************
         * Vrátí souřadnice projekce 3D bodu scény na obrazovku monitoru
         * ***************************************************************/
        public Vector2 Transfer3to2d(Vector3 point3d)
        {
            Vector3 projectedPosition = GraphicsDevice.Viewport.Project(point3d, cameraProjectionMatrix, cameraViewMatrix, Matrix.Identity);
            if (projectedPosition.Z < 1)
            {
                return new Vector2(projectedPosition.X, projectedPosition.Y);
            }
            else
                return new Vector2(-10.0f, -10.0f);
        }

        /********************************************************************
         * Vypočítá projekční a pohledovou matici
         * ******************************************************************/
        public Matrix CalculateViewMatrix()
        {
            return Matrix.CreateLookAt(
                    cameraPosition,
                    cameraLookAt,
                    Vector3.Up);
        }

        public Matrix CalculateProjectionMatrix()
        {
            return Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                graphics.GraphicsDevice.Viewport.AspectRatio,
                0.1f,
                100.0f);
        }

        private void cheats()
        {
            if (input.cheatWeather())
            {
                weather.cheat();
            }
            if (input.cheatNavHelp())
            {
                settings.TracePoints = !settings.TracePoints;
            }
            if (input.cheatNextLevel())
            {
                state.changeScore(500);
            }
            if (input.cheatNotWorking())
            {
                runways[0].cheat();
            }
            if (input.cheatPlaneCrash())
            {
                Plane p1 = new Boeing(this, 21, 25, 5500, 220, 90, "CSA 000", 1000.0);
                Plane p2 = new Boeing(this, 25, 25, 5500, 220, 270, "CSA 001", 1000.0);
                AddPlane(p1);
                AddPlane(p2);
            }
            if (input.cheatFallingPlane())
            {
                Plane p1 = new Boeing(this, 21, 25, 5500, 220, 90, "CSA 000", 20.0);
                //settings.Sound = false;
                AddPlane(p1);
            }
            if (input.cheatSound()) {
                settings.Sound = !settings.Sound;
            }
        }
    }
}
