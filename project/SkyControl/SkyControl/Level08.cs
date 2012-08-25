using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public class Level08 : Level
    {
        public void run()
        {
            Main.getInstance().Terrain.Reload("heightmap8", 20.0f, 15.0f);
            //Main.getInstance().AddTerrain(terrain);
            Airport airport1 = new Airport(Main.getInstance(), 20, 14, 1.5, 90);
            Airport airport2 = new Airport(Main.getInstance(), 20, 16, 2.5, 270);
            Exit exit1 = new Exit(Main.getInstance(), 40, 7, 230, 0.1);
            Exit exit2 = new Exit(Main.getInstance(), 40, 26.0, 270, 0.1);
            Exit exit3 = new Exit(Main.getInstance(), 10.0, 30, 0, 0.1);
            Exit exit4 = new Exit(Main.getInstance(), 0, 23.0, 90, 0.1);
            Exit exit5 = new Exit(Main.getInstance(), 6.0, 0, 120, 0.1);
            Plane plane1 = new Airbus(Main.getInstance(), 5.0, 15.5, 3000, 220, 60, exit1.GenerateAirbusCallSign(), 1000.0);
            Plane plane2 = new Cesna(Main.getInstance(), 7.0, 2.0, 2000, 180, 90, exit1.GenerateCesnaCallSign(), 1000.0);
            Plane plane3 = new Airbus(Main.getInstance(), 27.0, 23.5, 4000, 250, 75, exit1.GenerateAirbusCallSign(), 1000.0);
            Plane plane4 = new Boeing(Main.getInstance(), 10.0, 23.0, 5000, 280, 300, exit1.GenerateBoeingCallSign(), 1000.0);
            Main.getInstance().AddAirport(airport1);
            Main.getInstance().AddAirport(airport2);
            Main.getInstance().AddExit(exit1);
            Main.getInstance().AddExit(exit2);
            Main.getInstance().AddExit(exit3);
            Main.getInstance().AddExit(exit4);
            Main.getInstance().AddExit(exit5);
            Main.getInstance().AddPlane(plane1);
            Main.getInstance().AddPlane(plane2);
            Main.getInstance().AddPlane(plane3);
            Main.getInstance().AddPlane(plane4);
        }

        public int getScore()
        {
            return 13600;
        }
    }
}
