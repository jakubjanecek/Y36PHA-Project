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
    public class Weather : Microsoft.Xna.Framework.DrawableGameComponent
    {
        static int weatherMatrixX = 80;
        static int weatherMatrixY = 60;
        private bool[,] weather = new bool[weatherMatrixX, weatherMatrixY];
        private Random random = new Random();
        private double delay = 0;
        private int stormSize = 0;
        private int weatherDelay = 18;
        private int weatherRand = 100000;

        private GraphicsDevice graphics;
        private ContentManager content;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Texture2D texture3d;
        private Texture2D textureLightning;

        private Effect bbEffect;
        private VertexBuffer vertexBuffer;
        private VertexDeclaration vertexDeclaration;
        private VertexBuffer lightningVertexBuffer;
        private VertexDeclaration lightningVertexDeclaration;
        private Vector3[] stormElementsPositions;


        public Weather(Game game)
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics);
            texture = content.Load<Texture2D>("white");
            texture3d = content.Load<Texture2D>("Storm");
            textureLightning = content.Load<Texture2D>("Blesk");
            bbEffect = content.Load<Effect>("effect");
            stormElementsPositions = new Vector3[80 * 60 * 2];
            Random rand = new Random();
            for (int i = 0; i < stormElementsPositions.Length; i++)
            {
                stormElementsPositions[i].X = rand.Next(40000) / 1000.0f;
                //stormElementsPositions[i].Y = rand.Next(1500, 4000) / 1000.0f;
                stormElementsPositions[i].Y = rand.Next(5000, 8000) / 5280.0f * 2;
                stormElementsPositions[i].Z = random.Next(30000) / 1000.0f;
            }

            VertexPositionTexture[] vertices = new VertexPositionTexture[12];
            float stormBillboardWidth = (float)texture3d.Width / texture3d.Width;
            float stormBillboardHeight = (float)texture3d.Height / texture3d.Width;
            vertices[0] = new VertexPositionTexture(new Vector3(-stormBillboardWidth, -stormBillboardHeight, 0), new Vector2(0, 0));
            vertices[2] = new VertexPositionTexture(new Vector3(stormBillboardWidth, stormBillboardHeight, 0), new Vector2(1, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(-stormBillboardWidth, stormBillboardHeight, 0), new Vector2(0, 1));
            vertices[3] = new VertexPositionTexture(new Vector3(-stormBillboardWidth, -stormBillboardHeight, 0), new Vector2(0, 0));
            vertices[5] = new VertexPositionTexture(new Vector3(stormBillboardWidth, -stormBillboardHeight, 0), new Vector2(1, 0));
            vertices[4] = new VertexPositionTexture(new Vector3(stormBillboardWidth, stormBillboardHeight, 0), new Vector2(1, 1));

            vertices[6] = new VertexPositionTexture(new Vector3(stormBillboardWidth, -stormBillboardHeight, 0), new Vector2(0, 0));
            vertices[8] = new VertexPositionTexture(new Vector3(-stormBillboardWidth, stormBillboardHeight, 0), new Vector2(1, 1));
            vertices[7] = new VertexPositionTexture(new Vector3(stormBillboardWidth, stormBillboardHeight, 0), new Vector2(0, 1));
            vertices[9] = new VertexPositionTexture(new Vector3(stormBillboardWidth, -stormBillboardHeight, 0), new Vector2(0, 0));
            vertices[11] = new VertexPositionTexture(new Vector3(-stormBillboardWidth, -stormBillboardHeight, 0), new Vector2(1, 0));
            vertices[10] = new VertexPositionTexture(new Vector3(-stormBillboardWidth, stormBillboardHeight, 0), new Vector2(1, 1));

            vertexBuffer = new VertexBuffer(graphics, vertices.Length * VertexPositionTexture.SizeInBytes, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
            vertexDeclaration = new VertexDeclaration(graphics, VertexPositionTexture.VertexElements);

            VertexPositionTexture[] lightningVertices = new VertexPositionTexture[6];
            lightningVertices[0] = new VertexPositionTexture(new Vector3(-0.5f, 0, 0), new Vector2(0, 0));
            lightningVertices[2] = new VertexPositionTexture(new Vector3(0.5f, -stormBillboardHeight - 1.5f, 0), new Vector2(1, 1));
            lightningVertices[1] = new VertexPositionTexture(new Vector3(-0.5f, -stormBillboardHeight - 1.5f, 0), new Vector2(0, 1));
            lightningVertices[3] = new VertexPositionTexture(new Vector3(-0.5f, 0, 0), new Vector2(0, 0));
            lightningVertices[5] = new VertexPositionTexture(new Vector3(0.5f, 0, 0), new Vector2(1, 0));
            lightningVertices[4] = new VertexPositionTexture(new Vector3(0.5f, -stormBillboardHeight - 1.5f, 0), new Vector2(1, 1));

            lightningVertexBuffer = new VertexBuffer(graphics, lightningVertices.Length * VertexPositionTexture.SizeInBytes, BufferUsage.WriteOnly);
            lightningVertexBuffer.SetData(lightningVertices);
            lightningVertexDeclaration = new VertexDeclaration(graphics, VertexPositionTexture.VertexElements);

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
            if (Main.settings.Storms && (Main.state.CurrentState == GameState.GAME_2D || Main.state.CurrentState == GameState.GAME_3D))
            {
                delay -= ((double)gameTime.ElapsedGameTime.Milliseconds / 1000);
                if (delay <= 0)
                {
                    int rand = 0;
                    bool[,] copy = weather;
                    for (int i = 0; i < weather.GetLength(0); i++)
                    {
                        for (int j = 0; j < weather.GetLength(1); j++)
                        {
                            rand = random.Next(0, weatherRand);

                            if (weather[i, j] == false)
                            {
                                if ((i < weatherMatrixX - 1 && copy[i + 1, j] || i > 0 && copy[i - 1, j] ||
                                    j < weatherMatrixY - 1 && copy[i, j + 1] || j > 0 && copy[i, j - 1]) &&
                                    rand < ((float)10 - ((float)stormSize / 10.0)) * 1000.0)
                                {
                                    stormSize++;
                                    weather[i, j] = true;
                                }
                                else if ((i == 0 || j == 0 || i == weather.GetLength(0) - 1 || j == weather.GetLength(1) - 1) &&
                                    rand < 10)
                                {
                                    stormSize++;
                                    weather[i, j] = true;
                                }
                                else if (rand < 2)
                                {
                                    stormSize++;
                                    weather[i, j] = true;
                                }
                            }
                            else
                            {
                                if (copy[i, j] &&
                                    (i < weatherMatrixX - 1 && !copy[i + 1, j] || i > 0 && !copy[i - 1, j] ||
                                    j < weatherMatrixY - 1 && !copy[i, j + 1] || j > 0 && !copy[i, j - 1]) &&
                                    rand < ((float)stormSize / 10.0) * 1000.0)
                                {
                                    stormSize--;
                                    weather[i, j] = false;
                                }
                            }
                        }
                    }
                    delay = weatherDelay;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Main.settings.Storms)
            {
                switch (Main.state.CurrentState)
                {
                    case GameState.GAME_2D:
                        spriteBatch.Begin();
                        for (int i = 0; i < weather.GetLength(0); i++)
                        {
                            for (int j = 0; j < weather.GetLength(1); j++)
                            {
                                if (weather[i, j] == true)
                                {
                                    int w = Main.WIDTH / weather.GetLength(0);
                                    int h = Main.HEIGHT / weather.GetLength(1);
                                    spriteBatch.Draw(texture, new Rectangle(i * w, j * h, w, h), Color.RoyalBlue);
                                }
                            }
                        }
                        spriteBatch.End();
                        break;
                    case GameState.GAME_3D:
                        DrawClouds();
                        break;
                    default:
                        break;
                }
            }
            base.Draw(gameTime);
        }

        private void DrawClouds()
        {
            bbEffect.CurrentTechnique = bbEffect.Techniques["Textured"];

            Random rand = new Random();

            foreach (Vector3 position in stormElementsPositions)
            {
                if (inStorm(position.X, position.Z))
                {
                    Vector3 eyeDirection = Main.getInstance().cameraPosition - position;// cross(eyeVector, upVector);
                    float courseToEye = Main.getCourse((float)eyeDirection.X, (float)eyeDirection.Z);

                    Matrix worldMatrix = Matrix.CreateRotationY(-MathHelper.ToRadians(courseToEye)) * Matrix.CreateTranslation(position);
                    Main.getInstance().effect.Parameters["xWorld"].SetValue(worldMatrix);
                    Main.getInstance().effect.Parameters["xView"].SetValue(Main.getInstance().CalculateViewMatrix());
                    Main.getInstance().effect.Parameters["xProjection"].SetValue(Main.getInstance().CalculateProjectionMatrix());

                    Main.getInstance().effect.Parameters["xTexture"].SetValue(this.texture3d);
                    Main.getInstance().effect.Parameters["xEnableLighting"].SetValue(true);
                    Main.getInstance().effect.Parameters["xAmbient"].SetValue(0.4f);
                    Main.getInstance().effect.Parameters["xLightDirection"].SetValue(new Vector3(-0.5f, -1, -0.5f));

                    GraphicsDevice.RenderState.DepthBufferEnable = true;
                    Main.getInstance().effect.Begin();
                    foreach (EffectPass pass in Main.getInstance().effect.CurrentTechnique.Passes)
                    {
                        graphics.Vertices[0].SetSource(vertexBuffer, 0, VertexPositionTexture.SizeInBytes);
                        graphics.VertexDeclaration = vertexDeclaration;
                        int noVertices = vertexBuffer.SizeInBytes / VertexPositionTexture.SizeInBytes;

                        graphics.RenderState.AlphaTestEnable = true;
                        graphics.RenderState.AlphaFunction = CompareFunction.GreaterEqual;
                        graphics.RenderState.ReferenceAlpha = 250;

                        pass.Begin();
                        graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, noVertices / 3);
                        pass.End();

                        graphics.RenderState.DepthBufferWriteEnable = false;
                        graphics.RenderState.AlphaBlendEnable = true;
                        graphics.RenderState.SourceBlend = Blend.SourceAlpha;

                        bool showLightning = rand.Next(1000) < 1;

                        graphics.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                        graphics.RenderState.AlphaTestEnable = true;
                        graphics.RenderState.AlphaFunction = CompareFunction.Less;
                        graphics.RenderState.ReferenceAlpha = 250;

                        pass.Begin();
                        graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, noVertices / 3);
                        pass.End();

                        graphics.RenderState.AlphaBlendEnable = false;
                        graphics.RenderState.DepthBufferWriteEnable = true;
                        graphics.RenderState.AlphaTestEnable = false;
                        if (showLightning)
                        {
                            Main.getInstance().effect.Parameters["xTexture"].SetValue(this.textureLightning);

                            graphics.Vertices[0].SetSource(lightningVertexBuffer, 0, VertexPositionTexture.SizeInBytes);
                            graphics.VertexDeclaration = lightningVertexDeclaration;
                            noVertices = lightningVertexBuffer.SizeInBytes / VertexPositionTexture.SizeInBytes;

                            graphics.RenderState.AlphaTestEnable = true;
                            graphics.RenderState.AlphaFunction = CompareFunction.GreaterEqual;
                            graphics.RenderState.ReferenceAlpha = 250;

                            pass.Begin();
                            graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, noVertices / 3);
                            pass.End();

                            graphics.RenderState.DepthBufferWriteEnable = false;
                            graphics.RenderState.AlphaBlendEnable = true;
                            graphics.RenderState.SourceBlend = Blend.SourceAlpha;

                            graphics.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
                            graphics.RenderState.AlphaTestEnable = true;
                            graphics.RenderState.AlphaFunction = CompareFunction.Less;
                            graphics.RenderState.ReferenceAlpha = 250;

                            pass.Begin();
                            graphics.DrawPrimitives(PrimitiveType.TriangleList, 0, noVertices / 3);
                            pass.End();

                            graphics.RenderState.AlphaBlendEnable = false;
                            graphics.RenderState.DepthBufferWriteEnable = true;
                            graphics.RenderState.AlphaTestEnable = false;
                        }
                    }
                    Main.getInstance().effect.End();
                }
            }
        }

        public bool inStorm(double x, double y)
        {
            double xSize = Main.sectorSize.X / weather.GetLength(0);
            double ySize = Main.sectorSize.Y / weather.GetLength(1);
            int xIdx = (int)(x / xSize);
            int yIdx = (int)(y / ySize);
            if (xIdx < weather.GetLength(0) && yIdx < weather.GetLength(1) && xIdx >= 0 && yIdx >= 0 && weather[xIdx, yIdx])
            {
                return true;
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < weatherMatrixX; i++)
            {
                for (int j = 0; j < weatherMatrixY; j++)
                {
                    weather[i, j] = false;
                }
            }
        }

        public void cheat()
        {
            weatherDelay = weatherDelay == 18 ? 2 : 18;
            weatherRand = weatherRand == 100000 ? 10000 : 100000;
            delay = 0;
        }
    }
}