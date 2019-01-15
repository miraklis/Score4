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
    // MAIN WINDOW

    public partial class MainWindow : Window
    {
        // CONSTRUCTORS

        public MainWindow()
        {
            InitializeComponent();
            game = new Game();         
        }

        // PRIVATE MEMBERS
        private Game game;

        private void startGame()
        {
            if (game.Start())
            {
                grdBoard.Children.Clear();
                grdSelector.Children.Clear();
                Grid.SetRow(game.CurrentPawn, 0);
                Grid.SetColumn(game.CurrentPawn, game.CurrentSelection);
                grdSelector.Children.Add(game.CurrentPawn);
            }
        }

        private void updateScores()
        {
            int[] scores = game.GetScores;
            lbBlueScore.Content = scores[0];
            lbRedScore.Content = scores[1];
        }

        //private int findBestMove_AI()
        //{
        //    if (isBoardFull())
        //        return -1;

        //    int bestMove = -1;

        //    // first check if AI can win
        //    for (int c=0; bestMove < 0 && c < tableCols; c++)
        //    {
        //        if (!isColumnFull(c))
        //        {
        //            int bottom = tableRows - colSum[c] - 1;
        //            cells[bottom][c] = team2; // play the AI piece virtually
        //            colSum[c]++;
        //            cellsCnt++;

        //            if (checkForWinner(c, team2))
        //                bestMove = c;

        //            cells[bottom][c] = null; // restore cells
        //            colSum[c]--;
        //            cellsCnt--;
        //        }
        //    }

        //    // then check if human wins in next move
        //    for (int c = 0; bestMove < 0 && c < tableCols; c++)
        //    {
        //        if (!isColumnFull(c))
        //        {
        //            int bottom = tableRows - colSum[c] - 1;
        //            cells[bottom][c] = team1; // play the human piece virtually
        //            colSum[c]++;
        //            cellsCnt++;
        //            if (checkForWinner(c, team1))
        //                bestMove = c;
        //            cells[bottom][c] = null; // restore cells
        //            colSum[c]--;
        //            cellsCnt--;
        //        }
        //    }

        //    // then check for forbidden move so not to let human win afte AI plays
        //    List<int> forbiddenMoves = new List<int>();
        //    for (int c = 0; bestMove < 0 && c < tableCols; c++)
        //    {
        //        if (!isColumnFull(c))
        //        {
        //            int bottom = tableRows - colSum[c] - 1;
        //            cells[bottom][c] = team2; // play the AI piece virtually
        //            colSum[c]++;
        //            cellsCnt++;
        //            if (!isColumnFull(c))
        //            {
        //                int newBottom = tableRows - colSum[c] - 1;
        //                cells[newBottom][c] = team1; // play the human piece virtually
        //                colSum[c]++;
        //                cellsCnt++;
        //                if (checkForWinner(c, team1))
        //                    forbiddenMoves.Add(c);
        //                cells[newBottom][c] = null; // restore cells
        //                colSum[c]--;
        //                cellsCnt--;
        //            }
        //            cells[bottom][c] = null; // restore cells
        //            colSum[c]--;
        //            cellsCnt--;
        //        }
        //    }

        //    // Return best if it exists
        //    if (bestMove >= 0)
        //        return bestMove;

        //    // the moves which we are allowed to make in a not full column and not loose at the next turn
        //    List<int> possibleMoves = new List<int>();
        //    for (int c = 0; c < tableCols; c++)
        //        if (!isColumnFull(c) && !forbiddenMoves.Contains(c))
        //            possibleMoves.Add(c);

        //    // get the move with the best score
        //    if (possibleMoves.Count > 0) {
        //        int maxScore = 1;
        //        int score = 0;
        //        foreach(var move in possibleMoves)
        //        {
        //            int bottom = tableRows - colSum[move] - 1;
        //            cells[bottom][move] = team2; // play the AI piece virtually
        //            colSum[move]++;
        //            cellsCnt++;
        //            score = getScore(move, team2);
        //            if (maxScore < score)
        //            {
        //                maxScore = score;
        //                bestMove = move;
        //            }
        //            cells[bottom][move] = null; // play the AI piece virtually
        //            colSum[move]--;
        //            cellsCnt--;
        //        }
        //    }

        //    // Return best if it exists
        //    if (bestMove >= 0)
        //        return bestMove;

        //    // else return a random move in a not full column an not in a forbidden col
        //    if (possibleMoves.Count > 0)
        //    {
        //        Random r = new Random();
        //        int rMove = r.Next(possibleMoves.Count);
        //        return possibleMoves[rMove];
        //    }
        //    else
        //        return forbiddenMoves[0]; // we lost
        //}

        // EVENTS

        private void BtStart_Click(object sender, RoutedEventArgs e)
        {
            startGame();
        }

        private void BtResetScores_Click(object sender, RoutedEventArgs e)
        {
            game.ResetScores();
            updateScores();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool moveLeft = (e.Key == Key.Left && game.CurrentTeam == 0) || (e.Key == Key.A && game.CurrentTeam == 1);
            bool moveRight = (e.Key == Key.Right && game.CurrentTeam == 0) || (e.Key == Key.D && game.CurrentTeam == 1);
            bool moveDown = (e.Key == Key.Down && game.CurrentTeam == 0) || (e.Key == Key.S && game.CurrentTeam == 1);
            if (game.CurrentState == Game.State.Started)
            {
                if (moveLeft)
                {
                    if(game.SelectLeftColumn())
                        Grid.SetColumn(game.CurrentPawn, game.CurrentSelection);
                }
                if (moveRight)
                {
                    if(game.SelectRightColumn())
                        Grid.SetColumn(game.CurrentPawn, game.CurrentSelection);
                }
                if (moveDown)
                {
                    if (game.CanDropPawn)
                    {
                        grdSelector.Children.Remove(game.CurrentPawn);
                        Grid.SetRow(game.CurrentPawn, game.GetSelectionBottom);
                        Grid.SetColumn(game.CurrentPawn, game.CurrentSelection);
                        grdBoard.Children.Add(game.CurrentPawn);
                        game.DropPawn();
                        if (game.CurrentState == Game.State.Finished)
                        {
                            updateScores();
                        }
                        else
                        {
                            game.CreateNewPawn(game.ChangeTeam());
                            Grid.SetRow(game.CurrentPawn, 0);
                            Grid.SetColumn(game.CurrentPawn, game.CurrentSelection);
                            grdSelector.Children.Add(game.CurrentPawn);
                        }
                    }
                }
            }
        }
    }
}
