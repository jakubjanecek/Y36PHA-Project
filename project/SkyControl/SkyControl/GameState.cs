using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace SkyControl
{
    public enum GameState
    {
        INTRO,
        MENU_MAIN,
        MENU_LOAD,
        MENU_SETUP,
        MENU_GAME,
        MENU_SAVE,
        GAME_2D,
        GAME_3D,
        GAMEOVER,
        NEXT_LEVEL,
        CONGRATS
    }

    public class State
    {
        private static State instance = new State();
        private GameState state = GameState.INTRO;
        private int score = 0;
        private int mistakes = 0;
        private int level = 1;

        public GameState CurrentState { get { return state; } set { state = value; } }
        public int Score { get { return score; } set { score = value; } }
        public int Mistakes { get { return mistakes; } set { mistakes = value; } }
        public int Level { get { return level; } set { level = value; } }

        private State()
        {
        }

        public static State getInstance()
        {
            return instance;
        }

        public void save(string filename)
        {
            FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
            XmlSerializer xs = new XmlSerializer(typeof(State));
            xs.Serialize(fileStream, this);
            fileStream.Close();
        }

        public void load(string filename)
        {
            if (File.Exists(filename))
            {
                FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                XmlSerializer xs = new XmlSerializer(typeof(State));
                State loaded = (State)xs.Deserialize(fileStream);
                loaded.CurrentState = Main.settings.DefaultMode;
                instance = loaded;
                Main.state = instance;
                Main.getInstance().levels[level - 1].run();
                fileStream.Close();
            }
        }

        public void mistake()
        {
            mistakes++;
            if (mistakes >= 3)
            {
                this.CurrentState = GameState.GAMEOVER;
            }
        }

        public void changeScore(int val)
        {
            score += val;
            if (score >= Main.getInstance().levels[level - 1].getScore())
            {
                level++;
                if (level > 10)
                {
                    this.CurrentState = GameState.CONGRATS;
                }
                else
                {
                    Main.getInstance().lastState = this.CurrentState;
                    this.CurrentState = GameState.NEXT_LEVEL;
                }
            }
        }

        public void resetScore()
        {
            score = 0;
        }

        public void resetMistakes()
        {
            mistakes = 0;
        }

        public void resetLevel()
        {
            level = 1;
        }

    }
}