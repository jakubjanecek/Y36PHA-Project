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
    public class Input : Microsoft.Xna.Framework.GameComponent
    {
        private bool mouseChange = true;
        private bool isInRec = false;

        private bool reduceAltChange = false;
        private bool increaseAltChange = false;
        private bool reduceSpeedChange = false;
        private bool increaseSpeedChange = false;
        private bool approachChange = false;
        private bool confirmCommandChange = false;
        private bool cancelCommandChange = false;

        private bool displayModeChange = false;

        private Keys reduceAltKey = Keys.Down;
        private Keys increaseAltKey = Keys.Up;
        private Keys reduceSpeedKey = Keys.PageDown;
        private Keys increaseSpeedKey = Keys.PageUp;
        private Keys reduceDirKey = Keys.Left;
        private Keys increaseDirKey = Keys.Right;
        private Keys approachKey = Keys.Space;
        private Keys confirmCommandKey = Keys.Enter;
        private Keys cancelCommandKey = Keys.Escape;
        private Keys gMenuAccessKey = Keys.Escape;

        private Keys displayModeKey = Keys.M;
        private Keys cameraUpKey = Keys.W;
        private Keys cameraDownKey = Keys.S;
        private Keys cameraLeftKey = Keys.A;
        private Keys cameraRightKey = Keys.D;
        private Keys cameraZoomInKey = Keys.E;
        private Keys cameraZoomOutKey = Keys.Q;

        private bool cheatWeatherChange = false;
        private Keys cheatWeatherKey = Keys.F1;
        private bool cheatNavHelpChange = false;
        private Keys cheatNavHelpKey = Keys.F2;
        private bool cheatNextLevelChange = false;
        private Keys cheatNextLevelKey = Keys.F3;
        private bool cheatNotWorkingChange = false;
        private Keys cheatNotWorkingKey = Keys.F4;
        private bool cheatPlaneCrashChange = false;
        private Keys cheatPlaneCrashKey = Keys.F5;
        private bool cheatFallingPlaneChange = false;
        private Keys cheatFallingPlaneKey = Keys.F6;
        private bool cheatSoundChange = false;
        private Keys cheatSoundKey = Keys.F12;

        public Input(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released && isInRec)
            {
                isInRec = false;
                mouseChange = true;
            }

            if (!Keyboard.GetState().IsKeyDown(reduceAltKey)) { reduceAltChange = true; }
            if (!Keyboard.GetState().IsKeyDown(increaseAltKey)) { increaseAltChange = true; }
            if (!Keyboard.GetState().IsKeyDown(reduceSpeedKey)) { reduceSpeedChange = true; }
            if (!Keyboard.GetState().IsKeyDown(increaseSpeedKey)) { increaseSpeedChange = true; }
            if (!Keyboard.GetState().IsKeyDown(approachKey)) { approachChange = true; }
            if (!Keyboard.GetState().IsKeyDown(confirmCommandKey)) { confirmCommandChange = true; }
            if (!Keyboard.GetState().IsKeyDown(cancelCommandKey)) { cancelCommandChange = true; }

            if (!Keyboard.GetState().IsKeyDown(displayModeKey)) { displayModeChange = true; }

            if (!Keyboard.GetState().IsKeyDown(cheatWeatherKey)) { cheatWeatherChange = true; }
            if (!Keyboard.GetState().IsKeyDown(cheatNavHelpKey)) { cheatNavHelpChange = true; }
            if (!Keyboard.GetState().IsKeyDown(cheatNextLevelKey)) { cheatNextLevelChange = true; }
            if (!Keyboard.GetState().IsKeyDown(cheatNotWorkingKey)) { cheatNotWorkingChange = true; }
            if (!Keyboard.GetState().IsKeyDown(cheatPlaneCrashKey)) { cheatPlaneCrashChange = true; }
            if (!Keyboard.GetState().IsKeyDown(cheatFallingPlaneKey)) { cheatFallingPlaneChange = true; }
            if (!Keyboard.GetState().IsKeyDown(cheatSoundKey)) { cheatSoundChange = true; }

            base.Update(gameTime);
        }


        /*****************************************************************
         * ***************************************************************
         * 
         *                      VLASTNÍ METODY
         *
         * ***************************************************************
         * ***************************************************************/

        /// <summary>
        /// Zjistí, je-li ukazatel myši v zadané oblasti obrazovky.
        /// </summary>
        /// <param name="x">Výchozí bod dotazované oblasti</param>
        /// <param name="y">Výchozí bod dotazované oblasti</param>
        /// <param name="width">Šířka dotazované oblasti</param>
        /// <param name="height">Výška dotazované oblasti</param>
        /// <returns>TRUE, pokud se kurzor myši nachází v dotazované oblasti, jinak FALSE</returns>
        public bool IsMouseInBox(int x, int y, int width, int height)
        {
            return (Mouse.GetState().X >= x && Mouse.GetState().X <= x + width - 1 &&
                    Mouse.GetState().Y >= y && Mouse.GetState().Y <= y + height - 1);
        }

        /// <summary>
        /// Zjistí, je-li ukazatel myši v zadané oblasti obrazovky.
        /// </summary>
        /// <param name="pos">Výchozí bod dotazované oblasti</param>
        /// <param name="size">Velikost dotazované oblasti</param>
        /// <returns>TRUE, pokud se kurzor myši nachází v dotazované oblasti, jinak FALSE</returns>
        public bool IsMouseInBox(Vector2 pos, Vector2 size)
        {
            return IsMouseInBox((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        /// <summary>
        /// Zjistí, jestli uživatel vybral položku Menu
        /// </summary>
        /// <param name="x">Výchozí bod obdélníku položky</param>
        /// <param name="y">Výchozí bod obdélníku položky</param>
        /// <param name="width">Šířka obdélníku položky</param>
        /// <param name="height">Výška obdélníku položky</param>
        /// <returns>TRUE, pokud uživatel položku vybral, jinak FALSE</returns>
        public bool SelectMenuItem(int x, int y, int width, int height)
        {
            bool ret = IsMouseInBox(x, y, width, height) && Mouse.GetState().LeftButton == ButtonState.Pressed && mouseChange;
            if (ret)
            {
                isInRec = true;
                mouseChange = false;
            }
            return ret;
        }

        /// <summary>
        /// Zjistí, jestli uživatel vybral položku Menu
        /// </summary>
        /// <param name="pos">Výchozí bod obdélníku položky</param>
        /// <param name="size">Velikost obdélníku položky</param>
        /// <returns>TRUE, pokud uživatel položku vybral, jinak FALSE</returns>
        public bool SelectMenuItem(Vector2 pos, Vector2 size)
        {
            return SelectMenuItem((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }




        public bool SelectPlane(Plane plane)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                ((Main.state.CurrentState == GameState.GAME_2D && Math.Abs(plane.PixelX - Mouse.GetState().X) <= 4 && Math.Abs(plane.PixelY - Mouse.GetState().Y) <= 4) ||
                (Main.state.CurrentState == GameState.GAME_3D && Math.Abs(Main.getInstance().Transfer3to2d(plane.Get3dLocation()).X - Mouse.GetState().X) <= 10 && Math.Abs(Main.getInstance().Transfer3to2d(plane.Get3dLocation()).Y - Mouse.GetState().Y) <= 10)))
            {
                return true;
            }
            else return false;
        }


        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz ke snížení výšky
         * 
         * x, y - souřadnice ovládacího tlačítka HUDu na obrazovce
         * ***********************************************************/
        public bool ReduceAlt(int x, int y)
        {
            bool mouseRet = IsMouseInBox(x - 5, y - 5, 10, 10) && Mouse.GetState().LeftButton == ButtonState.Pressed && mouseChange;
            if (mouseRet)
            {
                isInRec = true;
                mouseChange = false;
            }
            bool keyboardRet = Keyboard.GetState().IsKeyDown(reduceAltKey) && reduceAltChange;
            if (keyboardRet)
            {
                reduceAltChange = false;
            }
            return mouseRet || keyboardRet;
        }

        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz ke zvýšení výšky
         * 
         * x, y - souřadnice ovládacího tlačítka HUDu na obrazovce
         * ***********************************************************/
        public bool IncreaseAlt(int x, int y)
        {
            bool mouseRet = IsMouseInBox(x - 5, y - 5, 10, 10) && Mouse.GetState().LeftButton == ButtonState.Pressed && mouseChange;
            if (mouseRet)
            {
                isInRec = true;
                mouseChange = false;
            }
            bool keyboardRet = Keyboard.GetState().IsKeyDown(increaseAltKey) && increaseAltChange;
            if (keyboardRet)
            {
                increaseAltChange = false;
            }
            return mouseRet || keyboardRet;
        }

        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz ke snížení rychlosti
         * 
         * x, y - souřadnice ovládacího tlačítka HUDu na obrazovce
         * ***********************************************************/
        public bool ReduceSpeed(int x, int y)
        {
            bool mouseRet = IsMouseInBox(x - 5, y - 5, 10, 10) && Mouse.GetState().LeftButton == ButtonState.Pressed && mouseChange;
            if (mouseRet)
            {
                isInRec = true;
                mouseChange = false;
            }
            bool keyboardRet = Keyboard.GetState().IsKeyDown(reduceSpeedKey) && reduceSpeedChange;
            if (keyboardRet)
            {
                reduceSpeedChange = false;
            }
            return mouseRet || keyboardRet;
        }

        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz ke zvýšení rychlosti
         * 
         * x, y - souřadnice ovládacího tlačítka HUDu na obrazovce
         * ***********************************************************/
        public bool IncreaseSpeed(int x, int y)
        {
            bool mouseRet = IsMouseInBox(x - 5, y - 5, 10, 10) && Mouse.GetState().LeftButton == ButtonState.Pressed && mouseChange;
            if (mouseRet)
            {
                isInRec = true;
                mouseChange = false;
            }
            bool keyboardRet = Keyboard.GetState().IsKeyDown(increaseSpeedKey) && increaseSpeedChange;
            if (keyboardRet)
            {
                increaseSpeedChange = false;
            }
            return mouseRet || keyboardRet;
        }

        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz k otočení letadla doleva
         * ***********************************************************/
        public bool ReduceDir()
        {
            return Keyboard.GetState().IsKeyDown(reduceDirKey);
        }

        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz k otočení letadla doprava
         * ***********************************************************/
        public bool IncreaseDir()
        {
            return Keyboard.GetState().IsKeyDown(increaseDirKey);
        }

        /*************************************************************
         * Zjistí, jestli uživatel mění kurz pomocí myši, nebo ne
         * x,y - souřadnice ovladače kurzu
         * d - průměr ovladače kurzu
         * ***********************************************************/
        public bool CourseChange(int x, int y, int d)
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed && IsMouseInBox(x, y, d, d);
        }

        /*************************************************************
         * Vrátí relativní pozici kurzoru od zadaného bodu.
         * ***********************************************************/
        public Vector2 GetRelativeCursorPos(int referenceX, int referenceY)
        {
            return new Vector2(Mouse.GetState().X - referenceX, Mouse.GetState().Y - referenceY);
        }

        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz APPROACH
         * 
         * x, y - souřadnice ovládacího tlačítka HUDu na obrazovce
         * width, height - rozměry ovládacího tlačítka na obrazovce
         * ***********************************************************/
        public bool Approach(int x, int y, int width, int height)
        {
            bool mouseRet = IsMouseInBox(x, y, width, height) && Mouse.GetState().LeftButton == ButtonState.Pressed && mouseChange;
            if (mouseRet)
            {
                isInRec = true;
                mouseChange = false;
            }
            bool keyboardRet = Keyboard.GetState().IsKeyDown(approachKey) && approachChange;
            if (keyboardRet)
            {
                approachChange = false;
            }
            return mouseRet || keyboardRet;
        }

        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz k potvrzení parametrů
         * 
         * x, y - souřadnice ovládacího tlačítka HUDu na obrazovce
         * width, height - rozměry ovládacího tlačítka na obrazovce
         * ***********************************************************/
        public bool ConfirmCommand(int x, int y, int width, int height)
        {
            bool mouseRet = IsMouseInBox(x, y, width, height) && Mouse.GetState().LeftButton == ButtonState.Pressed && mouseChange;
            if (mouseRet)
            {
                isInRec = true;
                mouseChange = false;
            }
            bool keyboardRet = Keyboard.GetState().IsKeyDown(confirmCommandKey) && confirmCommandChange;
            if (keyboardRet)
            {
                confirmCommandChange = false;
            }
            return mouseRet || keyboardRet;
        }

        /*************************************************************
         * Zjistí, jestli uživatel zadal příkaz ke zrušení parametrů
         * 
         * x, y - souřadnice ovládacího tlačítka HUDu na obrazovce
         * width, height - rozměry ovládacího tlačítka na obrazovce
         * ***********************************************************/
        public bool CancelCommand(int x, int y, int width, int height)
        {
            bool mouseRet = IsMouseInBox(x, y, width, height) && Mouse.GetState().LeftButton == ButtonState.Pressed && mouseChange;
            if (mouseRet)
            {
                isInRec = true;
                mouseChange = false;
            }
            bool keyboardRet = Keyboard.GetState().IsKeyDown(cancelCommandKey) && cancelCommandChange;
            if (keyboardRet)
            {
                cancelCommandChange = false;
            }
            return mouseRet || keyboardRet;
        }

        /*************************************************************
         * Zjisti, jestli uzivatel na neco kliknul.
         * Pouziva se pri gameover na navrat do menu.
         * ***********************************************************/
        public bool Any()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed || Keyboard.GetState().GetPressedKeys().Length > 0)
            {
                return true;
            }
            return false;
        }

        /*************************************************************
         * Zjisti, jestli uzivatel přistupuje ke Game Menu.
         * ***********************************************************/
        public bool AccessedGameMenu()
        {
            return Keyboard.GetState().IsKeyDown(gMenuAccessKey) && cancelCommandChange;
        }

        /*************************************************************
         * Zjistí, pohybuje-li uživatel s kamerou
         * ***********************************************************/
        public bool CameraUp()
        {
            return Keyboard.GetState().IsKeyDown(cameraUpKey);
        }
        public bool CameraDown()
        {
            return Keyboard.GetState().IsKeyDown(cameraDownKey);
        }
        public bool CameraLeft()
        {
            return Keyboard.GetState().IsKeyDown(cameraLeftKey);
        }
        public bool CameraRight()
        {
            return Keyboard.GetState().IsKeyDown(cameraRightKey);
        }
        public bool CameraZoomIn()
        {
            return Keyboard.GetState().IsKeyDown(cameraZoomInKey);
        }
        public bool CameraZoomOut()
        {
            return Keyboard.GetState().IsKeyDown(cameraZoomOutKey);
        }

        /*************************************************************
         * Zjistí, jesli uživatel přepíná mód zobrazení (2D-3D)
         * ***********************************************************/
        public bool SwitchDisplayMode()
        {
            bool ret = Keyboard.GetState().IsKeyDown(displayModeKey) && this.displayModeChange;
            if (ret)
            {
                displayModeChange = false;
            }
            return ret;
        }

        public bool cheatWeather()
        {
            bool ret = Keyboard.GetState().IsKeyDown(cheatWeatherKey) && cheatWeatherChange;
            if (ret)
            {
                cheatWeatherChange = false;
            }
            return ret;
        }

        public bool cheatNavHelp()
        {
            bool ret = Keyboard.GetState().IsKeyDown(cheatNavHelpKey) && cheatNavHelpChange;
            if (ret)
            {
                cheatNavHelpChange = false;
            }
            return ret;
        }

        public bool cheatNextLevel()
        {
            bool ret = Keyboard.GetState().IsKeyDown(cheatNextLevelKey) && cheatNextLevelChange;
            if (ret)
            {
                cheatNextLevelChange = false;
            }
            return ret;
        }

        public bool cheatNotWorking()
        {
            bool ret = Keyboard.GetState().IsKeyDown(cheatNotWorkingKey) && cheatNotWorkingChange;
            if (ret)
            {
                cheatNotWorkingChange = false;
            }
            return ret;
        }

        public bool cheatPlaneCrash()
        {
            bool ret = Keyboard.GetState().IsKeyDown(cheatPlaneCrashKey) && cheatPlaneCrashChange;
            if (ret)
            {
                cheatPlaneCrashChange = false;
            }
            return ret;
        }

        public bool cheatFallingPlane()
        {
            bool ret = Keyboard.GetState().IsKeyDown(cheatFallingPlaneKey) && cheatFallingPlaneChange;
            if (ret)
            {
                cheatFallingPlaneChange = false;
            }
            return ret;
        }

        public bool cheatSound()
        {
            bool ret = Keyboard.GetState().IsKeyDown(cheatSoundKey) && cheatSoundChange;
            if (ret)
            {
                cheatSoundChange = false;
            }
            return ret;
        }
    }
}