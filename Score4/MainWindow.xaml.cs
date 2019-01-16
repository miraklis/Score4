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
        private int cellsCnt;
        private Team[][] cells;

        private void insertPawn(int col)
        {
            grdSelector.Children.Remove(playingPawn);
            int bottom = tableRows - colSum[col] - 1;
            colSum[col]++;
            cellsCnt++;
            cells[bottom][col] = playingPawn.PawnTeam;
            Grid.SetRow(playingPawn, bottom);
            Grid.SetColumn(playingPawn, col);
            grdBoard.Children.Add(playingPawn);

            if (checkForWinner(col, currentTeam))
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

        private bool checkForWinner(int col, Team t)
        {
            int winSeq = 4;
            int row = tableRows - colSum[col];
            if (getMaxScore(col, row, t) >= winSeq)
                return true;
            else
                return false;
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
                }
                colSum[c] = 0;
            }
            cellsCnt = 0;
            grdBoard.Children.Clear();
        }

        private bool isBoardFull()
        {
            return (cellsCnt >= tableCols * tableCols);
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

        private int findBestMove_AI()
        {
            if (isBoardFull())
                return -1;

            int bestMove = -1;

            // first check if AI can win
            for (int c=0; bestMove < 0 && c < tableCols; c++)
            {
                if (!isColumnFull(c))
                {
                    int bottom = tableRows - colSum[c] - 1;
                    cells[bottom][c] = team2; // play the AI piece virtually
                    colSum[c]++;
                    cellsCnt++;

                    if (checkForWinner(c, team2))
                        bestMove = c;

                    cells[bottom][c] = null; // restore cells
                    colSum[c]--;
                    cellsCnt--;
                }
            }

            // then check if human wins in next move
            for (int c = 0; bestMove < 0 && c < tableCols; c++)
            {
                if (!isColumnFull(c))
                {
                    int bottom = tableRows - colSum[c] - 1;
                    cells[bottom][c] = team1; // play the human piece virtually
                    colSum[c]++;
                    cellsCnt++;
                    if (checkForWinner(c, team1))
                        bestMove = c;
                    cells[bottom][c] = null; // restore cells
                    colSum[c]--;
                    cellsCnt--;
                }
            }

            // then check for forbidden move so not to let human win afte AI plays
            List<int> forbiddenMoves = new List<int>();
            for (int c = 0; bestMove < 0 && c < tableCols; c++)
            {
                if (!isColumnFull(c))
                {
                    int bottom = tableRows - colSum[c] - 1;
                    cells[bottom][c] = team2; // play the AI piece virtually
                    colSum[c]++;
                    cellsCnt++;
                    if (!isColumnFull(c))
                    {
                        int newBottom = tableRows - colSum[c] - 1;
                        cells[newBottom][c] = team1; // play the human piece virtually
                        colSum[c]++;
                        cellsCnt++;
                        if (checkForWinner(c, team1))
                            forbiddenMoves.Add(c);
                        cells[newBottom][c] = null; // restore cells
                        colSum[c]--;
                        cellsCnt--;
                    }
                    cells[bottom][c] = null; // restore cells
                    colSum[c]--;
                    cellsCnt--;
                }
            }

            // Return best if it exists
            if (bestMove >= 0)
                return bestMove;

            // else return a random move in a not full column an not in a forbidden col
            List<int> possibleMoves = new List<int>();
            for (int c = 0; c < tableCols; c++)
                if (!isColumnFull(c) && !forbiddenMoves.Contains(c))
                    possibleMoves.Add(c);
            if (possibleMoves.Count > 0)
            {
                Random r = new Random();
                int rMove = r.Next(possibleMoves.Count);
                return possibleMoves[rMove];
            }
            else
                return forbiddenMoves[0]; // we lost
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
            bool moveRight = (selectedPos < tableCols - 1) && 
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
                    insertPawn(selectedPos);
                    if (currentTeam == team2)
                    {
                        int colFromAI = findBestMove_AI();
                        insertPawn(colFromAI);
                    }
                }
            }
        }
    }
}
