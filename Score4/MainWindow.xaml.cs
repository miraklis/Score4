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
            Game.Kind k = Game.Kind.PvP;
            if (rbPvC.IsChecked == true)
                k = Game.Kind.PvC;
            if (game.Start(k))
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

        // EVENTS

        private void BtStart_Click(object sender, RoutedEventArgs e)
        {
            if(game.CurrentState != Game.State.Started ||
                (MessageBox.Show("Are you sure you want to start a new Game?","Confirm",MessageBoxButton.YesNo)==MessageBoxResult.Yes))
                startGame();
        }

        private void BtResetScores_Click(object sender, RoutedEventArgs e)
        {
            game.ResetScores();
            updateScores();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool moveLeft = (e.Key == Key.Left && game.CurrentTeam == 0) || (e.Key == Key.A && game.CurrentTeam == 1 && game.GetKind == Game.Kind.PvP);
            bool moveRight = (e.Key == Key.Right && game.CurrentTeam == 0) || (e.Key == Key.D && game.CurrentTeam == 1 && game.GetKind == Game.Kind.PvP);
            bool moveDown = (e.Key == Key.Down && game.CurrentTeam == 0) || (e.Key == Key.S && game.CurrentTeam == 1 && game.GetKind == Game.Kind.PvP);
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
                            if(game.GetKind == Game.Kind.PvC) // playing against the PC
                            {
                                game.SetSelection(game.AI_GetBestMove()); // find the best move                                
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
                            else
                            {
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
}
