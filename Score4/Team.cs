using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (TeamColor == Colors.Blue)
                return "Blue";
            else
                return "Red";
        }

        // PUBLIC METHODS
        public int GetScore()
        {
            return score;
        }

        public void AddWinPoints()
        {
            score ++;
        }

        public void ResetScore()
        {
            score = 0;
        }

        // PRIVATE MEMBERS
        private int score;
    }
}
