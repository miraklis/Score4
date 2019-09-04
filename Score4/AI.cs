using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Score4
{
    class AI
    {
        // CONSTRUCTORS ***************
        public AI(Board b)
        {
            board = b;
        }

        // PUBLIC MEMBERS *************
        public int GetBestMove()
        {
            return findBestMove();
        }

        // PRIVATE MEMBERS ************
        private int findBestMove()
        {
            if (board.IsBoardFull)
                return -1;

            int bestMove = -1;
            Pawn p1 = new Pawn(0);
            Pawn p2 = new Pawn(1);

            // first check if AI can win
            for (int c = 0; bestMove < 0 && c < board.ColumnCount; c++)
            {
                if (board.PutPawn(p2, c))
                {
                    if (board.Check4Seq(p2))
                        bestMove = c;
                    board.RemovePawn(c);
                }
            }

            // then check if human wins in next move
            for (int c = 0; bestMove < 0 && c < board.ColumnCount; c++)
            {
                if (board.PutPawn(p1, c))
                {
                    if (board.Check4Seq(p1))
                        bestMove = c;
                    board.RemovePawn(c);
                }
            }

            // Return best if it exists
            if (bestMove >= 0)
                return bestMove;

            // then check for forbidden move so not to let human win after AI plays
            List<int> forbiddenMoves = new List<int>();
            for (int c = 0; bestMove < 0 && c < board.ColumnCount; c++)
            {
                if (board.PutPawn(p2, c))
                {
                    if (board.PutPawn(p1, c))
                    {
                        if (board.Check4Seq(p1))
                            forbiddenMoves.Add(c);
                        board.RemovePawn(c);
                    }
                    board.RemovePawn(c);
                }
            }

            // the moves which we are allowed to make in a not full column and not loose at the next turn
            List<int> possibleMoves = new List<int>();
            for (int c = 0; c < board.ColumnCount; c++)
                if (!board.IsColumnFull(c) && !forbiddenMoves.Contains(c))
                    possibleMoves.Add(c);

            // else return a random move in a not full column an not in a forbidden col
            if (possibleMoves.Count > 0)
            {
                Random r = new Random();
                int rMove = r.Next(possibleMoves.Count);
                return possibleMoves[rMove];
            }
            if (forbiddenMoves.Count > 0)
                return forbiddenMoves[0]; // ai lost

            return -1; // could not find any legal move
        }

        Board board;
    }
}
