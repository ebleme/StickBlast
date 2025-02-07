// maebleme2

namespace StickBlast
{
    public class BaseTileOccupation
    {
        public BaseTile BaseTile { get; set; }
        public int ConnectionCount { get; set; }

        public BaseTileOccupation(BaseTile baseTile, int connectionCount)
        {
            BaseTile = baseTile;
            ConnectionCount = connectionCount;
        }
    }
}