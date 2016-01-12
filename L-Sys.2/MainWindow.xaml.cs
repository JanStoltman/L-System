using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        double ANGLE_CHANGE = 1;

        double START_ANGLE = -90; //Angles work clockwise -45 == normal coutnerclockwise 45
        int MOVE_LENGHT = 10;

        char[] nonTerminateChars = { 'A', 'B' };
        string START_FORMULA = "A";
        List<string> TRANSFORMATIONS = new List<string>();//→F → F+F−F−F+F

        double STARTX = 700;
        double STARTY = 800;
        double angleCorrection = 1;

        SolidColorBrush brush = Brushes.DarkGreen;
        #endregion

        #region buttons variables
        int currentIteration = 1;
        int stepCounter = 0;
        int tColorCorrectionCounter = 1;
        double tCurrentAngle;
        List<Pointu> tPointuList = new List<Pointu>();


        double tempStepX;
        double tempStepY;
        string tempFormula;
        bool tFirstUse = true;
        bool toContinue = false;
        #endregion

        GetDataWindow dataWindow = new GetDataWindow();
        private readonly double lightionCorr = 1.02;
        List<Transformation> transformations = new List<Transformation>();
        string formula = "";


        public MainWindow()
        {
            InitializeComponent();

            dataWindow.ShowDialog();

            InitializeVariables();

            GenerateTransformations();

            GenerateFormula(MAX_ITERATIONS);

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
            angleCorrection = dataWindow.angleCorrection;

            tempStepX = STARTX;
            tempStepY = STARTY;
            tCurrentAngle = START_ANGLE;

            formula = "";
        }

        private void GenerateTransformations()
        {
            foreach (var x in TRANSFORMATIONS)
            {
                var v = x.Split(' ').ToArray();
                if (!transContains(x[0]))
                    transformations.Add(new Transformation(x[0], v[2]));
                else
                    transformations[getPlaceTrans(x[0])].addTrans(v[2]);

            }
        }

        private bool transContains(char x)
        {
            for(int i = 0; i < transformations.Count; i++)
            {
                if (transformations[i].seed == x) return true;
            }
            return false;
        }

        private int getPlaceTrans(char x)
        {
            for (int i = 0; i < transformations.Count; i++)
            {
                if (transformations[i].seed == x) return i;
            }
            return -1;
        }

        private void TransformStartFormula()
        {
            formula = "";
            foreach (char x in START_FORMULA)
            {
                if (transContains(x))
                {
                    formula += transformations[getPlaceTrans(x)].getTransformationString();
                }
                else formula += x;
            }
        }

        private void TransformTempStartFormula()
        {
            tempFormula = "";
            foreach (char x in START_FORMULA)
            {
                if (transContains(x))
                {
                    tempFormula += transformations[getPlaceTrans(x)].getTransformationString();
                }
                else tempFormula += x;
            }
        }

        private void GenerateFormula(int iterations)
        {
            TransformStartFormula();

            string temp = "";
            for (int i = 1; i < iterations; i++)
            {
                foreach (char x in formula)
                {
                    if (transContains(x))
                    {
                        temp += transformations[getPlaceTrans(x)].getTransformationString();
                    }
                    else temp += x;
                }
                formula = temp;
                temp = "";
            }
        }

        private void GenerateTempFormula(int iterations)
        {
            TransformTempStartFormula();

            string temp = "";
            for (int i = 1; i < iterations; i++)
            {
                foreach (char x in tempFormula)
                {
                    if (transContains(x))
                    {
                        temp += transformations[getPlaceTrans(x)].getTransformationString();
                    }
                    else temp += x;
                }
                tempFormula = temp;
                temp = "";
            }

        }


        private void Draw(String formulaTemp)
        {
            bool isFirstLoop = true;
            bool ifDraw = false;

            double x1 = STARTX;
            double y1 = STARTY;
            double x2 = STARTX;
            double y2 = STARTY;

            List<Pointu> pointuList = new List<Pointu>();

            Color br = brush.Color;
            int colorCorrectionCounter = 1;

            double currentTurnAngle = START_ANGLE;

            this.Dispatcher.Invoke(
                (ThreadStart)delegate ()
                {
                    foreach (var x in formulaTemp)
                    {
                        ifDraw = false;

                        if (colorCorrectionCounter >= formulaTemp.Length / 30)
                        {
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
                                ifDraw = true;
                                break;

                            case 'f':
                                x2 = x1 + (Math.Cos((Math.PI / 180.0) * tCurrentAngle) * MOVE_LENGHT);
                                y2 = y1 + (Math.Sin((Math.PI / 180.0) * tCurrentAngle) * MOVE_LENGHT);
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

                            case '>':
                                TURN_ANGLE += ANGLE_CHANGE;
                                break;

                            case '<':
                                TURN_ANGLE -= ANGLE_CHANGE;
                                break;
                        }

                        if (isFirstLoop)
                        {
                            MainCanvas.Children.Clear();
                            isFirstLoop = false;
                        }
                        if (ifDraw)
                        {

                            Line line = new Line();
                            line.Stroke = new SolidColorBrush(br);

                            line.Y1 = y1;
                            line.Y2 = y2;
                            line.X1 = x1;
                            line.X2 = x2;

                            line.StrokeThickness = 2;

                            MainCanvas.Children.Add(line);
                        }
                            x1 = x2;
                            y1 = y2;
                    }
                    MainCanvas.UpdateLayout();
                });
        }

        private void DrawStepByStep(String str,bool firstUse,double fStepX, double fStepY, int colorCorrectionCounter)
        {
            bool isFirstLoop = firstUse;
            bool ifDraw = false;

            double x1 = fStepX;
            double y1 = fStepY;
            double x2 = fStepX;
            double y2 = fStepY;

            //Color br = brush.Color; 

         //       if (colorCorrectionCounter >= formula.Length / 10)
         //       {
         //           br = Color.FromArgb(br.A, (byte)(br.R * lightionCorr), (byte)(br.G * lightionCorr),
         //(byte)(br.B * lightionCorr));
         //           colorCorrectionCounter = 1;
         //       }

         //       colorCorrectionCounter++;
            foreach (char x in str)
            {
                ifDraw = false;
                switch (x)
                {
                    case 'F':
                        x2 = x1 + (Math.Cos((Math.PI / 180.0) * tCurrentAngle) * MOVE_LENGHT);
                        y2 = y1 + (Math.Sin((Math.PI / 180.0) * tCurrentAngle) * MOVE_LENGHT);
                        ifDraw = true;
                        break;

                    case 'f':
                        x2 = x1 + (Math.Cos((Math.PI / 180.0) * tCurrentAngle) * MOVE_LENGHT);
                        y2 = y1 + (Math.Sin((Math.PI / 180.0) * tCurrentAngle) * MOVE_LENGHT);
                        break;

                    case '-':
                    case '−':
                        tCurrentAngle -= TURN_ANGLE;
                        break;

                    case '+':
                        tCurrentAngle += TURN_ANGLE;
                        break;

                    case '[':
                        tPointuList.Add(new Pointu(x1, y1, tCurrentAngle));
                        break;

                    case ']':
                        tCurrentAngle = tPointuList.Last().angle;
                        x1 = tPointuList.Last().x;
                        y1 = tPointuList.Last().y;
                        x2 = x1;
                        y2 = y1;
                        tPointuList.Remove(tPointuList.Last());
                        break;
                }
                try {
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        if (isFirstLoop)
                        {
                            MainCanvas.Children.Clear();
                            isFirstLoop = false;
                        }
                        if (ifDraw)
                        {

                            Line line = new Line();
                            line.Stroke = brush;// new SolidColorBrush(br);

                            line.Y1 = y1;
                            line.Y2 = y2;
                            line.X1 = x1;
                            line.X2 = x2;

                            line.StrokeThickness = 2;

                            MainCanvas.Children.Add(line);

                            tempStepX = x2;
                            tempStepY = y2;

                        }
                    }));
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        #region buttons functions
        private void button_Click(object sender, RoutedEventArgs e)
        {
            currentIteration = 1;

            toContinue = true;
            

            Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MainCanvas.Children.Clear();
                    this.UpdateLayout();
                }));

                while (stepCounter < formula.Length && toContinue)
                {
                    String tempForm = "";
                    while (!tempForm.Contains('F') && stepCounter < formula.Length)
                    {
                        tempForm += formula[stepCounter];
                        stepCounter++;
                    }

                    DrawStepByStep(tempForm, tFirstUse, tempStepX, tempStepY, tColorCorrectionCounter);

                    tFirstUse = false;
                    tColorCorrectionCounter++;
                    Thread.Sleep(15);
                }

                if (!toContinue)
                    stepCounter = 0;
                tempStepX = STARTX;
                tempStepY = STARTY;
                tFirstUse = true;
                tCurrentAngle = START_ANGLE;
            });      
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            toContinue = false;
            
            GenerateTempFormula(currentIteration);
            currentIteration++;

            this.Dispatcher.Invoke(new Action(() =>
            {
                MainCanvas.Children.Clear();
                this.UpdateLayout();
            }));

            Task.Factory.StartNew(() =>
            {
                
                Draw(tempFormula);
            });
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            currentIteration = 1;
            toContinue = false;
            Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    MainCanvas.Children.Clear();
                    this.UpdateLayout();
                }));
               Draw(formula);
            });
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            currentIteration = 1;
            toContinue = false;
            MainCanvas.Children.Clear();

            GenerateFormula(MAX_ITERATIONS);
            Task.Factory.StartNew(() =>
            {
                Draw(formula);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.UpdateLayout();
                }));
            });
        }
        #endregion
    }
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
