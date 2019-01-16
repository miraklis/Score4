using System;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Controls;

namespace Score4
{
    public partial class Pawn : UserControl
    {

        // ENUMS *********************
        public enum State
        {
            Normal,
            Flashing
        }

        // CONSTRUCTORS **************
        public Pawn()
        {
            InitializeComponent();
        }

        public Pawn(int t) : this()
        {
            team = t;
            switch (team)
            {
                case 0:
                    pawnColor = Colors.Yellow;
                    break;
                case 1:
                    pawnColor = Colors.Red;
                    break;
                default:
                    pawnColor = Colors.Transparent;
                    break;
            }
            pawnArea.Fill = new SolidColorBrush(pawnColor);
            flashColor = Colors.Green;
            onFlash = false;
            flashPeriod = 500;
            tmr = new DispatcherTimer();
            tmr.Interval = TimeSpan.FromMilliseconds(flashPeriod);
            tmr.Tick += Tmr_Tick;
            tmr.Stop();
        }

        // EVENTS **************
        private void Tmr_Tick(object sender, EventArgs e)
        {
            Color cl = onFlash ? flashColor : pawnColor;
            pawnArea.Fill = new SolidColorBrush(cl);
            onFlash = !onFlash;
        }

        // OPERATOR OVERLOADING **********
        public static bool operator==(Pawn p1, Pawn p2)
        {
            if((p1 is Pawn) && (p2 is Pawn))
                return (p1.GetTeam() == p2.GetTeam());
            if ((p1 is null) && (p2 is null))
                return true;
            return false;
        }

        public static bool operator!=(Pawn p1, Pawn p2)
        {
            return !(p1 == p2);
        }

        // PUBLIC MEMBERS ****************
        public void StartFlashing() => tmr.Start();

        public int GetTeam() => team;

        // PRIVATE MEMBERS ******************

        private DispatcherTimer tmr;
        private Color pawnColor;
        private Color flashColor;
        private int flashPeriod;
        private bool onFlash;
        private int team;
    }
}