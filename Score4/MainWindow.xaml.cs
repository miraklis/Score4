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
            team1 = new Team(Colors.Yellow);
            team2 = new Team(Colors.Red);
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
                MessageBox.Show(winner + " team has won the match", "Winner");
            }
            else
            {
                if (grdBoard.IsBoardFull())
                {
                    state = GameState.Finished;
                    MessageBox.Show("It is a draw!", "Draw");
                }
                else
                {
                    changeTurn();
                    grdBoard.CreateNewPlayingPawn(currentTeam);
                }
            }
        }

        private void BtStart_Click(object sender, RoutedEventArgs e)
        {
            startGame();
        }

        private void BtResetScores_Click(object sender, RoutedEventArgs e)
        {
            team1.ResetScore();
            team2.ResetScore();
            updateScores();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            bool moveLeft = (e.Key == Key.Left && currentTeam == team1)
                || (e.Key == Key.A && currentTeam == team2);
            bool moveRight = (e.Key == Key.Right && currentTeam == team1)
                || (e.Key == Key.D && currentTeam == team2);
            bool insertPawn = (e.Key == Key.Down && currentTeam == team1)
                || (e.Key == Key.S && currentTeam == team2);
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
        private Team team1;
        private Team team2;
        private Team currentTeam;

        private void startGame()
        {
            if (state != GameState.Started)
            {
                state = GameState.Started;
                currentTeam = team1;
                grdBoard.ClearBoard();
                grdBoard.CreateNewPlayingPawn(currentTeam);
                updateScores();
            }
        }

        private void changeTurn()
        {
            currentTeam = (currentTeam == team1 ? team2 : team1);
        }

        private void updateScores()
        {
            lbBlueScore.Content = team1.GetScore();
            lbRedScore.Content = team2.GetScore();
        }
    }
}
