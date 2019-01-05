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
            pawnGradStopColl[0].Color = team.TeamColor;
            pawnGradStopColl[2].Color = team.TeamColor;
            PawnTeam = team;
        }

        public Team PawnTeam { get; set; }
    }
}
