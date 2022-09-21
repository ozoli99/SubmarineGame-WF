using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System;
using Model;

namespace View
{
    public partial class GameForm : Form
    {
        #region Private fields

        private SubmarineGameModel _model;
        private List<PictureBox> _mines;
        private Stopwatch _stopWatch;
        private Timer _timer;
        private Timer _gameTimer;
        private Timer _minesTimer;

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
            _timer = new Timer();
            _timer.Interval = 50;
            _timer.Tick += new EventHandler(Timer_Tick);

            // This'll be responsible for the elapsed game time
            _gameTimer = new Timer();
            _gameTimer.Interval = 1000;
            _gameTimer.Tick += new EventHandler(GameTimer_Tick);

            // This'll be responsible for the mine generation
            _minesTimer = new Timer();
            _minesTimer.Interval = 3000;
            _minesTimer.Tick += new EventHandler(MinesTimer_Tick);

            _stopWatch = new Stopwatch();
            _stopWatch.Start();

            KeyPreview = true;
            KeyDown += new KeyEventHandler(GameForm_KeyDown);

            NewGame();
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