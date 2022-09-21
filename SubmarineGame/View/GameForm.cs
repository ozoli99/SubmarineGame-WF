using System.Diagnostics;
using Model;
using Persistence;

namespace View
{
    public partial class GameForm : Form
    {
        #region Private fields

        private SubmarineGameModel _model;
        private List<PictureBox> _mines;
        private Stopwatch _stopWatch;
        private System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.Timer _gameTimer;
        private System.Windows.Forms.Timer _minesTimer;

        #endregion

        #region Constructor

        public GameForm()
        {
            InitializeComponent();

            this.Width = SubmarineGameModel.GameAreaWidth + 16;
            this.Height = SubmarineGameModel.GameAreaHeight + 61;

            _model = new SubmarineGameModel(new TextFilePersistence());
            _model.SubmarineMoved += new EventHandler<SubmarineEventArgs>(Game_SubmarineMoved);
            _model.GameOver += new EventHandler<SubmarineEventArgs>(Game_GameOver);
            _model.MineDestroyed += new EventHandler<SubmarineEventArgs>(Game_MineDestroyed);
            _model.TimePaused += new EventHandler<SubmarineEventArgs>(Game_TimePaused);

            _mines = new List<PictureBox>();

            // This'll move the mines
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 50;
            _timer.Tick += new EventHandler(Timer_Tick);

            // This'll be responsible for the elapsed game time
            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = 1000;
            _gameTimer.Tick += new EventHandler(GameTimer_Tick);

            // This'll be responsible for the mine generation
            _minesTimer = new System.Windows.Forms.Timer();
            _minesTimer.Interval = 3000;
            _minesTimer.Tick += new EventHandler(MinesTimer_Tick);

            _stopWatch = new Stopwatch();
            _stopWatch.Start();

            KeyPreview = true;
            KeyDown += new KeyEventHandler(GameForm_KeyDown);

            NewGame();
        }

        #endregion

        #region Menu event handlers

        private void MenuFile_NewGame(object sender, EventArgs e)
        {
            StopTimers();

            toolStripDestroyedMineCount.Text = "0";

            NewGame();
        }

