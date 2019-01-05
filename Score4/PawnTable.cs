using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Score4
{
    public class PawnTable: Grid
    {

        // CONSTRUCTORS

        public PawnTable(): base()
        {
            holes = new Team[tableRows, tableCols];
            clearBoard();
            StartPos = tableCols / 2;
        }

        // DELEGATES

        public delegate void PawnPlacedHandler(Team winner);
        public event PawnPlacedHandler PawnPlaced;

        // PUBLIC PROPERTIES

        public int StartPos { get; }
        public int CurrentPos { get; set; }

        // PUBLIC MEMBERS

        public Pawn CreateNewPlayingPawn(Team team)
        {
            currentPawn = new Pawn(team);
            CurrentPos = StartPos;
            Grid.SetRow(currentPawn, 0);
            Grid.SetColumn(currentPawn, CurrentPos);
            this.Children.Add(currentPawn);
            return currentPawn;
        }

        public Pawn GetCurrentPawn()
        {
            return currentPawn;
        }

        public Pawn MoveCurrentPawn(MoveDirection dir)
        {
            switch (dir)
            {
                case MoveDirection.Left:
                    if(CurrentPos>0)
                        CurrentPos--;
                    break;
                case MoveDirection.Right:
                    if(CurrentPos<tableCols-1)
                        CurrentPos++;
                    break;
            }
            Grid.SetColumn(currentPawn, CurrentPos);
            return currentPawn;
        }

        public void InsertPawn()
        {
            int bottom = findColumnBottom(CurrentPos);
            if (bottom > 0)
            {
                holes[bottom, CurrentPos] = currentPawn.PawnTeam;
                Grid.SetRow(currentPawn, bottom);
                Grid.SetColumn(currentPawn, CurrentPos);                
                PawnPlaced(checkForWinner(bottom, CurrentPos));
            }
        }

        public void ClearBoard()
        {
            clearBoard();
        }

        // PRIVATE MEMBERS

        private int tableRows = 7;
        private int tableCols = 7;
        private Team[,] holes;
        private Pawn currentPawn;

        private int findColumnBottom(int col)
        {
            int r = tableRows - 1;
            while (holes[r, col] != null && r > 0)
            {
                r--;
            }
            return r;
        }

        private void clearBoard()
        {
            for(int r = 0; r < tableRows; r++)
            {
                for (int c = 0; c < tableCols; c++)
                {
                    holes[r, c] = null;
                }
            }
            this.Children.Clear();
        }

        private Team checkForWinner(int row, int col)
        {
            Team teamWon = null;
            int winSeq = 4;
            int directions = 8;
            int[] seq = new int[directions];

            int[] deltaR = { -1, -1, -1, 0, 1, 1, 1, 0 };
            int[] deltaC = { -1, 0, 1, 1, 1, 0, -1, -1 };
            int cnt = 0;
            int d = 0;
            int r = 0;
            int c = 0;
            while (d < directions)
            {
                for (r = row, c = col, cnt = 0;
                    r >= 1 && c >= 0 && r < tableRows && c < tableCols && cnt < winSeq ;
                    r += deltaR[d], c += deltaC[d], cnt++)
                {
                    if (holes[r, c] == currentPawn.PawnTeam)
                        seq[d]++;
                    else
                        break;
                }
                if (seq[d] >= winSeq)
                {
                    teamWon = currentPawn.PawnTeam;
                    break;
                }
                d++;
            }
            return teamWon;
        }
    }
}
