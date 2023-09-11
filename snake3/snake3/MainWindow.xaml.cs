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
using System.Windows.Threading;

namespace snake3
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Point> apples = new List<Point>();

        private List<Point> snakePoints = new List<Point>();

        private enum Direction
        {
            Up,
            Down,
            Left,
            Right,
        };

        TimeSpan speed = new TimeSpan(10000);

        Point startpoint = new Point(400, 225);
        Point currentPosition = new Point();

        int direction = 0;

        int prevDirection = 0;

        int size = 20;

        int length = 100;

        Random rnd = new Random();

        int score = 0;


        private void MakeSnake(Point currentPos)
        {
            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = Brushes.Green;
            newEllipse.Width = size;
            newEllipse.Height = size;
            Canvas.SetTop(newEllipse, currentPos.Y);
            Canvas.SetLeft(newEllipse, currentPos.X);

            int count = background.Children.Count;
            background.Children.Add(newEllipse);
            snakePoints.Add(currentPos);

            if (count > length)
            {
                background.Children.RemoveAt(count - length + 9);
                snakePoints.RemoveAt(count - length);
            }
        }

        private void appleBonus(int index)
        {
            Point apple = new Point(rnd.Next(5, 795), rnd.Next(5, 445));

            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = Brushes.Red;
            newEllipse.Width = size;
            newEllipse.Height = size;
            Canvas.SetTop(newEllipse, apple.Y);
            Canvas.SetLeft(newEllipse, apple.X);
            background.Children.Insert(index, newEllipse);
            apples.Insert(index, apple);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (prevDirection != (int)Direction.Up)
                        direction = (int)Direction.Down;
                    break;

                case Key.Up:
                    if (prevDirection != (int)Direction.Down)
                        direction = (int)Direction.Up;
                    break;

                case Key.Left:
                    if (prevDirection != (int)Direction.Right)
                        direction = (int)Direction.Left;
                    break;

                case Key.Right:
                    if (prevDirection != (int)Direction.Left)
                        direction = (int)Direction.Right;
                    break;
            }
            prevDirection = direction;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            switch (direction)
            {
                case (int)Direction.Down:
                    currentPosition.Y += 1;
                    MakeSnake(currentPosition);
                    break;
                case (int)Direction.Up:
                    currentPosition.Y -= 1;
                    MakeSnake(currentPosition);
                    break;
                case (int)Direction.Left:
                    currentPosition.X -= 1;
                    MakeSnake(currentPosition);
                    break;
                case (int)Direction.Right:
                    currentPosition.X += 1;
                    MakeSnake(currentPosition);
                    break;

                    
            }

            if ((currentPosition.X < 5) || (currentPosition.X > 800) ||
                        (currentPosition.Y < 5) || (currentPosition.Y > 450))
                GameOver();



            int n = 0;
            foreach (Point point in apples)
            {
                if ((Math.Abs(point.X - currentPosition.X) < size) &&
                    (Math.Abs(point.Y - currentPosition.Y) < size))
                {
                    length += 25;
                    score += 10;
                    

                    apples.RemoveAt(n);
                    background.Children.RemoveAt(n);
                    appleBonus(n);
                    break;
                }
                n++;

            }


            for (int i = 0; i < (snakePoints.Count - size * 2); i++)
            {
                Point point = new Point(snakePoints[i].X, snakePoints[i].Y);
                if ((Math.Abs(point.X - currentPosition.X) < (size)) &&
                    (Math.Abs(point.Y - currentPosition.Y) < (size)))
                {
                    GameOver();
                    break;
                }
            }
        }

        private void GameOver()
        {
            MessageBox.Show("Game Over, your points are " + score.ToString(), "End of the game", MessageBoxButton.OK);
            this.Close();
        }




        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);

            timer.Interval = speed;
            timer.Start();

            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            MakeSnake(startpoint);
            currentPosition = startpoint;

            for (int i = 0; i < 10; i++)
            {
                appleBonus(i);
            }
        }
    }
}

