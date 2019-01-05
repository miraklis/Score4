using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Score4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // GLOBAL ENUMS

    public enum GameState
    {
        ReadyToStart,
        Started,
        Finished
    }
    public enum MoveDirection
    {
        Left,
        Right,
        Down
    }


    // MAIN WINDOW

    public partial class MainWindow : Window
    {

        // CONSTRUCTORS

        public MainWindow()
        {
            InitializeComponent();
            grdBoard.PawnPlaced += Table_PawnPlaced;
            teamBlue = new Team(Colors.Blue);
            teamRed = new Team(Colors.Red);
            state = GameState.ReadyToStart;
        }

        // EVENTS

        private void Table_PawnPlaced(Team winner)
        {
            if (winner != null)
            {
                winner.AddWinPoints();
                state = GameState.Finished;
                updateScores();
                MessageBox.Show(winner.ToString() + " team has won the match", "Winner");
            }
            else
            {
                changeTurn();
                grdBoard.CreateNewPlayingPawn(currentTeam);
            }
        }

        private void BtStart_Click(object sender, RoutedEventArgs e)
        {
            startGame();
        }

        private void BtResetScores_Click(object sender, RoutedEventArgs e)
        {
            teamBlue.ResetScore();
            teamRed.ResetScore();
            updateScores();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            bool moveLeft = (e.Key == Key.Left && currentTeam == teamBlue)
                || (e.Key == Key.A && currentTeam == teamRed);
            bool moveRight = (e.Key == Key.Right && currentTeam == teamBlue)
                || (e.Key == Key.D && currentTeam == teamRed);
            bool insertPawn = (e.Key == Key.Down && currentTeam == teamBlue)
                || (e.Key == Key.S && currentTeam == teamRed);
            if (state == GameState.Started)
            {
                if (moveLeft)
                    grdBoard.MoveCurrentPawn(MoveDirection.Left);
                if (moveRight)
                    grdBoard.MoveCurrentPawn(MoveDirection.Right);
                if (insertPawn)
                    grdBoard.InsertPawn();
            }
        }
        
        // PRIVATE MEMBERS

        private GameState state { get; set; }
        private Team teamBlue;
        private Team teamRed;
        private Team currentTeam;

        private void startGame()
        {
            if (state != GameState.Started)
            {
                state = GameState.Started;
                currentTeam = teamBlue;
                grdBoard.ClearBoard();
                grdBoard.CreateNewPlayingPawn(currentTeam);
                updateScores();
            }
        }

        private void changeTurn()
        {
            currentTeam = (currentTeam == teamBlue ? teamRed : teamBlue);
        }

        private void updateScores()
        {
            lbBlueScore.Content = teamBlue.GetScore();
            lbRedScore.Content = teamRed.GetScore();
        }
    }
}
