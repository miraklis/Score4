using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Score4
{
    class Board
    {
        // CONSTRUCTORS *****************
        public Board(int rows = 6, int cols = 7)
        {
            tableRows = rows;
            tableCols = cols;
            cells = new Pawn[tableRows][];
            for (int i = 0; i < tableRows; i++)
                cells[i] = new Pawn[tableCols];
            numPawnsPerCol = new int[tableCols];
            winningPawns = new List<Pawn>();
        }

        // PUBLIC MEMBERS ***************
        public int RowCount => tableRows;
        public int ColumnCount => tableCols;
        public bool IsBoardFull => pawnsCounter >= tableRows * tableCols;
        public bool IsColumnFull(int col) => numPawnsPerCol[col] >= tableRows;

        public int ColumnBottom(int col)
        {
            return tableRows - numPawnsPerCol[col] - 1;
        }

        public void Clear()
        {
            for (int r = 0; r < tableRows; r++)
            {
                for (int c = 0; c < tableCols; c++)
                {
                    cells[r][c] = null;
                }
            }
            for (int c = 0; c < tableCols; c++)
                numPawnsPerCol[c] = 0;
            pawnsCounter = 0;
        }

        public bool PutPawn(Pawn p, int col)
        {
            int bottom = ColumnBottom(col);
            if (bottom >= 0)
            {
                cells[bottom][col] = p;
                numPawnsPerCol[col]++;
                pawnsCounter++;
                return true;
            }
            return false;
        }

        public bool Check4Seq(Pawn p)
        {
            bool winnerSeqFound = false;
            winningPawns.Clear();
            int r, c;

            // check horizontal
            for (r = 0; !winnerSeqFound && r < cells.Length; r++)
            {
                for (c = 0; !winnerSeqFound && c < cells[r].Length; c++)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= winningSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }

            // check vertical
            for (c = 0; !winnerSeqFound && c < cells[0].Length; c++)
            {
                for (r = 0; !winnerSeqFound && r < cells.Length; r++)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= winningSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }

            // check diagonals (top left -> bottom right)
            for (int rr = 2; !winnerSeqFound && rr >= 0; rr--)
            {
                for (r = rr, c = 0; !winnerSeqFound && r < tableRows && c < tableCols; r++, c++)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= winningSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }
            for (int cc = 1; !winnerSeqFound && cc < 4; cc++)
            {
                for (r = 0, c = cc; !winnerSeqFound && r < tableRows && c < tableCols; r++, c++)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= winningSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }

            // check diagonals (top right -> bottom left)
            for (int rr = 2; !winnerSeqFound && rr >= 0; rr--)
            {
                for (r = rr, c = tableCols - 1; !winnerSeqFound && r < tableRows && c >= 0; r++, c--)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= winningSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }
            for (int cc = tableCols - 2; !winnerSeqFound && cc > 2; cc--)
            {
                for (r = 0, c = cc; !winnerSeqFound && r < tableRows && c >= 0; r++, c--)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= winningSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }
            return winnerSeqFound;
        }

        public void FlashWinners()
        {
            if(winningPawns.Count >= winningSeq)
            {
                foreach (var p in winningPawns)
                    p.StartFlashing();
            }
        }

        // PRIVATE MEMBERS ***************
        private int winningSeq = 4;
        private int tableRows;
        private int tableCols;
        private int pawnsCounter;
        private int[] numPawnsPerCol;
        private Pawn[][] cells;
        private List<Pawn> winningPawns;

    }
}
