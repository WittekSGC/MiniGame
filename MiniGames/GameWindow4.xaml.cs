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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
        bool sized = true;
        int h, w;
        int prevH, prevW;
        int PrevClickGridSizeX, PrevClickGridSizeY;
        Point[] RightPosition = new Point[5];

        public GameWindow4(MainWindow main, WindowState state)
        {
            InitializeComponent();
            Main = main;
            WindowState = state;
            AddStars();

            //SizeChanged += GameWindow4_SizeChanged;
            //StateChanged += GameWindow4_StateChanged;
            Loaded += GameWindow4_Loaded;

            RightPosition[0].X = 0;
            RightPosition[0].Y = 45;

            RightPosition[1].X = 174;
            RightPosition[1].Y = 76;

            RightPosition[2].X = 48;
            RightPosition[2].Y = 176;

            RightPosition[3].X = 234;
            RightPosition[3].Y = 58;
            
            RightPosition[4].X = 80;
            RightPosition[4].Y = 52;

        }

        private void GameWindow4_Loaded(object sender, RoutedEventArgs e)
        {
            AddClickGrid();
        }

        private void GameWindow4_StateChanged(object sender, EventArgs e)
        {
            ClickGridReSize();
        }

        private void GameWindow4_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClickGridReSize();
        }

        /// <summary>
        /// метод для управления объектом Canvas при клике мышью
        /// </summary>
        /// <param name="sender">Объект Canvas</param>
        /// <param name="e">события объекта</param>
        private void R_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double animationTime = 0.4;
            Image r = (sender as Canvas).Children[0] as Image;

            BitmapImage imgSrc = new BitmapImage();
            imgSrc.BeginInit();
            imgSrc.UriSource = new Uri("/Resources/Game4/circled.png", UriKind.Relative);
            imgSrc.EndInit();
            r.Source = imgSrc;

            DoubleAnimation doubleAnimationW = new DoubleAnimation(0, w, TimeSpan.FromSeconds(animationTime));
            DoubleAnimation doubleAnimationH = new DoubleAnimation(0, h, TimeSpan.FromSeconds(animationTime));
            r.BeginAnimation(WidthProperty, doubleAnimationW);
            r.BeginAnimation(HeightProperty, doubleAnimationH);

            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(w / 2, h / 2, 0, 0), 
                new Thickness(0, 0, 0, 0), 
                TimeSpan.FromSeconds(animationTime));
            r.BeginAnimation(MarginProperty, thicknessAnimation);

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
                    e.Cancel = true;
                else
                {
                    Main.WindowState = WindowState;
                    Main.Show();
                }
        }

        /// <summary>
        /// метод, масштабирующий сетку нажатий под текущий размер экрана
        /// </summary>
        private void ClickGridReSize()
        {
            if (!sized)
            {
                sized = true;
                return;
            }
            else
            {
                sized = false;
            }

            int counter = 0;
            foreach (Canvas item in ClickGrid.Children)
            {
                item.Width = ClickGrid.Width / w * PrevClickGridSizeX;
                item.Height = ClickGrid.Height / h * PrevClickGridSizeY;

                //доработать отступы

                item.Margin = new Thickness(item.Margin.Left * (PrevClickGridSizeX / ClickGrid.Width), 1, 0, 0);

                foreach (UIElement child in item.Children)
                {
                    if (child is Image)
                    {
                        (child as Image).Width = item.Width;
                        (child as Image).Height = item.Height;
                        (child as Image).Margin = new Thickness(0);
                        (child as Image).SetCurrentValue(WidthProperty, item.Width);
                        (child as Image).SetCurrentValue(HeightProperty, item.Height);
                        (child as Image).SetCurrentValue(MarginProperty, new Thickness(0));
                    }
                    prevW = (int)item.Width;
                    prevW = (int)item.Height;
                }

                counter++;
            }

            ClickGrid.UpdateLayout();
            PrevClickGridSizeX = (int)ClickGrid.Width;
            PrevClickGridSizeY = (int)ClickGrid.Height;
        }

        /// <summary>
        /// добавление сетки для нажатий
        /// </summary>
        private void AddClickGrid()
        {
            w = 40;
            h = 40;
            prevW = 40;
            prevH = 40;
            PrevClickGridSizeX = (int)ClickGrid.Width;
            PrevClickGridSizeY = (int)ClickGrid.Height;

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
                ClickGrid.Children.Add(c);
            }
            /*for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
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
                    int marginTop = i * w;
                    int marginLeft = j * h;
                    c.Margin = new Thickness(marginTop, marginLeft, 0, 0);
                    ClickGrid.Children.Add(c);
                }
            }
            */
        }

        /// <summary>
        /// отображение звездочек прогресса на экране
        /// </summary>
        private void AddStars()
        {
            BitmapImage imgSrc = new BitmapImage();
            imgSrc.BeginInit();
            imgSrc.UriSource = new Uri("/Resources/Game2/star.png", UriKind.Relative);
            imgSrc.EndInit();

            for (int i = 0; i < 5; i++)
            {
                StarsArray[i] = new Image
                {
                    Source = imgSrc,
                    Width = 50,
                    Height = 50,
                    Opacity = 0.4,
                    Margin = new Thickness(0, 0, 4, 0)
                };

                StarsPanel.Children.Add(StarsArray[i]);
            }
        }
    }
}
