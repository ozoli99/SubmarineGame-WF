using Persistence;

namespace Model
{
    public class SubmarineGameModel
    {
        #region Constants

        public const int GameAreaWidth = 940;
        public const int GameAreaHeight = 700;
        public const int SubmarineSize = 64;
        public const int MineSize = 64;
        public const int SubmarineStep = 10;
        public const int MineStep = 5;

        #endregion

        #region Private fields

        private Shape _submarine;
        private List<Shape> _mines;
        private Random _random;
        private int _destroyedMineCount;
        private IPersistence _persistence;

        #endregion
    }
}