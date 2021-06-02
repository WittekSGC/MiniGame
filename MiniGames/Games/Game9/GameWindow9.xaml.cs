using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace MiniGames
{
    /// <summary>
    /// Логика взаимодействия для GameWindow9.xaml
    /// </summary>
    public partial class GameWindow9 : Window
    {
        MainWindow Main;
        private bool ManualClosing = true;
        private Image[] StarsArray = new Image[5];
        private BitmapImage EmptyImg;
        private int LevelsCount = 6, Level = 0;
        private Image[][] Images = new Image[4][] {
            new Image[3],
            new Image[3],
            new Image[3],
            new Image[3]
            };

        //проверка на количество открытых элементов
        private int OpenedImagesCount = 0;
        private bool CheckNextLevel = false;
        private List<Image> OpenedImages = new List<Image>();

        //массив пар картинок через точки, в точках X - колонка, Y - строка
        private Point[][] ImagesPairs = new Point[6][]
        {
            new Point[2],
            new Point[2],
            new Point[2],
            new Point[2],
            new Point[2],
            new Point[2],
        };
        private BitmapImage[] imagesPathes = new BitmapImage[6];

        private Image ImagePick1, ImagePick2;
        private double LastWidth;
        private bool FirstClick = true;

        public GameWindow9(MainWindow main, WindowState state)
        {
            InitializeComponent();
            Main = main;
            WindowState = state;
            Loaded += GameWindow9_Loaded;

            imagesPathes[0] = App.LoadImageByPath("/Resources/Game9/кальмар.png");
            imagesPathes[1] = App.LoadImageByPath("/Resources/Game9/краб.png");
            imagesPathes[2] = App.LoadImageByPath("/Resources/Game9/осьминог.png");
            imagesPathes[3] = App.LoadImageByPath("/Resources/Game9/рыба1.png");
            imagesPathes[4] = App.LoadImageByPath("/Resources/Game9/рыба2.png");
            imagesPathes[5] = App.LoadImageByPath("/Resources/Game9/черепаха.png");

            EmptyImg = App.LoadImageByPath("/Resources/Game9/фон.png");
        }

        private void GameWindow9_Loaded(object sender, RoutedEventArgs e)
        {
            AddStars();
            CreateRandomPairs();
            LoadImages();

            SizeChanged += GameWindow9_SizeChanged;
        }

        private void GameWindow9_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeImages();
        }

        private void ResizeImages()
        {
            double imgWidth, imgHeight;
            imgWidth = (ActualWidth - 16) / 6 - 20;
            imgHeight = (ActualHeight - 248) / 3 - 20;

            if (imgWidth > imgHeight) imgWidth = imgHeight;
            else imgHeight = imgWidth;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Images[i][j].Width = imgWidth;
                    Images[i][j].Height = imgHeight;
                }
            }
        }

        //создание случайных пар в случайных местах поля
        private void CreateRandomPairs()
        {
            int count = 12;
            List<Point> points = new List<Point>();
            points.Add(new Point(0, 0));
            points.Add(new Point(1, 0));
            points.Add(new Point(2, 0));
            points.Add(new Point(3, 0));
            points.Add(new Point(0, 1));
            points.Add(new Point(1, 1));
            points.Add(new Point(2, 1));
            points.Add(new Point(3, 1));
            points.Add(new Point(0, 2));
            points.Add(new Point(1, 2));
            points.Add(new Point(2, 2));
            points.Add(new Point(3, 2));

            while (count > 0)
            {
                Random r = new Random();
                Point RandomPoint = points[r.Next(0, points.Count)];
                points.Remove(RandomPoint);

                int a = (count - 1) % 6;
                int b = (count - 1) / 6;
                ImagesPairs[a][b] = RandomPoint;
                count--;
            }
        }

        //загрузка начальных картнок
        private void LoadImages()
        {
            double imgWidth, imgHeight;
            imgWidth = (ActualWidth - 16) / 6 - 20;
            imgHeight = (ActualHeight - 248) / 3 - 20;

            if (imgWidth > imgHeight) imgWidth = imgHeight;
            else imgHeight = imgWidth;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Images[i][j] = new Image()
                    {
                        Stretch = Stretch.UniformToFill,
                        Source = EmptyImg,
                        Margin = new Thickness(10),
                        Height = imgHeight,
                        Width = imgWidth,
                        Cursor = Cursors.Hand
                    };
                    Grid.SetColumn(Images[i][j], i + 1);
                    Grid.SetRow(Images[i][j], j);

                    Images[i][j].MouseLeftButtonUp += GameWindow9_MouseLeftButtonUp;

                    GameGrid.Children.Add(Images[i][j]);
                }
            }
        }

        //картинка, на которую нажали - скрывается
        private void GameWindow9_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //запоминаем состояния 
            LastWidth = (sender as Image).Width;

            //проверяем, нажата первая, вторая, или повторяюаяся картинка
            if (ImagePick1 == null)
            { ImagePick1 = (sender as Image); FirstClick = true; OpenedImagesCount++; }
            else
                if (ImagePick1 == (sender as Image)) { FirstClick = true; OpenedImagesCount--; }
            else
            { ImagePick2 = (sender as Image); FirstClick = false; OpenedImagesCount++; }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(OpenedImages.Contains(Images[i][j])))
                        Images[i][j].IsEnabled = false;
                }
            }

            //если открыто 2 картинки - блокировка нажатий и вызов проверки
            if (OpenedImagesCount == 2)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!(OpenedImages.Contains(Images[i][j])))
                            Images[i][j].MouseLeftButtonUp -= GameWindow9_MouseLeftButtonUp;
                    }
                }
                CheckNextLevel = true;
            }

            //анимации ухода
            (sender as Image).RenderTransform = new ScaleTransform(1, 1, LastWidth / 2, LastWidth / 2);
            DoubleAnimation da = new DoubleAnimation(0, TimeSpan.FromSeconds(.2));
            (sender as Image).RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            da.Completed += Da_Completed;
            (sender as Image).RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);

        }

        //нажатая картинка спрявана
        private void Da_Completed(object sender, EventArgs e)
        {
            //замена изображения
            Image currentImg = (FirstClick) ? ImagePick1 : ImagePick2;
            int col = Grid.GetColumn(currentImg);
            int row = Grid.GetRow(currentImg);

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (ImagesPairs[i][j].X == col - 1 && ImagesPairs[i][j].Y == row)
                    {
                        if (currentImg.Source != imagesPathes[i])
                            currentImg.Source = imagesPathes[i];
                        else
                        {
                            currentImg.Source = EmptyImg;
                            ImagePick1 = null;
                        }
                    }
                }
            }

            //анимации возврата
            DoubleAnimation da = new DoubleAnimation(1, TimeSpan.FromSeconds(.3));
            currentImg.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            da.Completed += CheckLevelAfterAnimation;
            currentImg.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }

        //запуск проверки после анимации появления
        private void CheckLevelAfterAnimation(object sender, EventArgs e)
        {
            if (CheckNextLevel)
            {
                CheckNext();
                CheckNextLevel = false;
                OpenedImagesCount = 0;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(OpenedImages.Contains(Images[i][j])))
                        Images[i][j].IsEnabled = true;
                }
            }
        }

        //проверка на следующий уровень
        private void CheckNext()
        {
            if (ImagePick1.Source == ImagePick2.Source)
            {
                //следующий уровень
                OpenedImages.Add(ImagePick1);
                OpenedImages.Add(ImagePick2);
                NextLevel();


                ImagePick1 = null;
                ImagePick2 = null;
            }
            else
            {
                //закрыть открытые картинки и вернуть возможность нажатия

                //анимации ухода
                DoubleAnimation da = new DoubleAnimation(0, TimeSpan.FromSeconds(.2));
                da.BeginTime = TimeSpan.FromSeconds(.8);
                ImagePick1.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
                ImagePick1.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);
                ImagePick2.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
                da.Completed += ImagesHidden;
                ImagePick2.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);
            }
        }

        //картинки скрыты после проверки
        private void ImagesHidden(object sender, EventArgs e)
        {
            ImagePick1.Source = EmptyImg;
            ImagePick2.Source = EmptyImg;

            DoubleAnimation da = new DoubleAnimation(1, TimeSpan.FromSeconds(.3));
            ImagePick1.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            ImagePick1.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);
            ImagePick2.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            da.Completed += Da_Hiddden_Two_Images_Completed;
            ImagePick2.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);
            ImagePick1 = null;
            ImagePick2 = null;
        }

        private void Da_Hiddden_Two_Images_Completed(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(OpenedImages.Contains(Images[i][j])))
                        Images[i][j].MouseLeftButtonUp += GameWindow9_MouseLeftButtonUp;
                }
            }
        }

        private void NextLevel()
        {
            //открыть картинки и снять с них функцию нажатия
            StarsArray[Level++].BeginAnimation(OpacityProperty, new DoubleAnimation(1, TimeSpan.FromSeconds(.4)));

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(OpenedImages.Contains(Images[i][j])))
                        Images[i][j].MouseLeftButtonUp += GameWindow9_MouseLeftButtonUp;
                }
            }

            if (Level == LevelsCount)
                EndOfGame();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ManualClosing)
                if (!(bool)new ModalWindow("Вы точно хотите прервать игру?", ModalWindowMode.TextWithYesNoBtn).ShowDialog())
                {
                    e.Cancel = true;
                    return;
                }
            Main.WindowState = this.WindowState;
            Main.Show();
        }

        private void EndOfGame()
        {
            bool? Result = new ModalWindow("Молодец! Хочешь сыграть еще раз?", ModalWindowMode.TextWithYesNoBtn).ShowDialog();
            if ((bool)Result)
            {
                //восстановить стартовые значения
                Level = 0;
                OpenedImages = new List<Image>();
                OpenedImagesCount = 0;
                CreateRandomPairs();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Images[i][j].Source = EmptyImg;
                        Images[i][j].MouseLeftButtonUp += GameWindow9_MouseLeftButtonUp;
                        Images[i][j].IsEnabled = true;
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    StarsArray[i].BeginAnimation(OpacityProperty, null);
                }
            }
            else
            {
                ManualClosing = false;
                Close();
            }
        }

        private void AddStars()
        {
            BitmapImage imgSrc = TaskPattern.LoadImageByPath("/Resources/Game2/star.png");

            StarsArray = new Image[LevelsCount];

            for (int i = 0; i < LevelsCount; i++)
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
