using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public class Level02 : Level
    {
        public void run()
        {
            Main.getInstance().Terrain.Reload("heightmap2", 3, 15);
            //Main.getInstance().AddTerrain(terrain);
            Airport airport = new Airport(Main.getInstance(), 3, 15, 2.5, 270);
            Exit exit1 = new Exit(Main.getInstance(), 40.0, 25.0, 270, 0.3);
            Exit exit2 = new Exit(Main.getInstance(), 30.0, 0, 180, 0.3);
            Plane plane = new Boeing(Main.getInstance(), 31.0, 25.0, 3500, 240, 270, exit1.GenerateBoeingCallSign(), 1000.0);
            Main.getInstance().AddAirport(airport);
            Main.getInstance().AddExit(exit1);
            Main.getInstance().AddExit(exit2);
            Main.getInstance().AddPlane(plane);
        }

        public int getScore()
        {
            return 2200;
        }
    }
}
