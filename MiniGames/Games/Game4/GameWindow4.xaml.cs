using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace MiniGames
{
    /// <summary>
    /// Логика взаимодействия для Game4.xaml
    /// </summary>
    public partial class GameWindow4 : Window
    {
        MainWindow Main;
        private bool ManualClosing = true;
        private Image[] StarsArray = new Image[5];
        private int StarsEnabledCount = 0;
        int h, w, imgW, imgH;
        int Level = 1;

        Point[] RightPosition = new Point[5];

        //массив правильных точек нажатия
        Image[][] ImagesForClicks = new Image[5][]
        {
            new Image[2],
            new Image[2],
            new Image[2],
            new Image[2],
            new Image[2]
        };

        public GameWindow4(MainWindow main, WindowState state)
        {
            InitializeComponent();
            Main = main;
            WindowState = state;
            AddStars();

            Loaded += GameWindow4_Loaded;
            CreateRightPoints();
        }

        private void GameWindow4_Loaded(object sender, RoutedEventArgs e)
        {
            AddClickGrid();
        }

        /// <summary>
        /// метод для управления объектом Canvas при клике мышью
        /// </summary>
        /// <param name="sender">Объект Canvas</param>
        /// <param name="e">события объекта</param>
        private void R_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image r = (sender as Canvas).Children[0] as Image;
            bool Entry = false;

            //проверяет, было ли нажатие на одну из двух картинок для кликов
            foreach (Image[] items in ImagesForClicks)
            {
                foreach (Image item in items)
                {
                    if (r.Equals(item))
                    {
                        Entry = true;
                        break;
                    }
                }
                if (Entry) break;
            }

            //если NULL, значит было, а значит повторять анимации не нужно
            if (!Entry) return;

            //иначе проиграть анимации
            double animationTime = 0.4;
            DoubleAnimation doubleAnimationW = new DoubleAnimation(0, w, TimeSpan.FromSeconds(animationTime));
            DoubleAnimation doubleAnimationH = new DoubleAnimation(0, h, TimeSpan.FromSeconds(animationTime));
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(w / 2, h / 2, 0, 0),
                new Thickness(0, 0, 0, 0),
                TimeSpan.FromSeconds(animationTime));

            //и убрать обе картинки из массива
            foreach (Image[] items in ImagesForClicks)
            {
                foreach (Image item in items)
                {
                    if (r.Equals(item))
                    {
                        items[0].Source = App.LoadImageByPath("/Resources/Game4/circled.png");
                        items[0].BeginAnimation(WidthProperty, doubleAnimationW);
                        items[0].BeginAnimation(HeightProperty, doubleAnimationH);
                        items[0].BeginAnimation(MarginProperty, thicknessAnimation);
                        items[1].Source = App.LoadImageByPath("/Resources/Game4/circled.png");
                        items[1].BeginAnimation(WidthProperty, doubleAnimationW);
                        items[1].BeginAnimation(HeightProperty, doubleAnimationH);
                        items[1].BeginAnimation(MarginProperty, thicknessAnimation);

                        items[0] = null;
                        items[1] = null;

                        //зажигание звезды 
                        EndOfLevel();

                        return;
                    }
                }
            }

        }

        /// <summary>
        /// действия при закрытии окна
        /// </summary>
        /// <param name="sender">окно</param>
        /// <param name="e">переменная обработки событий</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ManualClosing)
                if (!(bool)new ModalWindow("Вы точно хотите прервать игру?", ModalWindowMode.TextWithYesNoBtn).ShowDialog())
                {
                    e.Cancel = true;
                    return;
                }
            Main.WindowState = WindowState;
            Main.Show();
        }

        /// <summary>
        /// добавление сетки для нажатий
        /// </summary>
        private void AddClickGrid()
        {
            //высота кружка
            if (Level == 1)
            {
                w = 40;
                h = 40;
                imgW = 360;
                imgH = 258;
            }
            //if (Level == 2)
            else
            {
                w = 50;
                h = 50;
                imgW = 470;
                imgH = 230;
            }
            int countX = imgW / w;
            int countY = imgH / h;

            for (int i = 0; i < 5; i++)
            {
                Canvas c = new Canvas()
                {
                    Width = w,
                    Height = h,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = Brushes.Transparent
                };
                Image r = new Image()
                {
                    Source = null,
                    Width = 0,
                    Height = 0,
                    Margin = new Thickness(w / 2, h / 2, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                c.Children.Add(r);
                c.MouseLeftButtonUp += R_MouseLeftButtonUp;
                int marginTop = (int)RightPosition[i].X;
                int marginLeft = (int)RightPosition[i].Y;
                c.Margin = new Thickness(marginTop, marginLeft, 0, 0);
                LeftClicks.Children.Add(c);

                //занесение созданной картинки в массив 
                ImagesForClicks[i][0] = r;
            }

            for (int i = 0; i < 5; i++)
            {
                Canvas c = new Canvas()
                {
                    Width = w,
                    Height = h,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = Brushes.Transparent
                };
                Image r = new Image()
                {
                    Source = null,
                    Width = 0,
                    Height = 0,
                    Margin = new Thickness(w / 2, h / 2, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                c.Children.Add(r);
                c.MouseLeftButtonUp += R_MouseLeftButtonUp;
                int marginTop = (int)RightPosition[i].X;
                int marginLeft = (int)RightPosition[i].Y;
                c.Margin = new Thickness(marginTop, marginLeft, 0, 0);
                RightClicks.Children.Add(c);

                //занесение созданной картинки в массив 
                ImagesForClicks[i][1] = r;
            }
        }

        /// <summary>
        /// отображение звездочек прогресса на экране
        /// </summary>
        private void AddStars()
        {
            for (int i = 0; i < 5; i++)
            {
                StarsArray[i] = new Image
                {
                    Source = App.LoadImageByPath("/Resources/star.png"),
                    Width = 50,
                    Height = 50,
                    Opacity = 0.4,
                    Margin = new Thickness(0, 0, 4, 0)
                };

                StarsPanel.Children.Add(StarsArray[i]);
            }
        }

        /// <summary>
        /// зажигание звездочки при правильном нажатии
        /// </summary>
        private void EndOfLevel()
        {
            DoubleAnimation showStar = new DoubleAnimation(1, TimeSpan.FromSeconds(0.6));
            StarsArray[StarsEnabledCount].BeginAnimation(OpacityProperty, showStar);
            StarsEnabledCount++;

            if (StarsEnabledCount == 5)
            {
                EndOfGame();
            }
        }

        /// <summary>
        /// метод, выполняющийся когда все отличия найдены
        /// </summary>
        private void EndOfGame()
        {
            bool? Result = new ModalWindow("Молодец! Хочешь сыграть еще раз?", ModalWindowMode.TextWithYesNoBtn).ShowDialog();
            if ((bool)Result)
            {
                //восстанвить начальные значения 
                Level = (Level == 1) ? 2 : 1;

                leftImage.Source = App.LoadImageByPath("/Resources/Game4/lvl" + Level.ToString() + "/part1left.png");
                rightImage.Source = App.LoadImageByPath("/Resources/Game4/lvl" + Level.ToString() + "/part2right.png");
                foreach (Image image in StarsArray)
                {
                    image.BeginAnimation(OpacityProperty, null); 
                }

                CreateRightPoints();

                StarsEnabledCount = 0;
                foreach (Image item in StarsArray)
                {
                    item.Opacity = 0.4;
                }

                LeftClicks.Children.Clear();
                RightClicks.Children.Clear();

                AddClickGrid();
            }
            else
            {
                ManualClosing = false;
                Close();
            }
        }

        private void CreateRightPoints()
        {
            switch (Level)
            {
                case 1:
                    RightPosition[0].X = 290;
                    RightPosition[0].Y = 45;

                    RightPosition[1].X = 105;
                    RightPosition[1].Y = 35;

                    RightPosition[2].X = 65;
                    RightPosition[2].Y = 185;

                    RightPosition[3].X = 225;
                    RightPosition[3].Y = 70;

                    RightPosition[4].X = 0;
                    RightPosition[4].Y = 20;
                    break;
                case 2:
                    RightPosition[0].X = 10;
                    RightPosition[0].Y = 120;

                    RightPosition[1].X = 85;
                    RightPosition[1].Y = 115;

                    RightPosition[2].X = 150;
                    RightPosition[2].Y = 35;

                    RightPosition[3].X = 300;
                    RightPosition[3].Y = 45;

                    RightPosition[4].X = 280;
                    RightPosition[4].Y = 180;
                    break;
                default:
                    break;
            }
        }
    }
}
