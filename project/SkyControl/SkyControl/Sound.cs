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
    public class Sound : Microsoft.Xna.Framework.GameComponent
    {
        private ContentManager content;
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        private Dictionary<string, int> soundDelays = new Dictionary<string, int>();
        private List<string> keysToPlay = new List<string>();
        private List<string> prefKeysToPlay = new List<string>();
        private List<string> nonEssentialKeysToPlay = new List<string>();
        private int delay = 0;

        public Sound(Game game)
            : base(game)
        {
            content = game.Content;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            sounds.Add("0", content.Load<SoundEffect>("0"));
            sounds.Add("1", content.Load<SoundEffect>("1"));
            sounds.Add("2", content.Load<SoundEffect>("2"));
            sounds.Add("3", content.Load<SoundEffect>("3"));
            sounds.Add("4", content.Load<SoundEffect>("4"));
            sounds.Add("5", content.Load<SoundEffect>("5"));
            sounds.Add("6", content.Load<SoundEffect>("6"));
            sounds.Add("7", content.Load<SoundEffect>("7"));
            sounds.Add("8", content.Load<SoundEffect>("8"));
            sounds.Add("9", content.Load<SoundEffect>("9"));
            sounds.Add(" ", content.Load<SoundEffect>("_"));
            sounds.Add("a", content.Load<SoundEffect>("a"));
            sounds.Add("b", content.Load<SoundEffect>("b"));
            sounds.Add("c", content.Load<SoundEffect>("c"));
            sounds.Add("d", content.Load<SoundEffect>("d"));
            sounds.Add("e", content.Load<SoundEffect>("e"));
            sounds.Add("i", content.Load<SoundEffect>("i"));
            sounds.Add("l", content.Load<SoundEffect>("l"));
            sounds.Add("n", content.Load<SoundEffect>("n"));
            sounds.Add("o", content.Load<SoundEffect>("o"));
            sounds.Add("r", content.Load<SoundEffect>("r"));
            sounds.Add("s", content.Load<SoundEffect>("s"));
            sounds.Add("t", content.Load<SoundEffect>("t"));

            sounds.Add("heading", content.Load<SoundEffect>("heading"));
            sounds.Add("climb", content.Load<SoundEffect>("climb"));
            sounds.Add("descent", content.Load<SoundEffect>("descent"));
            sounds.Add("to", content.Load<SoundEffect>("to"));
            sounds.Add("flight", content.Load<SoundEffect>("flight"));
            sounds.Add("level", content.Load<SoundEffect>("level"));
            sounds.Add("speed", content.Load<SoundEffect>("speed"));
            sounds.Add("feet", content.Load<SoundEffect>("feet"));
            sounds.Add("000", content.Load<SoundEffect>("thousand"));
            sounds.Add("00", content.Load<SoundEffect>("hundred"));
            sounds.Add("space", content.Load<SoundEffect>("space"));
            sounds.Add("request", content.Load<SoundEffect>("request"));
            sounds.Add("land", content.Load<SoundEffect>("land"));
            sounds.Add("fuel10", content.Load<SoundEffect>("fuel10"));
            sounds.Add("fuel5", content.Load<SoundEffect>("fuel5"));
            sounds.Add("mayday", content.Load<SoundEffect>("mayday"));
            sounds.Add("left", content.Load<SoundEffect>("left"));
            sounds.Add("right", content.Load<SoundEffect>("right"));
            sounds.Add("around", content.Load<SoundEffect>("goingaround"));
            sounds.Add("approach", content.Load<SoundEffect>("approach"));

            sounds.Add("disp_0", content.Load<SoundEffect>("disp_0"));
            sounds.Add("disp_1", content.Load<SoundEffect>("disp_1"));
            sounds.Add("disp_2", content.Load<SoundEffect>("disp_2"));
            sounds.Add("disp_3", content.Load<SoundEffect>("disp_3"));
            sounds.Add("disp_4", content.Load<SoundEffect>("disp_4"));
            sounds.Add("disp_5", content.Load<SoundEffect>("disp_5"));
            sounds.Add("disp_6", content.Load<SoundEffect>("disp_6"));
            sounds.Add("disp_7", content.Load<SoundEffect>("disp_7"));
            sounds.Add("disp_8", content.Load<SoundEffect>("disp_8"));
            sounds.Add("disp_9", content.Load<SoundEffect>("disp_9"));
            sounds.Add("disp_", content.Load<SoundEffect>("_"));
            sounds.Add("disp_ ", content.Load<SoundEffect>("_"));
            sounds.Add("disp_a", content.Load<SoundEffect>("disp_a"));
            sounds.Add("disp_b", content.Load<SoundEffect>("disp_b"));
            sounds.Add("disp_c", content.Load<SoundEffect>("disp_c"));
            sounds.Add("disp_d", content.Load<SoundEffect>("disp_d"));
            sounds.Add("disp_e", content.Load<SoundEffect>("disp_e"));
            sounds.Add("disp_i", content.Load<SoundEffect>("disp_i"));
            sounds.Add("disp_l", content.Load<SoundEffect>("disp_l"));
            sounds.Add("disp_n", content.Load<SoundEffect>("disp_n"));
            sounds.Add("disp_o", content.Load<SoundEffect>("disp_o"));
            sounds.Add("disp_r", content.Load<SoundEffect>("disp_r"));
            sounds.Add("disp_s", content.Load<SoundEffect>("disp_s"));
            sounds.Add("disp_t", content.Load<SoundEffect>("disp_t"));

            sounds.Add("disp_approach", content.Load<SoundEffect>("disp_approach"));
            sounds.Add("disp_course", content.Load<SoundEffect>("disp_course"));
            sounds.Add("disp_climb", content.Load<SoundEffect>("disp_climb"));
            sounds.Add("disp_descent", content.Load<SoundEffect>("disp_descent"));
            sounds.Add("disp_feets", content.Load<SoundEffect>("disp_feets"));
            sounds.Add("disp_knots", content.Load<SoundEffect>("disp_knots"));
            sounds.Add("disp_speed", content.Load<SoundEffect>("disp_speed"));
            sounds.Add("disp_left", content.Load<SoundEffect>("disp_left"));
            sounds.Add("disp_right", content.Load<SoundEffect>("disp_right"));
            sounds.Add("disp_000", content.Load<SoundEffect>("disp_thousand"));
            sounds.Add("disp_00", content.Load<SoundEffect>("disp_hundred"));

            sounds.Add("t_0", content.Load<SoundEffect>("t_0"));
            sounds.Add("t_1", content.Load<SoundEffect>("t_1"));
            sounds.Add("t_2", content.Load<SoundEffect>("t_2"));
            sounds.Add("t_3", content.Load<SoundEffect>("t_3"));
            sounds.Add("t_4", content.Load<SoundEffect>("t_4"));
            sounds.Add("t_5", content.Load<SoundEffect>("t_5"));
            sounds.Add("t_6", content.Load<SoundEffect>("t_6"));
            sounds.Add("t_7", content.Load<SoundEffect>("t_7"));
            sounds.Add("t_8", content.Load<SoundEffect>("t_8"));
            sounds.Add("t_9", content.Load<SoundEffect>("t_9"));
            sounds.Add("t_a", content.Load<SoundEffect>("t_a"));
            sounds.Add("t_b", content.Load<SoundEffect>("t_b"));
            sounds.Add("t_c", content.Load<SoundEffect>("t_c"));
            sounds.Add("t_d", content.Load<SoundEffect>("t_d"));
            sounds.Add("t_e", content.Load<SoundEffect>("t_e"));
            sounds.Add("t_i", content.Load<SoundEffect>("t_i"));
            sounds.Add("t_l", content.Load<SoundEffect>("t_l"));
            sounds.Add("t_n", content.Load<SoundEffect>("t_n"));
            sounds.Add("t_o", content.Load<SoundEffect>("t_o"));
            sounds.Add("t_r", content.Load<SoundEffect>("t_r"));
            sounds.Add("t_s", content.Load<SoundEffect>("t_s"));
            sounds.Add("t_t", content.Load<SoundEffect>("t_t"));
            sounds.Add("t_", content.Load<SoundEffect>("_"));
            sounds.Add("t_ ", content.Load<SoundEffect>("_"));

            sounds.Add("t_cleared", content.Load<SoundEffect>("t_cleared"));
            sounds.Add("t_cancel", content.Load<SoundEffect>("t_cancel"));
            sounds.Add("t_left", content.Load<SoundEffect>("t_left"));
            sounds.Add("t_right", content.Load<SoundEffect>("t_right"));
            sounds.Add("t_toobig", content.Load<SoundEffect>("t_toobig"));
            sounds.Add("t_runway", content.Load<SoundEffect>("t_runway"));
            sounds.Add("t_runwayout", content.Load<SoundEffect>("t_runwayout"));
            sounds.Add("t_runwayback", content.Load<SoundEffect>("t_runwayback"));

            sounds.Add("alarm", content.Load<SoundEffect>("alarm"));

            soundDelays.Add(" ", 0);
            soundDelays.Add("space", 200);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (Main.settings.Sound)
            {
                if (delay < 0)
                {
                    if (prefKeysToPlay.Count > 0)
                    {
                        string key = prefKeysToPlay[0];
                        prefKeysToPlay.RemoveAt(0);
                        sounds[key.ToLower()].Play();
                        delay = sounds[key.ToLower()].Duration.Milliseconds + sounds[key.ToLower()].Duration.Seconds * 1000 + sounds[key.ToLower()].Duration.Minutes * 60000;
                    }
                    else if (keysToPlay.Count > 0)
                    {
                        string key = keysToPlay[0];
                        keysToPlay.RemoveAt(0);
                        sounds[key.ToLower()].Play();
                        delay = sounds[key.ToLower()].Duration.Milliseconds + sounds[key.ToLower()].Duration.Seconds * 1000 + sounds[key.ToLower()].Duration.Minutes * 60000;
                        if (soundDelays.ContainsKey(key.ToLower()))
                        {
                            delay = soundDelays[key.ToLower()];
                        }
                    }
                    else if (nonEssentialKeysToPlay.Count > 0)
                    {
                        string key = nonEssentialKeysToPlay[0];
                        nonEssentialKeysToPlay.RemoveAt(0);
                        sounds[key.ToLower()].Play();
                        delay = sounds[key.ToLower()].Duration.Milliseconds + sounds[key.ToLower()].Duration.Seconds * 1000 + sounds[key.ToLower()].Duration.Minutes * 60000;
                        if (soundDelays.ContainsKey(key.ToLower()))
                        {
                            delay = soundDelays[key.ToLower()];
                        }
                    }
                }
                else
                {
                    delay -= gameTime.ElapsedGameTime.Milliseconds;
                }
            }
            base.Update(gameTime);
        }

        public void play(string key)
        {
            if (Main.settings.Sound)
            {
                keysToPlay.Add(key);
                nonEssentialKeysToPlay.Clear();
            }
        }

        public void playSequence(string key)
        {
            if (Main.settings.Sound)
            {
                foreach (char ch in key)
                {
                    keysToPlay.Add(new String(ch, 1));
                }
                nonEssentialKeysToPlay.Clear();
            }
        }

        public void playDispSequence(string key)
        {
            if (Main.settings.Sound)
            {
                foreach (char ch in key)
                {
                    keysToPlay.Add("disp_" + new String(ch, 1));
                }
                nonEssentialKeysToPlay.Clear();
            }
        }

        public void playTowerSequence(string key)
        {
            if (Main.settings.Sound)
            {
                foreach (char ch in key)
                {
                    keysToPlay.Add("t_" + new String(ch, 1));
                }
                nonEssentialKeysToPlay.Clear();
            }
        }

        public void playPreferred(string key)
        {
            if (Main.settings.Sound)
            {
                prefKeysToPlay.Add(key);
                nonEssentialKeysToPlay.Clear();
            }
        }

        public void playNonEssential(string key)
        {
            if (Main.settings.Sound)
            {
                nonEssentialKeysToPlay.Add(key);
            }
        }

        public void playNonEssentialSequence(string key)
        {
            if (Main.settings.Sound)
            {
                foreach (char ch in key)
                {
                    nonEssentialKeysToPlay.Add(new String(ch, 1));
                }
            }
        }

        /*****************************************************
         * Vrací, kolik milisekund je již vloženo do zvukového
         * bufferu.
         * ***************************************************/
        public int GetFilledTime()
        {
            int totalLength = 0;

            foreach (String key in prefKeysToPlay)
            {
                totalLength += sounds[key.ToLower()].Duration.Milliseconds + sounds[key.ToLower()].Duration.Seconds * 1000 + sounds[key.ToLower()].Duration.Minutes * 60000;
            }
            foreach (String key in keysToPlay)
            {
                totalLength += sounds[key.ToLower()].Duration.Milliseconds + sounds[key.ToLower()].Duration.Seconds * 1000 + sounds[key.ToLower()].Duration.Minutes * 60000;
            }

            return totalLength;
        }

        public void Clear()
        {
            keysToPlay.Clear();
            prefKeysToPlay.Clear();
            nonEssentialKeysToPlay.Clear();
        }
    }
}