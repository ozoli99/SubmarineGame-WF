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

        #region Public methods

        public void NewGame()
        {
            _destroyedMineCount = 0;
            gameTime = 0;

            _submarine.X = (GameAreaWidth - SubmarineSize) / 2;
            _submarine.Y = (GameAreaHeight - SubmarineSize);

            _mines.Clear();
            GenerateStartingMines();
        }

        public void LoadGame(String fileName)
        {

        }

        public void SaveGame(String fileName)
        {

        }

        public void PauseGame()
        {

        }

        public void Submarine_MoveUp()
        {

        }

        public void Submarine_MoveDown()
        {

        }

        public void Submarine_MoveLeft()
        {

        }

        public void Submarine_MoveRight()
        {

        }

        public void MoveMines()
        {

        }

        public Shape AddMine()
        {
            Int32 mineX = _random.Next(1, GameAreaWidth - MineSize);
            Int32 mineWeight = _random.Next(1, 4);

            Shape newMine = new Shape(ShapeType.Mine, mineX, 0, MineSize, MineSize, mineWeight);

            while (_mines.Contains(newMine))
            {
                mineX = _random.Next(1, GameAreaWidth - MineSize);
                mineWeight = _random.Next(1, 4);

                newMine = new Shape(ShapeType.Mine, mineX, 0, MineSize, MineSize, mineWeight);
            }

            _mines.Add(newMine);

            return newMine;
        }

        #endregion

        #region Private methods

        private void GenerateStartingMines()
        {
            for (Int32 i = 0; i < 6; ++i)
            {
                AddMine();
            }
        }

        #endregion
    }
}