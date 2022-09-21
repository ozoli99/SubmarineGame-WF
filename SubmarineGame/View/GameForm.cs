using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System;
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

                    for (Int32 i = 0; i < _model.Mines.Count; ++i)
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
            Boolean restartTimer = _gameTimer.Enabled;
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