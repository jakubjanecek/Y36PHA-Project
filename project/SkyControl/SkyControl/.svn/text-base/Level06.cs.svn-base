using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public class Level06 : Level
    {
        public void run()
        {
            Main.getInstance().Terrain.Reload("heightmap6", 7.0f, 25.5f);
            //Main.getInstance().AddTerrain(terrain);
            Airport airport1 = new Airport(Main.getInstance(), 7, 23, 2.0, 250);
            airport1.Side = "right";
            Airport airport2 = new Airport(Main.getInstance(), 7, 28, 2.5, 250);
            airport2.Side = "left";
            Exit exit1 = new Exit(Main.getInstance(), 22.0, 0, 180, 0.2);
            Exit exit2 = new Exit(Main.getInstance(), 40, 17.0, 270, 0.2);
            Exit exit3 = new Exit(Main.getInstance(), 0, 0, 135, 0.2);
            Exit exit4 = new Exit(Main.getInstance(), 0, 8, 90, 0.1);
            Plane plane1 = new Boeing(Main.getInstance(), 22.0, 6.0, 5000, 280, 180, exit1.GenerateBoeingCallSign(), 1000.0);
            Plane plane2 = new Airbus(Main.getInstance(), 20.0, 16.5, 3000, 260, 245, exit1.GenerateAirbusCallSign(), 1000.0);
            Plane plane3 = new Cesna(Main.getInstance(), 25.0, 19.5, 4000, 250, 245, exit1.GenerateCesnaCallSign(), 1000.0);
            Main.getInstance().AddAirport(airport1);
            Main.getInstance().AddAirport(airport2);
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
            return 9000;
        }
    }
}
