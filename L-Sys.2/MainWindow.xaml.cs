using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace L_Sys._2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {//→
        char[] constatns = { 'F', '+', '-', '[', ']' };

        /*  #region water like shit
           int MAX_ITERATIONS = 5; 

          const double TURN_ANGLE = 20;
          double currentTurnAngle = -90; //Angles work clockwise -45 == normal coutnerclockwise 45
           int MOVE_LENGHT = 12;

          char[] nonTerminateChars = { 'A', 'B' };
          readonly string START_FORMULA = "F";
          readonly string[] TRANSFORMATIONS = { "F → F[+F]F[-F][F]" };

          readonly double STARTX = 600;
          readonly double STARTY = 800;

          SolidColorBrush brush = Brushes.AliceBlue;
          #endregion*/

        /*  #region Heighway dragon
            int MAX_ITERATIONS = 14;

           const double TURN_ANGLE = 90;
           double currentTurnAngle = -180; //Angles work clockwise -45 == normal coutnerclockwise 45
            int MOVE_LENGHT = 4;

           char[] nonTerminateChars = { 'A', 'B' };
           readonly string START_FORMULA = "FA";
           readonly string[] TRANSFORMATIONS = { "A → A+BF+", "B → -FA−B" };//→F → F+F−F−F+F

           readonly double STARTX = 500;
           readonly double STARTY = 200;
           #endregion*/

        /*#region bardzo ładna kalarepa Sir
         int MAX_ITERATIONS = 7;

        const double TURN_ANGLE = 25;
        double currentTurnAngle = -90; //Angles work clockwise -45 == normal coutnerclockwise 45
         int MOVE_LENGHT = 2;

        char[] nonTerminateChars = { 'A', 'B' };
        readonly string START_FORMULA = "A";
        readonly string[] TRANSFORMATIONS = { "A → F−[[A]+A]+F[+FA]−A" ,"F → FF" };//→F → F+F−F−F+F

        readonly double STARTX = 700;
        readonly double STARTY = 800;

        SolidColorBrush brush = Brushes.DarkGreen;
        #endregion*/

        #region lab part
         int MAX_ITERATIONS = 5;

        double TURN_ANGLE = 25;
        double START_ANGLE = -90; //Angles work clockwise -45 == normal coutnerclockwise 45
        int MOVE_LENGHT = 10;

        char[] nonTerminateChars = { 'A', 'B' };
        string START_FORMULA = "A";
        List<string> TRANSFORMATIONS = new List<string>();//→F → F+F−F−F+F

        double STARTX = 700;
        double STARTY = 800;

        SolidColorBrush brush = Brushes.DarkGreen;
        #endregion

        GetDataWindow dataWindow = new GetDataWindow();
        private readonly double lightionCorr = 1.06;
        Dictionary<char, string> transformations = new Dictionary<char, string>();
        string formula = "";

        public MainWindow()
        {
            InitializeComponent();


            dataWindow.ShowDialog();

            InitializeVariables();

            GenerateTransformations();

            TransformStartFormaula();
            GenerateFormula();

            Task.Factory.StartNew(() =>
            {
                Draw();
            });


        }

        private void InitializeVariables()
        {
            MainCanvas.Children.Clear();
            STARTX = dataWindow.STARTX;
          STARTY = dataWindow.STARTY;

          MAX_ITERATIONS = dataWindow.MAX_ITERATIONS;
          TURN_ANGLE = dataWindow.TURN_ANGLE;
          START_ANGLE = dataWindow.START_ANGLE;
          MOVE_LENGHT = dataWindow.MOVE_LENGHT;
          START_FORMULA = dataWindow.START_FORMULA;
          TRANSFORMATIONS = dataWindow.TRANSFORMATIONS;
            brush = dataWindow.brush;

            formula = "";
        }

        private void GenerateTransformations()
        {
            foreach (var x in TRANSFORMATIONS)
            {
                var v = x.Split(' ').ToArray();
                transformations.Add(x[0], v[2]);
            }
        }
        private void TransformStartFormaula()
        {
            foreach(char x in START_FORMULA)
            {
                if (transformations.ContainsKey(x))
                {
                    formula += transformations[x];
                }
                else formula += x;
            }
        }
        private void GenerateFormula()
        {
            string temp = "";
            for (int i = 1; i < MAX_ITERATIONS; i++)
            {
                foreach (char x in formula)
                {
                    if (transformations.ContainsKey(x))
                    {
                        temp += transformations[x];
                    }
                    else temp += x;
                }
                formula = temp;
                temp = "";
            }
        }
        private void Draw()
        {       
                double x1 = STARTX;
                double y1 = STARTY;
                double x2 = STARTX;
                double y2 = STARTY;

                List<Pointu> pointuList = new List<Pointu>();

                Color br = brush.Color;
                int colorCorrectionCounter = 1;

                double currentTurnAngle = START_ANGLE;
            MainCanvas.Dispatcher.InvokeAsync(new Action(() =>
            {
            foreach (var x in formula)
                {
               
                    if (colorCorrectionCounter  >= formula.Length / 10) { 
                        br = Color.FromArgb(br.A, (byte)(br.R * lightionCorr), (byte)(br.G * lightionCorr),
             (byte)(br.B * lightionCorr));
                        colorCorrectionCounter = 1;
                }

                    colorCorrectionCounter++;

                    switch (x)
                    {
                        case 'F':
                             x2 = x1 + (Math.Cos((Math.PI / 180.0) * currentTurnAngle) * MOVE_LENGHT);
                             y2 = y1 + (Math.Sin((Math.PI / 180.0) * currentTurnAngle) * MOVE_LENGHT);

                            break;

                        case '-':
                        case '−':
                            currentTurnAngle -= TURN_ANGLE;
                            break;

                        case '+':
                            currentTurnAngle += TURN_ANGLE;
                            break;

                        case '[':
                            pointuList.Add(new Pointu(x1, y1, currentTurnAngle));
                            break;

                        case ']':
                            currentTurnAngle = pointuList.Last().angle;
                            x1 = pointuList.Last().x;
                            y1 = pointuList.Last().y;
                            x2 = x1;
                            y2 = y1;
                            pointuList.Remove(pointuList.Last());
                            break;
                    }


                    if (x1 != x2 || y1 != y2)
                    {

                        Line line = new Line();
                        line.Stroke = new SolidColorBrush(br);

                        line.Y1 = y1;
                        line.Y2 = y2;
                        line.X1 = x1;
                        line.X2 = x2;

                        line.StrokeThickness = 2;

                        MainCanvas.Children.Add(line);
                        x1 = x2;
                        y1 = y2;
                  
                }         
        }
            }));

        }

        #region maybe useful one day
        //SolidColorBrush pickBrush()
        //{
        //    Type brushesType = typeof(Brushes);

        //    PropertyInfo[] properties = brushesType.GetProperties();
        //    Random random = new Random();

        //    int rnd = random.Next(properties.Length);
        //    return (SolidColorBrush)properties[rnd].GetValue(null, null);
        //    Brushes.
        //}
        #endregion





    }
}
