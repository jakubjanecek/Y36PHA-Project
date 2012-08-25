using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public class Level04 : Level
    {
        public void run()
        {
            Main.getInstance().Terrain.Reload("heightmap4", 37, 11.5f);
            //Main.getInstance().AddTerrain(terrain);
            Airport airport1 = new Airport(Main.getInstance(), 37, 10, 2, 90);
            airport1.Side = "left";
            Airport airport2 = new Airport(Main.getInstance(), 37, 13, 2.5, 90);
            airport2.Side = "right";
            Exit exit1 = new Exit(Main.getInstance(), 40, 25.0, 270, 0.2);
            Exit exit2 = new Exit(Main.getInstance(), 0, 10, 90, 0.2);
            Exit exit3 = new Exit(Main.getInstance(), 30.0, 0, 200, 0.2);
            Plane plane1 = new Boeing(Main.getInstance(), 31.0, 25.0, 6000, 300, 270, exit1.GenerateBoeingCallSign(), 1000.0);
            Plane plane2 = new Boeing(Main.getInstance(), 19.0, 25.0, 5000, 280, 270, exit1.GenerateBoeingCallSign(), 1000.0);
            Plane plane3 = new Airbus(Main.getInstance(), 14.0, 4.0, 4500, 250, 270, exit1.GenerateAirbusCallSign(), 1000.0);
            Main.getInstance().AddAirport(airport1);
            Main.getInstance().AddAirport(airport2);
            Main.getInstance().AddExit(exit1);
            Main.getInstance().AddExit(exit2);
            Main.getInstance().AddExit(exit3);
            Main.getInstance().AddPlane(plane1);
            Main.getInstance().AddPlane(plane2);
            Main.getInstance().AddPlane(plane3);
        }

        public int getScore()
        {
            return 5200;
        }
    }
}
