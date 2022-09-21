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

        #region Public fields

        public int gameTime;

        #endregion

        #region Events

        #endregion

        #region Properties

        public Shape Submarine { get { return _submarine; } }
        public IList<Shape> Mines { get { return _mines.AsReadOnly(); } }

        #endregion

        #region Constructors

        public SubmarineGameModel() : this(null) { }
        public SubmarineGameModel(IPersistence persistence)
        {
            _destroyedMineCount = 0;
            gameTime = 0;

            _random = new Random();

            _submarine = new Shape(ShapeType.Submarine, (GameAreaWidth - SubmarineSize) / 2, GameAreaHeight - SubmarineSize, SubmarineSize, SubmarineSize, 1);
            _mines = new List<Shape>();

            _persistence = persistence;
        }

        #endregion
    }
}