using System;
using System.Windows.Media;

namespace Score4
{
    public class Team
    {
        // CONSTRUCTORS

        public Team(Color color)
        {
            TeamColor = color;
            score = 0;
        }

        // PUBLIC PROPERTIES
        public Color TeamColor { get; }

        // OVERLOADED METHODS
        public override string ToString()
        {
            if (TeamColor == Colors.Yellow)
                return "Yellow";
            else
                return "Red";
        }

        // PUBLIC PROPERTIES
        public int Score
        {
            get => score;
        }

        // PUBLIC METHODS
        public void AddWinPoints()
        {
            score++;
        }

        public void ResetScore()
        {
            score = 0;
        }

        // PRIVATE MEMBERS
        private int score;
    }
}
