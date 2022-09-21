using System.Diagnostics;
using System.Windows.Forms;
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

        public GameForm()
        {
            InitializeComponent();
        }
    }
}