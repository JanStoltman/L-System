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
using System.Windows.Shapes;

namespace L_Sys._2
{

    public partial class GetDataWindow : Window
    {
        public int MAX_ITERATIONS = 5;

        public double TURN_ANGLE = 25;
        public double START_ANGLE = -90; //Angles work clockwise -45 == normal coutnerclockwise 45
        public int MOVE_LENGHT = 10;

        public char[] nonTerminateChars = { 'A', 'B' };
        public string START_FORMULA = "A";
        public List<string> TRANSFORMATIONS = new List<String>();//→F → F+F−F−F+F

        public double STARTX = 700;
        public double STARTY = 800;

        public SolidColorBrush brush = Brushes.DarkGreen;

        public GetDataWindow()
        {
            InitializeComponent();
        }

        private void okBTT_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(iteracjeTXB.Text, out MAX_ITERATIONS);
            double.TryParse(katTXB.Text, out TURN_ANGLE);
            Int32.TryParse(moveLenghtTXB.Text, out MOVE_LENGHT);
            double.TryParse(XTXB.Text, out STARTX);
            double.TryParse(YTXB.Text, out STARTY);

            TRANSFORMATIONS = przeksztalceniaTXB.Text.Split('|').ToList<string>();
            START_FORMULA = startFormulaTXB.Text;
            this.Close();
        }

        private void smokBTT_Click(object sender, RoutedEventArgs e)
        {
          MAX_ITERATIONS = 14;

          TURN_ANGLE = 90;
          START_ANGLE = -180; //Angles work clockwise -45 == normal coutnerclockwise 45
          MOVE_LENGHT = 4;

         START_FORMULA = "FA";
         TRANSFORMATIONS.Add("A → A+BF+");
         TRANSFORMATIONS.Add("B → -FA−B");//→F → F+F−F−F+F

         STARTX = 500;
         STARTY = 200;

         brush = Brushes.DarkRed;

            this.Close();
        }

        private void waterShitBTT_Click(object sender, RoutedEventArgs e)
        {
            MAX_ITERATIONS = 5;

            TURN_ANGLE = 20;
            START_ANGLE = -90; //Angles work clockwise -45 == normal coutnerclockwise 45
            MOVE_LENGHT = 12;

            START_FORMULA = "F";
            TRANSFORMATIONS.Add("F → F[+F]F[-F][F]");

            STARTX = 600;
            STARTY = 800;

            brush = Brushes.DarkBlue;

            this.Close();
        }
    }
}
