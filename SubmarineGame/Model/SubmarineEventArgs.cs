namespace Model
{
    public class SubmarineEventArgs : EventArgs
    {
        private int _gameTime;
        private int _destroyedMineCount;
        private bool _moveUp;
        private bool _moveDown;
        private bool _moveLeft;
        private bool _moveRight;

        public int GameTime { get { return _gameTime; } }
        public int DestroyedMineCount { get { return _destroyedMineCount; } }
        public bool MoveUp { get { return _moveUp; } }
        public bool MoveDown { get { return _moveDown; } }
        public bool MoveLeft { get { return _moveLeft; } }
        public bool MoveRight { get { return _moveRight; } }

        public SubmarineEventArgs(int gameTime, int destroyedMineCount, bool moveUp, bool moveDown, bool moveLeft, bool moveRight)
        {
            _gameTime = gameTime;
            _destroyedMineCount = destroyedMineCount;
            _moveUp = moveUp;
            _moveDown = moveDown;
            _moveLeft = moveLeft;
            _moveRight = moveRight;
        }
    }
}
