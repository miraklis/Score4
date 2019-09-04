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

        public int BottomRow(int col)
        {
            return tableRows - numPawnsPerCol[col] - 1;
        }

        public int TopPawnRow(int col)
        {
            return tableRows - numPawnsPerCol[col];
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
            if (!IsColumnFull(col))
            {
                cells[BottomRow(col)][col] = p;
                numPawnsPerCol[col]++;
                pawnsCounter++;
                return true;
            }
            return false;
        }

        public bool RemovePawn(int col)
        {
            if (TopPawnRow(col) < tableRows)
            {
                cells[TopPawnRow(col)][col] = null;
                numPawnsPerCol[col]--;
                pawnsCounter--;
                return true;
            }
            return false;
        }

        public bool Check4Seq(Pawn p, int pawnsInSeq = 4)
        {
            bool winnerSeqFound = false;
            winningPawns.Clear();
            int r, c;

            // check horizontal
            for (r = 0; !winnerSeqFound && r < tableRows; r++)
            {
                winningPawns.Clear();
                for (c = 0; !winnerSeqFound && c < tableCols; c++)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= pawnsInSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }

            // check vertical
            for (c = 0; !winnerSeqFound && c < tableCols; c++)
            {
                winningPawns.Clear();
                for (r = 0; !winnerSeqFound && r < tableRows; r++)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= pawnsInSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }

            // check left -> right diagonals
            for (int rr = 2; !winnerSeqFound && rr >= 0; rr--)
            {
                winningPawns.Clear();
                for (r = rr, c = 0; !winnerSeqFound && r < tableRows && c < tableCols; r++, c++)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= pawnsInSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }
            for (int cc = 1; !winnerSeqFound && cc < 4; cc++)
            {
                winningPawns.Clear();
                for (r = 0, c = cc; !winnerSeqFound && r < tableRows && c < tableCols; r++, c++)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= pawnsInSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }

            // check right -> left diagonals
            for (int rr = 2; !winnerSeqFound && rr >= 0; rr--)
            {
                winningPawns.Clear();
                for (r = rr, c = tableCols - 1; !winnerSeqFound && r < tableRows && c >= 0; r++, c--)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= pawnsInSeq)
                            winnerSeqFound = true;
                    }
                    else
                        winningPawns.Clear();
                }
            }
            for (int cc = tableCols - 2; !winnerSeqFound && cc > 2; cc--)
            {
                winningPawns.Clear();
                for (r = 0, c = cc; !winnerSeqFound && r < tableRows && c >= 0; r++, c--)
                {
                    if (cells[r][c] == p)
                    {
                        winningPawns.Add(cells[r][c]);
                        if (winningPawns.Count >= pawnsInSeq)
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
