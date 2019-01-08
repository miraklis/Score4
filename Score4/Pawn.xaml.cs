using System;
using System.Windows.Media;
using System.Windows.Controls;

namespace Score4
{
    /// <summary>
    /// Interaction logic for Pawn.xaml
    /// </summary>
    /// 
    
    public partial class Pawn : UserControl
    {
        public Pawn()
        {
            InitializeComponent();
        }

        public Pawn(Team team) : this()
        {
            pawnArea.Fill = new SolidColorBrush(team.TeamColor);
            PawnTeam = team;
        }

        public Team PawnTeam { get; set; }
    }
}
