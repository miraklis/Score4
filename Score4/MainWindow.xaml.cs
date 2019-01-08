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
            team1 = new Team(Colors.Yellow);
            team2 = new Team(Colors.Red);
            state = GameState.ReadyToStart;
            tableRows = grdBoard.RowDefinitions.Count;
            tableCols = grdBoard.ColumnDefinitions.Count;
            cells = new Team[tableRows][];
            for (int r = 0; r < cells.Length; r++)
            {
                cells[r] = new Team[tableCols];
            }
            colSum = new int[tableCols];
            rowSum = new int[tableRows];
            clearBoard();
        }

        // PRIVATE MEMBERS

        private GameState state { get; set; }
        private Team team1;
        private Team team2;
        private Team currentTeam;
        private Pawn playingPawn;
        private int selectedPos;
        private int tableRows;
        private int tableCols;
        private int[] colSum;
        private int[] rowSum;
        private Team[][] cells;

        private void insertPawn()
        {
            int bottom = tableRows - colSum[selectedPos] - 1;
            colSum[selectedPos]++;
            rowSum[bottom]++;
            cells[bottom][selectedPos] = playingPawn.PawnTeam;
            Grid.SetRow(playingPawn, bottom);
            Grid.SetColumn(playingPawn, selectedPos);
            grdBoard.Children.Add(playingPawn);
            pawnPlaced();
        }

        private void pawnPlaced()
        {
            if (checkForWinner() != null)
            {
                currentTeam.AddWinPoints();
                state = GameState.Finished;
                updateScores();
                MessageBox.Show(currentTeam + " team has won the match", "Winner");
            }
            else
            {
                if (isBoardFull())
                {
                    state = GameState.Finished;
                    MessageBox.Show("It is a draw!", "Draw");
                }
                else
                {
                    changeTurn();
                    createNewPawn();
                }
            }
        }

        private Team checkForWinner()
        {
            int winSeq = 4;
            int row = tableRows - colSum[selectedPos];
            if (getMaxScore(selectedPos, row, playingPawn.PawnTeam) >= winSeq)
                return playingPawn.PawnTeam;
            else
                return null;
        }

        private int getMaxScore(int col, int row, Team t)
        {
            int directions = 8;
            int[] seq = new int[directions];

            int[] deltaR = { -1, -1, -1, 0, 1, 1, 1, 0 };
            int[] deltaC = { -1, 0, 1, 1, 1, 0, -1, -1 };
            int d = 0;
            int r = 0;
            int c = 0;
            while (d < directions)
            {
                for (r = row, c = col;
                    r >= 0 && c >= 0 && r < tableRows && c < tableCols;
                    r += deltaR[d], c += deltaC[d])
                {
                    if (cells[r][c] == t)
                        seq[d]++;
                    else
                        break;
                }
                d++;
            }
            d = 0;
            int max = 0;
            int half_dirs = directions / 2;
            while (d < half_dirs)
            {
                if ((seq[d] + seq[d + half_dirs]) - 1 > max)
                    max = (seq[d] + seq[d + half_dirs]) - 1;
                d++;
            }
            return max;
        }

        private void startGame()
        {
            if (state != GameState.Started)
            {
                state = GameState.Started;
                currentTeam = team1;
                clearBoard();
                createNewPawn();
                updateScores();
            }
        }

        private void createNewPawn()
        {
            playingPawn = new Pawn(currentTeam);
            selectedPos = 3;
            Grid.SetRow(playingPawn, 0);
            Grid.SetColumn(playingPawn, selectedPos);
            grdSelector.Children.Add(playingPawn);
        }

        private void clearBoard()
        {
            for (int c = 0; c < tableCols; c++)
            {
                for (int r = 0; r < tableRows; r++)
                {
                    cells[r][c] = null;
                    rowSum[r] = 0;
                }
                colSum[c] = 0;
            }
            grdBoard.Children.Clear();
        }

        private bool isBoardFull()
        {
            return (rowSum[0] >= tableCols);
        }

        private bool isColumnFull(int col)
        {
            return (colSum[col] >= tableRows);
        }

        private void changeTurn()
        {
            currentTeam = (currentTeam == team1 ? team2 : team1);
        }

        private void updateScores()
        {
            lbBlueScore.Content = team1.Score;
            lbRedScore.Content = team2.Score;
        }

        // EVENTS

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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool moveLeft = (selectedPos > 0) &&
                ((e.Key == Key.Left && currentTeam == team1) || (e.Key == Key.A && currentTeam == team2));
            bool moveRight = (selectedPos < grdBoard.ColumnDefinitions.Count() - 1) && 
                ((e.Key == Key.Right && currentTeam == team1) || (e.Key == Key.D && currentTeam == team2));
            bool moveDown = (!isColumnFull(selectedPos)) && 
                ((e.Key == Key.Down && currentTeam == team1) || (e.Key == Key.S && currentTeam == team2));
            if (state == GameState.Started)
            {
                if (moveLeft)
                {
                    selectedPos--;
                    Grid.SetColumn(playingPawn, selectedPos);
                }
                if (moveRight)
                {
                    selectedPos++;
                    Grid.SetColumn(playingPawn, selectedPos);
                }
                if (moveDown)
                {
                    grdSelector.Children.Remove(playingPawn);
                    insertPawn();
                }
            }
        }
    }
}
