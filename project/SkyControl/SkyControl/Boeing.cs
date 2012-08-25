using Microsoft.Xna.Framework;

namespace SkyControl
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Boeing : SkyControl.Plane
    {
        public Boeing(Game game, double x, double y, int alt, int speed, double dir, string callSign, double fuel)
            : base(game, x, y, alt, speed, dir, callSign, fuel)
        {
            base.reactionTime = 4000.0;
        }
    }
}