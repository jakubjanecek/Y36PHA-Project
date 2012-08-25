using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace SkyControl
{
    public class Settings
    {

        private static Settings instance = null;
        private static string FILE = "settings.xml";

        private GameState defaultMode = GameState.GAME_2D;
        private bool storms = true;
        private bool tracePoints = true;
        private bool fuel = true;
        private bool sound = true;

        public GameState DefaultMode { get { return defaultMode; } set { defaultMode = (value == GameState.GAME_2D || value == GameState.GAME_3D ? value : GameState.GAME_2D); } }
        public bool Storms { get { return storms; } set { storms = value; } }
        public bool TracePoints { get { return tracePoints; } set { tracePoints = value; } }
        public bool Fuel { get { return fuel; } set { fuel = value; } }
        public bool Sound { get { return sound; } set { sound = value; } }

        public static Settings getInstance()
        {
            if (instance == null)
            {
                initialize();
            }
            return instance;
        }

        private Settings()
        {
        }

        ~Settings()
        {
            save();
        }

        private static void initialize()
        {
            if (!File.Exists(FILE))
            {
                new Settings().save();
            }
            FileStream fileStream = new FileStream(FILE, FileMode.Open, FileAccess.Read);
            XmlSerializer xs = new XmlSerializer(typeof(Settings));
            instance = (Settings)xs.Deserialize(fileStream);
            fileStream.Close();
        }

        public void save()
        {
            FileStream fileStream = new FileStream(FILE, FileMode.Create, FileAccess.Write);
            XmlSerializer xs = new XmlSerializer(typeof(Settings));
            xs.Serialize(fileStream, this);
            fileStream.Close();
        }
    }
}
