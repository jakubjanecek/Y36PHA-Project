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
    enum MenuPanelBehavior
    {
        OUT,        // panel je zcela vysunut
        HIDDEN,     // panel je zcela schován
        APPEARING,  // panel se vysouvá
        HIDING      // panel se zasouvá
    };

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        GraphicsDevice graphics;
        ContentManager content;
        SpriteBatch spriteBatch;

        Texture2D background;           // textura pozadí
        Texture2D panel;                // obrázek panelu
        Rectangle window;               // čtverec o rozměrech okna
        Rectangle panelRect;            // rámec panelu
        Vector2 scale;                  // kolikrát je větší aktuální zobrazení než pozadí

        SpriteFont fontHeadline;        // font nadpisů
        SpriteFont fontItem;            // font položek
        Color greenColor = new Color(11, 186, 20);  // barva neaktivních položek
        Color yellowColor = new Color(255, 255, 0); // barva aktivních položek

        MenuPanelBehavior panelState = MenuPanelBehavior.HIDDEN;   // stav panelu (vysouvá/zasouvá se, je vysunut/zasunut)
        int movingTime = 300;                   // jak dlouho má trvat vysouvání/zasouvání menu (v ms)
        float accuratePanelPosition = 0;        // X-ová pozice panelu s větší přesností než pixely

        int activeItem = 0;             // index aktivní položky menu (na které položce je kurzor)



        public MainMenu(Game game)
            : base(game)
        {
            this.graphics = game.GraphicsDevice;
            content = game.Content;

            window = new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height);
            scale = new Vector2();
            panelRect = new Rectangle();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics);
            background = content.Load<Texture2D>("Main screen");
            panel = content.Load<Texture2D>("Menu panel");
            fontHeadline = content.Load<SpriteFont>("MenuHeadline");
            fontItem = content.Load<SpriteFont>("MenuItem");

            scale.X = (float)graphics.Viewport.Width / (float)background.Width;
            scale.Y = (float)graphics.Viewport.Height / (float)background.Height;
            panelRect = new Rectangle(graphics.Viewport.Width,
                                        (int)(scale.Y * (float)(background.Height - panel.Height - 10)),
                                        (int)(scale.X * (float)panel.Width),
                                        (int)(scale.Y * (float)panel.Height));
            accuratePanelPosition = graphics.Viewport.Width;

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Main.state.CurrentState == GameState.MENU_MAIN ||
                Main.state.CurrentState == GameState.MENU_LOAD ||
                Main.state.CurrentState == GameState.MENU_SETUP ||
                Main.state.CurrentState == GameState.MENU_GAME ||
                Main.state.CurrentState == GameState.MENU_SAVE)
            {
                switch (panelState)
                {
                    case MenuPanelBehavior.HIDDEN:
                        panelState = MenuPanelBehavior.APPEARING;
                        break;
                    case MenuPanelBehavior.OUT:
                        break;
                    case MenuPanelBehavior.APPEARING:
                        PanelShow(gameTime);
                        break;
                    case MenuPanelBehavior.HIDING:
                        break;
                    default:
                        break;
                }
            }

            int offset;
            switch (Main.state.CurrentState)
            {
                case GameState.MENU_MAIN:
                    // aktivace položky v menu
                    offset = (525 - 255) / 4;
                    for (int i = 0; i < 4; i++)
                    {
                        if (Main.getInstance().input.IsMouseInBox(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + i * offset)), panelRect.Width, (int)(scale.Y * offset)))
                        {
                            activeItem = i;
                        }
                    }

                    switch (activeItem)
                    {
                        case 0:
                            // New Game
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * 255), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.getInstance().levels[0].run();
                                Main.state.CurrentState = Main.settings.DefaultMode;
                            }
                            break;
                        case 1:
                            // Load
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.CurrentState = GameState.MENU_LOAD;
                                activeItem = 0;
                            }
                            break;
                        case 2:
                            // Preferences
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 2 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.CurrentState = GameState.MENU_SETUP;
                                activeItem = 0;
                            }
                            break;
                        case 3:
                            // Exit
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 3 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Game.Exit();
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case GameState.MENU_LOAD:
                    // aktivace položky v menu
                    offset = (525 - 255) / 7;
                    for (int i = 0; i < 7; i++)
                    {
                        if (Main.getInstance().input.IsMouseInBox(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + i * offset)), panelRect.Width, (int)(scale.Y * offset)))
                        {
                            activeItem = i;
                        }
                    }

                    switch (activeItem)
                    {
                        case 0:
                            // Slot1
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * 255), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.load("slot1");
                            }
                            break;
                        case 1:
                            // Slot2
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.load("slot2");
                            }
                            break;
                        case 2:
                            // Slot3
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 2 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.load("slot3");
                            }
                            break;
                        case 3:
                            // Slot4
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 3 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.load("slot4");
                            }
                            break;
                        case 4:
                            // Slot5
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 4 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.load("slot5");
                            }
                            break;
                        case 5:
                            // Slot6
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 5 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.load("slot6");
                            }
                            break;
                        case 6:
                            // Back
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 6 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.CurrentState = GameState.MENU_MAIN;
                                activeItem = 1;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case GameState.MENU_SETUP:
                    // aktivace položky v menu
                    offset = (525 - 255) / 6;
                    for (int i = 0; i < 6; i++)
                    {
                        if (Main.getInstance().input.IsMouseInBox(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + i * offset)), panelRect.Width, (int)(scale.Y * offset)))
                        {
                            activeItem = i;
                        }
                    }

                    switch (activeItem)
                    {
                        case 0:
                            // Default mode
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.settings.DefaultMode = Main.settings.DefaultMode == GameState.GAME_3D ? GameState.GAME_2D : GameState.GAME_3D;
                            }
                            break;
                        case 1:
                            // Navigation helpers
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.settings.TracePoints = Main.settings.TracePoints ? false : true;
                            }
                            break;
                        case 2:
                            // Storm
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 2 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.settings.Storms = Main.settings.Storms ? false : true;
                            }
                            break;
                        case 3:
                            // Fuel
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 3 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.settings.Fuel = Main.settings.Fuel ? false : true;
                            }
                            break;
                        case 4:
                            // Sound
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 4 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.settings.Sound = Main.settings.Sound ? false : true;
                            }
                            break;
                        case 5:
                            // Back
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 5 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.CurrentState = GameState.MENU_MAIN;
                                activeItem = 3;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case GameState.MENU_GAME:
                    // aktivace položky v menu
                    offset = (525 - 255) / 5;
                    for (int i = 0; i < 3; i++)
                    {
                        if (Main.getInstance().input.IsMouseInBox(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + i * offset)), panelRect.Width, (int)(scale.Y * offset)))
                        {
                            activeItem = i;
                        }
                    }

                    switch (activeItem)
                    {
                        case 0:
                            // Save
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * 255), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.CurrentState = GameState.MENU_SAVE;
                            }
                            break;
                        case 1:
                            // Continue
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.CurrentState = Main.getInstance().lastState;
                                activeItem = 0;
                            }
                            break;
                        case 2:
                            // End game
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 2 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.getInstance().resetGame();
                                Main.state.resetScore();
                                Main.state.resetMistakes();
                                Main.state.resetLevel();
                                Main.state.CurrentState = GameState.MENU_MAIN;
                                activeItem = 0;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case GameState.MENU_SAVE:
                    // aktivace položky v menu
                    offset = (525 - 255) / 7;
                    for (int i = 0; i < 7; i++)
                    {
                        if (Main.getInstance().input.IsMouseInBox(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + i * offset)), panelRect.Width, (int)(scale.Y * offset)))
                        {
                            activeItem = i;
                        }
                    }

                    switch (activeItem)
                    {
                        case 0:
                            // Slot1
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * 255), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.save("slot1");
                            }
                            break;
                        case 1:
                            // Slot2
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.save("slot2");
                            }
                            break;
                        case 2:
                            // Slot3
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 2 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.save("slot3");
                            }
                            break;
                        case 3:
                            // Slot4
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 3 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.save("slot4");
                            }
                            break;
                        case 4:
                            // Slot5
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 4 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.save("slot5");
                            }
                            break;
                        case 5:
                            // Slot6
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 5 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.save("slot6");
                            }
                            break;
                        case 6:
                            // Back
                            if (Main.getInstance().input.SelectMenuItem(panelRect.X, panelRect.Y + (int)(scale.Y * (255 + 6 * offset)), panelRect.Width, (int)(scale.Y * offset)))
                            {
                                Main.state.CurrentState = GameState.MENU_GAME;
                                activeItem = 0;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            switch (Main.state.CurrentState)
            {
                case GameState.MENU_MAIN:
                case GameState.MENU_LOAD:
                case GameState.MENU_SETUP:
                case GameState.MENU_GAME:
                case GameState.MENU_SAVE:
                    spriteBatch.Begin();
                    spriteBatch.Draw(background, window, Color.White);
                    spriteBatch.Draw(panel, panelRect, Color.White);

                    Color actColor;
                    switch (Main.state.CurrentState)
                    {
                        case GameState.MENU_MAIN:
                            ShowCenteredLabel(fontHeadline, "Main Menu", greenColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * 220));
                            actColor = activeItem == 0 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "New Game", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 1 * (525 - 255) / 8)));
                            actColor = activeItem == 1 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Load", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 3 * (525 - 255) / 8)));
                            actColor = activeItem == 2 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Preferences", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 5 * (525 - 255) / 8)));
                            actColor = activeItem == 3 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Exit", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 7 * (525 - 255) / 8)));
                            break;
                        case GameState.MENU_LOAD:
                            ShowCenteredLabel(fontHeadline, "Load", greenColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * 220));
                            actColor = activeItem == 0 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot1", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 1 * (525 - 255) / 14)));
                            actColor = activeItem == 1 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot2", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 3 * (525 - 255) / 14)));
                            actColor = activeItem == 2 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot3", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 5 * (525 - 255) / 14)));
                            actColor = activeItem == 3 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot4", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 7 * (525 - 255) / 14)));
                            actColor = activeItem == 4 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot5", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 9 * (525 - 255) / 14)));
                            actColor = activeItem == 5 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot6", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 11 * (525 - 255) / 14)));
                            actColor = activeItem == 6 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Back", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 13 * (525 - 255) / 14)));
                            break;
                        case GameState.MENU_SETUP:
                            Vector2 labelSize = fontItem.MeasureString("TEST");
                            labelSize.X *= scale.X;
                            labelSize.Y *= scale.Y;
                            ShowCenteredLabel(fontHeadline, "Preferences", greenColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * 220));
                            actColor = activeItem == 0 ? yellowColor : greenColor;
                            spriteBatch.DrawString(fontItem, "Mode", new Vector2(panelRect.X + 60 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 1 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            spriteBatch.DrawString(fontItem, Main.settings.DefaultMode == GameState.GAME_3D ? "3D" : "2D", new Vector2(panelRect.X + 200 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 1 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            actColor = activeItem == 1 ? yellowColor : greenColor;
                            spriteBatch.DrawString(fontItem, "Helpers", new Vector2(panelRect.X + 60 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 3 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            spriteBatch.DrawString(fontItem, Main.settings.TracePoints ? "ON" : "OFF", new Vector2(panelRect.X + 200 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 3 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            actColor = activeItem == 2 ? yellowColor : greenColor;
                            spriteBatch.DrawString(fontItem, "Storm", new Vector2(panelRect.X + 60 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 5 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            spriteBatch.DrawString(fontItem, Main.settings.Storms ? "ON" : "OFF", new Vector2(panelRect.X + 200 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 5 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            actColor = activeItem == 3 ? yellowColor : greenColor;
                            spriteBatch.DrawString(fontItem, "Fuel", new Vector2(panelRect.X + 60 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 7 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            spriteBatch.DrawString(fontItem, Main.settings.Fuel ? "ON" : "OFF", new Vector2(panelRect.X + 200 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 7 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            actColor = activeItem == 4 ? yellowColor : greenColor;
                            spriteBatch.DrawString(fontItem, "Sound", new Vector2(panelRect.X + 60 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 9 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            spriteBatch.DrawString(fontItem, Main.settings.Sound ? "ON" : "OFF", new Vector2(panelRect.X + 200 * scale.X, panelRect.Y + (int)(scale.Y * (255 + 9 * (525 - 255) / 12)) - labelSize.Y / 2), actColor, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
                            actColor = activeItem == 5 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Back", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 11 * (525 - 255) / 12)));
                            break;
                        case GameState.MENU_GAME:
                            ShowCenteredLabel(fontHeadline, "Game Menu", greenColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * 220));
                            actColor = activeItem == 0 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Save", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 1 * (525 - 255) / 10)));
                            actColor = activeItem == 1 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Continue", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 3 * (525 - 255) / 10)));
                            actColor = activeItem == 2 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Exit", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 5 * (525 - 255) / 10)));
                            break;
                        case GameState.MENU_SAVE:
                            ShowCenteredLabel(fontHeadline, "Save", greenColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * 220));
                            actColor = activeItem == 0 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot1", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 1 * (525 - 255) / 14)));
                            actColor = activeItem == 1 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot2", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 3 * (525 - 255) / 14)));
                            actColor = activeItem == 2 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot3", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 5 * (525 - 255) / 14)));
                            actColor = activeItem == 3 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot4", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 7 * (525 - 255) / 14)));
                            actColor = activeItem == 4 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot5", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 9 * (525 - 255) / 14)));
                            actColor = activeItem == 5 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Slot6", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 11 * (525 - 255) / 14)));
                            actColor = activeItem == 6 ? yellowColor : greenColor;
                            ShowCenteredLabel(fontItem, "Back", actColor, panelRect.X + panelRect.Width / 2, panelRect.Y + (int)(scale.Y * (255 + 13 * (525 - 255) / 14)));
                            break;
                        default:
                            break;
                    }

                    spriteBatch.End();
                    break;
                default:
                    break;
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
        /// Provede krok animace vysunutí panelu
        /// </summary>
        /// <param name="gameTime">Uběhlý čas od posledního tiku</param>
        private void PanelShow(GameTime gameTime)
        {
            // Zjistíme, kolik času uběhlo od posledního updatu a podle toho nastavíme změnu vysunutí
            accuratePanelPosition -= gameTime.ElapsedGameTime.Milliseconds * panelRect.Width / movingTime;
            if (panelRect.X + panelRect.Width > graphics.Viewport.Width)
            {
                panelRect.X = (int)accuratePanelPosition;

                // Pokud se panel vysunul až příliš, vrátíme jej
                if (panelRect.X + panelRect.Width < graphics.Viewport.Width)
                {
                    panelRect.X = graphics.Viewport.Width - panelRect.Width;
                }
            }
            else
            {
                panelState = MenuPanelBehavior.OUT;
            }
        }

        /// <summary>
        /// Provede krok animace zasunutí panelu
        /// </summary>
        /// <param name="gameTime">Uběhlý čas od posledního tiku</param>
        private void PanelHide(GameTime gameTime)
        {
            // Zjistíme, kolik času uběhlo od posledního updatu a podle toho nastavíme změnu vysunutí
            accuratePanelPosition += gameTime.ElapsedGameTime.Milliseconds * panelRect.Width / movingTime;
            if (panelRect.X < graphics.Viewport.Width)
            {
                panelRect.X = (int)accuratePanelPosition;

                // Pokud se panel schoval až příliš, vrátíme jej
                if (panelRect.X > graphics.Viewport.Width)
                {
                    panelRect.X = graphics.Viewport.Width;
                }
            }
            else
            {
                panelState = MenuPanelBehavior.HIDDEN;
            }
        }

        /// <summary>
        /// Zobrazí text vycentrovaný na zadané pozici
        /// </summary>
        /// <param name="font">Font, který se má použít</param>
        /// <param name="text">Text, který se má zobrazit</param>
        /// <param name="xPos">X souřadnice požadovaného středu</param>
        /// <param name="yPos">Y souřadnice požadovaného středu</param>
        private void ShowCenteredLabel(SpriteFont font, String text, Color color, int xPos, int yPos)
        {
            Vector2 labelSize = font.MeasureString(text);
            labelSize.X *= scale.X;
            labelSize.Y *= scale.Y;
            spriteBatch.DrawString(font, text, new Vector2(xPos - (labelSize.X / 2), yPos - labelSize.Y / 2), color, 0, new Vector2(0, 0), new Vector2(scale.X, scale.Y), new SpriteEffects(), 0);
        }
    }
}
