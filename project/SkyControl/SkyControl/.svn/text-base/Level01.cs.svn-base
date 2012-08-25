using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public class Level01 : Level
    {
        public void run()
        {
            Main.getInstance().Terrain.Reload("heightmap1", 6.6f, 5.2f);
            //Main.getInstance().AddTerrain(terrain);
            Airport airport = new Airport(Main.getInstance(), 6.6, 5.2, 2.5, 300);
            Main.getInstance().AddAirport(airport);
            Exit exit1 = new Exit(Main.getInstance(), 10, 30, 22.0, 0.5);
            Exit exit2 = new Exit(Main.getInstance(), 40, 10, 250.0, 0.1);
            Main.getInstance().AddExit(exit1);
            Main.getInstance().AddExit(exit2);
            Plane plane1 = new Boeing(Main.getInstance(), 13.0, 25.0, 4000, 280, 30, exit1.GenerateBoeingCallSign(), 1000.0);
            Plane plane2 = new Airbus(Main.getInstance(), 15.0, 10.0, 2500, 180, 300, exit1.GenerateAirbusCallSign(), 630.0);
            Main.getInstance().AddPlane(plane1);
            Main.getInstance().AddPlane(plane2);
        }

        public int getScore()
        {
            return 1000;
        }
    }
}