        private void MenuFile_LoadGame(object sender, EventArgs e)
        {
            StopTimers();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _model.LoadGame(openFileDialog.FileName);

                    submarine.Top = _model.Submarine.Y;
                    submarine.Left = _model.Submarine.X;

                    RemoveMines();

                    for (int i = 0; i < _model.Mines.Count; ++i)
                    {
                        CreateMine(_model.Mines[i].X, _model.Mines[i].Y);
                    }

                    StartTimers();
                }
                catch (DataException)
                {
                    MessageBox.Show("Error occurred during load.", "Submarine Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MenuFile_SaveGame(object sender, EventArgs e)
        {
            StopTimers();

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _model.SaveGame(saveFileDialog.FileName);
                }
                catch (DataException)
                {
                    MessageBox.Show("Error occurred during save.", "Submarine Game", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            StartTimers();
        }

        private void MenuFile_Exit(object sender, EventArgs e)
        {
            bool restartTimer = _gameTimer.Enabled;
            StopTimers();

            if (MessageBox.Show("Are you sure you want to quit?", "Submarine Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
            else
            {
                if (restartTimer)
                {
                    StartTimers();
                }
            }
        }

        #endregion

        #region GameForm event handlers

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    _model.Submarine_MoveUp();
                    break;
                case Keys.Down:
                case Keys.S:
                    _model.Submarine_MoveDown();
                    break;
                case Keys.Left:
                case Keys.A:
                    _model.Submarine_MoveLeft();
                    submarine.Image = Properties.Resources.submarineLeft;
                    break;
                case Keys.Right:
                case Keys.D:
                    _model.Submarine_MoveRight();
                    submarine.Image = Properties.Resources.submarine;
                    break;
                case Keys.Space:
                    _model.PauseGame();
                    break;
            }
        }

        #endregion

        #region Game event handlers

        private void Game_SubmarineMoved(object sender, SubmarineEventArgs e)
        {
            if (e.MoveUp)
            {
                submarine.Top -= SubmarineGameModel.SubmarineStep;
            }
            else if (e.MoveDown)
            {
                submarine.Top += SubmarineGameModel.SubmarineStep;
            }
            else if (e.MoveLeft)
            {
                submarine.Left -= SubmarineGameModel.SubmarineStep;
            }
            else
            {
                submarine.Left += SubmarineGameModel.SubmarineStep;
            }
        }

        private void Game_GameOver(object sender, SubmarineEventArgs e)
        {
            StopTimers();

            _stopWatch.Stop();
            long elapsedTime = _stopWatch.ElapsedMilliseconds / 1000;

            saveGameToolStripMenuItem.Enabled = false;

            if (MessageBox.Show("Game Over" + Environment.NewLine + "Time: " + TimeSpan.FromSeconds(elapsedTime).ToString("g") + Environment.NewLine + "Destroyed mines: " + e.DestroyedMineCount.ToString("g"), "Submarine Game", MessageBoxButtons.OK, MessageBoxIcon.Asterisk) == DialogResult.OK)
            {
                NewGame();
            }
        }

        private void Game_MineDestroyed(object sender, SubmarineEventArgs e)
        {
            toolStripDestroyedMineCount.Text = e.DestroyedMineCount.ToString("g");
        }

        private void Game_TimePaused(object sender, SubmarineEventArgs e)
        {
            StopTimers();

            if (MessageBox.Show("Game Paused" + Environment.NewLine + "Do you want to continue the game?", "Submarine Game", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
            {
                StartTimers();
            }
        }

        #endregion

        #region Time event handlers

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveMines();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            _model.gameTime++;
            toolStripTime.Text = TimeSpan.FromSeconds(_model.gameTime).ToString();
        }

        private void MinesTimer_Tick(object sender, EventArgs e)
        {
            GenerateMine();
            if (_minesTimer.Interval <= 3000 && _minesTimer.Interval > 2000)
            {
                _minesTimer.Interval -= 300;
            }
            else if (_minesTimer.Interval > 1000 && _minesTimer.Interval <= 2000)
            {
                _minesTimer.Interval -= 200;
            }
            else if (_minesTimer.Interval <= 1000 && _minesTimer.Interval > 500)
            {
                _minesTimer.Interval -= 100;
            }
        }

        #endregion

        #region Private methods

        private void NewGame()
        {
            _model.NewGame();

            submarine.Top = _model.Submarine.Y;
            submarine.Left = _model.Submarine.X;

            RemoveMines();

            for (int i = 0; i < _model.Mines.Count; ++i)
            {
                CreateMine(_model.Mines[i].X);
            }

            _minesTimer.Interval = 3000;
            StartTimers();
        }

        private void RemoveMines()
        {
            foreach (PictureBox mine in _mines)
            {
                this.Controls.Remove(mine);
                mine.Dispose();
            }
            _mines.Clear();
        }

        private void GenerateMine()
        {
            Shape newMine = _model.AddMine();

            PictureBox mine = new PictureBox();
            mine.Tag = "mine";
            mine.Image = Properties.Resources.nuclear_bomb;
            mine.SizeMode = PictureBoxSizeMode.AutoSize;
            mine.Top = newMine.Y;
            mine.Left = newMine.X;

            _mines.Add(mine);
            this.Controls.Add(mine);
        }

        private void CreateMine(int mineX, int mineY = 0)
        {
            PictureBox mine = new PictureBox();
            mine.Tag = "mine";
            mine.Image = Properties.Resources.nuclear_bomb;
            mine.SizeMode = PictureBoxSizeMode.AutoSize;
            mine.Top = mineY;
            mine.Left = mineX;

            _mines.Add(mine);
            this.Controls.Add(mine);
        }

        private void MoveMines()
        {
            _model.MoveMines();

            // If there was a deletion
            if (_model.Mines.Count != _mines.Count)
            {
                RemoveMines();
                for (int i = 0; i < _model.Mines.Count; ++i)
                {
                    CreateMine(_model.Mines[i].X, _model.Mines[i].Y);
                }
            }

            for (int i = 0; i < _mines.Count; ++i)
            {
                _mines[i].Top += SubmarineGameModel.MineStep * _model.Mines[i].Weight;
            }
        }

        private void StartTimers()
        {
            _timer.Start();
            _gameTimer.Start();
            _minesTimer.Start();
        }

        private void StopTimers()
        {
            _timer.Stop();
            _gameTimer.Stop();
            _minesTimer.Stop();
        }

        #endregion
    }
}