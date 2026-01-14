using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf
{
    enum GameColors
    {
        Player_1, 
        Player_2, 
        Tie,
        Default,
        Win
    }
    enum ButtonType
    {
        Cell,
        Reset
    }
    enum Cell
    {
        E,
        X,
        O
    }
    public partial class MainWindow : Window
    {
        private Cell[,] Matrix {  get; set; }
        private int Player = 1;
        public MainWindow()
        {
            InitializeComponent();
            InitializeButtons();
            InitializeMatrix();
            this.Width = 468;
            this.Height = 550;  
        }
        public void InitializeButtons()
        {
            WrapPanel wrapPanel = new WrapPanel();
            for (int i = 0; i < 9; i++)
            {
                Button button = new Button();
                button.Width = 150;
                button.Height = 150;
                button.Tag = ButtonType.Cell;
                button.Uid = i.ToString();
                button.Click += Button_Click;
                button.Background = GetColor(GameColors.Default);
                button.FontSize = 36;
                button.FontWeight = FontWeights.Bold;
                wrapPanel.Children.Add(button);
            }
            Button reset = new Button();
            reset.Width = 450;
            reset.Height = 50;
            reset.Content = "Reset";
            reset.Tag = ButtonType.Reset;
            reset.Click += Reset;
            wrapPanel.Children.Add(reset);
            this.Content = wrapPanel;
        }
        public void InitializeMatrix()
        {
            Matrix = new Cell[,]
            {
                {Cell.E,Cell.E,Cell.E},
                {Cell.E,Cell.E,Cell.E},
                {Cell.E,Cell.E,Cell.E},
            };
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if ((ButtonType)button.Tag != ButtonType.Cell)
                return;

            int index = int.Parse(button.Uid);

            int row = index / 3;
            int col = index % 3;

            if (Matrix[row, col] != Cell.E)
                return;

            if (Player == 1)
            {
                Matrix[row, col] = Cell.X;
                button.Background = GetColor(GameColors.Player_1);
                button.Content = Drow(Cell.X);
                Player = 2;
            }
            else
            {
                Matrix[row, col] = Cell.O;
                button.Background = GetColor(GameColors.Player_2);
                button.Content = Drow(Cell.O);
                Player = 1;
            }
            button.Click -= Button_Click;
            CheckWin();
        }
        private string Drow(Cell cell)
        {
            switch (cell)
            {
                case Cell.E:
                    return "";
                case Cell.X:
                    return "X";
                case Cell.O:
                    return "O";
                default:
                    return "";
            }
        }
        private void Reset(object sender, RoutedEventArgs e)
        {
            if (Content is not WrapPanel wrapPanel)
                return;

            foreach (var child in wrapPanel.Children)
            {
                if (child is Button button && (ButtonType)button.Tag == ButtonType.Cell)
                {
                    int index = int.Parse(button.Uid);
                    int row = index / 3;
                    int col = index % 3;
                    Matrix[row, col] = Cell.E;
                    button.IsEnabled = true;
                    button.Click += Button_Click;
                    button.Background = GetColor(GameColors.Default);
                    button.Content = "";
                }
            }
            Player = 1;
        }
        private void CheckWin()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Matrix[i, 0] == Matrix[i,1] && Matrix[i,1] == Matrix[i,2] && Matrix[i,0] != Cell.E)
                {
                    DrowWin(GetButton((i, 0)));
                    DrowWin(GetButton((i, 1)));
                    DrowWin(GetButton((i, 2)));
                    return;
                }

                if (Matrix[0, i] == Matrix[1, i] && Matrix[1, i] == Matrix[2, i] && Matrix[0, i] != Cell.E)
                {
                    DrowWin(GetButton((0, i)));
                    DrowWin(GetButton((1, i)));
                    DrowWin(GetButton((2, i)));
                    return;
                }
            }
            if (Matrix[0,0] == Matrix[1,1] && Matrix[1,1] == Matrix[2, 2] && Matrix[0, 0] != Cell.E)
            {
                DrowWin(GetButton((0, 0)));
                DrowWin(GetButton((1, 1)));
                DrowWin(GetButton((2, 2)));
                return;
            }
            if (Matrix[2, 0] == Matrix[1, 1] && Matrix[1, 1] == Matrix[0, 2] && Matrix[2, 0] != Cell.E)
            {
                DrowWin(GetButton((2, 0)));
                DrowWin(GetButton((1, 1)));
                DrowWin(GetButton((0, 2)));
                return;
            }
            bool isFull = true;
            foreach (var item in Matrix)
            {
                if(item == Cell.E)
                    isFull = false;
            }
            if (isFull)
            {
                Tie();
            }
        }
        private void Tie()
        {
            if (Content is not WrapPanel wrapPanel)
                return;

            foreach (var child in wrapPanel.Children)
            {
                if (child is Button button && (ButtonType)button.Tag == ButtonType.Cell)
                {
                    button.Background = GetColor(GameColors.Tie);
                    button.Click -= Button_Click;
                }
            }
        }
        private string GetButton((int,int) i)
        {
            switch (i)
            {
                case (0, 0):
                    return 0.ToString();
                case (0, 1):
                    return 1.ToString();
                case (0, 2):
                    return 2.ToString();
                case (1, 0):
                    return 3.ToString();
                case (1, 1):
                    return 4.ToString();
                case (1, 2):
                    return 5.ToString();
                case (2, 0):
                    return 6.ToString();
                case (2, 1):
                    return 7.ToString();
                case (2, 2):
                    return 8.ToString();
                default:
                    return 0.ToString();
            }
        }
        private void DrowWin(string index)
        {
            if (Content is not WrapPanel wrapPanel)
                return;

            foreach (var child in wrapPanel.Children)
            {
                if (child is Button button && (ButtonType)button.Tag == ButtonType.Cell)
                {
                    if (button.Uid == index)
                    {   
                        button.Background = GetColor(GameColors.Win);
                    }
                    button.Click -= Button_Click;
                }
            }
        }
        private LinearGradientBrush GetColor(GameColors color)
        {
            LinearGradientBrush myBrush = new LinearGradientBrush();

            switch (color)
            {
                case GameColors.Player_1:
                    myBrush.GradientStops.Add(new GradientStop(Colors.DarkGreen, 0.0));
                    myBrush.GradientStops.Add(new GradientStop(Colors.Green, 0.5));
                    myBrush.GradientStops.Add(new GradientStop(Colors.DarkOliveGreen, 1.0));
                    break;
                case GameColors.Player_2:
                    myBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.0));
                    myBrush.GradientStops.Add(new GradientStop(Colors.BlueViolet, 0.5));
                    myBrush.GradientStops.Add(new GradientStop(Colors.DarkBlue, 1.0));
                    break;
                case GameColors.Tie:
                    myBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
                    myBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
                    myBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
                    break;
                case GameColors.Default:
                    myBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
                    myBrush.GradientStops.Add(new GradientStop(Colors.White, 0.5));
                    myBrush.GradientStops.Add(new GradientStop(Colors.Gray, 1.0));
                    break;
                case GameColors.Win:
                    myBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
                    myBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
                    myBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
                    break;
                default:
                    break;
            }
            return myBrush;
        }
    }
}