using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public class Level03 : Level
    {
        public void run()
        {
            Main.getInstance().Terrain.Reload("heightmap3", 3, 5.5f);
            //Main.getInstance().AddTerrain(terrain);
            Airport airport = new Airport(Main.getInstance(), 3, 5.5, 2.5, 300);
            Exit exit1 = new Exit(Main.getInstance(), 40.0, 25.0, 270, 0.2);
            Exit exit2 = new Exit(Main.getInstance(), 5.0, 0, 115, 0.2);
            Exit exit3 = new Exit(Main.getInstance(), 30.0, 0, 180, 0.1);
            Plane plane1 = new Boeing(Main.getInstance(), 31.0, 25.0, 3500, 240, 270, exit1.GenerateBoeingCallSign(), 1000.0);
            Plane plane2 = new Cesna(Main.getInstance(), 30.0, 5.0, 3000, 200, 180, exit1.GenerateCesnaCallSign(), 1000.0);
            Main.getInstance().AddAirport(airport);
            Main.getInstance().AddExit(exit1);
            Main.getInstance().AddExit(exit2);
            Main.getInstance().AddExit(exit3);
            Main.getInstance().AddPlane(plane1);
            Main.getInstance().AddPlane(plane2);
        }

        public int getScore()
        {
            return 3600;
        }
    }
}
