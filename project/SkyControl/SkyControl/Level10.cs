using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyControl
{
    public class Level10 : Level
    {
        public void run()
        {
            Main.getInstance().Terrain.Reload("heightmap10", 37.25f, 6.5f);
            //Main.getInstance().AddTerrain(terrain);
            Airport airport1 = new Airport(Main.getInstance(), 37, 5.5, 2, 90);
            Airport airport2 = new Airport(Main.getInstance(), 37, 6, 2.5, 60);
            airport2.Side = "left";
            Airport airport3 = new Airport(Main.getInstance(), 37.5, 8, 1.5, 60);
            airport3.Side = "right";
            Exit exit1 = new Exit(Main.getInstance(), 40, 12, 230, 0.1);
            Exit exit2 = new Exit(Main.getInstance(), 40, 26.0, 270, 0.1);
            Exit exit3 = new Exit(Main.getInstance(), 10.0, 30, 0, 0.1);
            Exit exit4 = new Exit(Main.getInstance(), 0, 23.0, 90, 0.1);
            Exit exit5 = new Exit(Main.getInstance(), 6.0, 0, 120, 0.1);
            Exit exit6 = new Exit(Main.getInstance(), 0, 13, 90, 0.1);
            Plane plane1 = new Airbus(Main.getInstance(), 5.0, 13.0, 3000, 220, 60, exit1.GenerateAirbusCallSign(), 1000.0);
            Plane plane2 = new Cesna(Main.getInstance(), 7.0, 2.0, 2000, 180, 90, exit1.GenerateCesnaCallSign(), 1000.0);
            Plane plane3 = new Airbus(Main.getInstance(), 25.0, 16.0, 4000, 250, 0, exit1.GenerateAirbusCallSign(), 1000.0);
            Plane plane4 = new Boeing(Main.getInstance(), 10.0, 23.0, 5000, 280, 350, exit1.GenerateBoeingCallSign(), 1000.0);
            Plane plane5 = new Boeing(Main.getInstance(), 25.0, 28.0, 5500, 280, 290, exit1.GenerateBoeingCallSign(), 1000.0);
            Main.getInstance().AddAirport(airport1);
            Main.getInstance().AddAirport(airport2);
            Main.getInstance().AddAirport(airport3);
            Main.getInstance().AddExit(exit1);
            Main.getInstance().AddExit(exit2);
            Main.getInstance().AddExit(exit3);
            Main.getInstance().AddExit(exit4);
            Main.getInstance().AddExit(exit5);
            Main.getInstance().AddExit(exit6);
            Main.getInstance().AddPlane(plane1);
            Main.getInstance().AddPlane(plane2);
            Main.getInstance().AddPlane(plane3);
            Main.getInstance().AddPlane(plane4);
            Main.getInstance().AddPlane(plane5);
        }

        public int getScore()
        {
            return 20000;
        }
    }
}
