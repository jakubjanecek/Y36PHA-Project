using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public class Level05 : Level
    {
        public void run()
        {
            Main.getInstance().Terrain.Reload("heightmap5", 3.0f, 3.0f);
            //Main.getInstance().AddTerrain(terrain);
            Airport airport = new Airport(Main.getInstance(), 3.0, 3, 2.5, 315);
            Exit exit1 = new Exit(Main.getInstance(), 22.0, 0, 180, 0.2);
            Exit exit2 = new Exit(Main.getInstance(), 40, 17.0, 270, 0.2);
            Exit exit3 = new Exit(Main.getInstance(), 10.0, 30, 0, 0.1);
            Exit exit4 = new Exit(Main.getInstance(), 0, 8, 90, 0.1);
            Plane plane1 = new Boeing(Main.getInstance(), 22.0, 6.0, 5000, 280, 180, exit1.GenerateBoeingCallSign(), 1000.0);
            Plane plane2 = new Airbus(Main.getInstance(), 20.0, 17.0, 3000, 260, 290, exit1.GenerateAirbusCallSign(), 1000.0);
            Plane plane3 = new Cesna(Main.getInstance(), 25.0, 20.0, 4000, 250, 290, exit1.GenerateCesnaCallSign(), 1000.0);
            Main.getInstance().AddAirport(airport);
            Main.getInstance().AddExit(exit1);
            Main.getInstance().AddExit(exit2);
            Main.getInstance().AddExit(exit3);
            Main.getInstance().AddExit(exit4);
            Main.getInstance().AddPlane(plane1);
            Main.getInstance().AddPlane(plane2);
            Main.getInstance().AddPlane(plane3);
        }

        public int getScore()
        {
            return 7000;
        }
    }
}
