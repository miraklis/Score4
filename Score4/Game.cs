﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Score4
{
    public class Game
    {
        // ENUMS **************************
        public enum State
        {
            ReadyToStart,
            Started,
            Finished
        }
        public enum Kind
        {
            PvP, // Player vs Player
            PvC  // Player vs CPU
        }
        public enum Result
        {
            Winner,
            Draw
        }

        // CONSTRUCTORS ********************
        public Game()
        {
            board = new Board();
            ai = new AI(board);
            scores = new int[2];
            state = State.ReadyToStart;
        }

        // PUBLIC MEMBERS *****************
        public State CurrentState => state;
        public Pawn CurrentPawn => currentPawn;
        public int CurrentTeam => currentTeam;
        public int CurrentSelection => selection;
        public int GetSelectionBottom => board.BottomRow(selection);
        public bool CanDropPawn => !board.IsColumnFull(selection);
        public int[] GetScores => scores;
        public Kind GetKind => kind;

        public bool Start(Kind k)
        {
            state = State.Started;
            kind = k;
            board.Clear();
            currentTeam = 0;
            CreateNewPawn(currentTeam); // First team pawn (team 0)
            return true;
        }

        public bool SelectLeftColumn()
        {
            if (selection > 0)
            {
                selection--;
                return true;
            }
            return false;
        }

        public bool SelectRightColumn()
        {
            if (selection < board.ColumnCount - 1)
            {
                selection++;
                return true;
            }
            return false;
        }

        public bool SetSelection(int col)
        {
            if(col >=0 && col < board.ColumnCount)
            {
                selection = col;
                return true;
            }
            return false;
        }

        public void DropPawn()
        {
            if(board.PutPawn(currentPawn, selection))
            {
                if(check4Winner())
                {
                    finish(Result.Winner);
                    return;
                }
                if(board.IsBoardFull)
                {
                    finish(Result.Draw);
                    return;
                }
            }
        }

        public Pawn CreateNewPawn(int t)
        {
            selection = 3;
            return currentPawn = new Pawn(t);
        }

        public void ResetScores()
        {
            scores[0] = 0;
            scores[1] = 0;
        }

        public int ChangeTeam()
        {
            return currentTeam = currentTeam == 0 ? 1 : 0;
        }

        public int AI_GetBestMove()
        {
            return ai.GetBestMove();
        }

        // PRIVATE MEMBERS ****************

        private void finish(Result result)
        {
            if (state == State.Started)
            {
                if (result == Result.Winner)
                {
                    board.FlashWinners();
                    scores[currentTeam]++;
                    System.Windows.MessageBox.Show("We have a winner", "Winner");
                }
                if (result == Result.Draw)
                    System.Windows.MessageBox.Show("Board is Full. We have a draw", "Draw");
                state = State.Finished;
            }
        }

        private bool check4Winner()
        {
            return board.Check4Seq(currentPawn);
        }

        private State state;
        private Kind kind;
        private Board board;
        private AI ai;
        private Pawn currentPawn;
        private int[] scores;
        private int currentTeam;
        private int selection;
    }
}