using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace MiniGames
{
    /// <summary>
    /// Логика взаимодействия для Game7.xaml
    /// </summary>
    public partial class GameWindow7 : Window
    {
        MainWindow Main;
        private bool ManualClosing = true;
        private Image[] StarsArray = new Image[5];
        private int LevelsCount = 3, Level = 1;
        private TaskPattern CurrentTask;

        //private Image[] Cluster = new Image[0];
        private Point[] ClusteredPoints;

        //multipler for sreen adoptation
        private double Multipler = 1;
        private double StartWidth, StartHeight;

        public GameWindow7(MainWindow main, WindowState state)
        {
            InitializeComponent();
            Main = main;
            WindowState = state;
            Loaded += GameWindow7_Loaded;
        }

        private void GameWindow7_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTask = new Task1();

            StartWidth = Width;
            StartHeight = Height;

            AddStars();
            LoadTask();

            MainCanvas.Drop += CanvasDrop;
            SizeChanged += ResizeElements;
        }

        private void ResizeElements(object sender, SizeChangedEventArgs e)
        {
            double x = ActualWidth - StartWidth;
            double y = ActualHeight - StartHeight;

            //узнать множитель изменения меньшего измерения (высота или ширины)
            Multipler = (x < y) ? ActualWidth / StartWidth : ActualHeight / StartHeight;

            MainCanvas.Width = CurrentTask.PictureSizeX * Multipler;
            MainCanvas.Height = CurrentTask.PictureSizeY * Multipler;
            MainCanvas.Margin = new Thickness(0, CurrentTask.MarginTop * Multipler, 0, 0);

            int count = 0;

            foreach (UIElement item in MainCanvas.Children)
            {
                Canvas.SetLeft(item, CurrentTask.ImagesPositions[count].X * Multipler);
                Canvas.SetTop(item, CurrentTask.ImagesPositions[count].Y * Multipler);
                (item as Image).Width = ClusteredPoints[count].X * Multipler;
                (item as Image).Height = ClusteredPoints[count].Y * Multipler;
                count++;
            }

            Title = Multipler.ToString();
        }

        private void CanvasDrop(object sender, DragEventArgs e)
        {
            double MouseX = e.GetPosition(MainCanvas).X;
            double MouseY = e.GetPosition(MainCanvas).Y;

            //relatived Task
            for (int i = 0; i < CurrentTask.ImagesCount; i++)
            {
                double x = CurrentTask.ImagesPositions[i].X;
                double y = CurrentTask.ImagesPositions[i].Y;

                if (MouseX >= x && MouseY >= y)
                {
                    x += CurrentTask.Images[i].Width;
                    y += CurrentTask.Images[i].Height;

                    if (MouseX <= x && MouseY <= y)
                    {
                        //drop check
                        string path = CurrentTask.Images[i].Source.ToString();
                        int pos = path.LastIndexOf('_');

                        if (/*имя картинки совпадают (но без empty)*/ e.Data.GetData(typeof(ImageSource)).ToString().Contains(path.Substring(pos + 1)))
                        {
                            CurrentTask.Images[i].Source = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                            foreach (UIElement item in DetailsPanel.Children)
                            {
                                if ((item as Image).Source == e.Data.GetData(typeof(ImageSource)) as ImageSource)
                                {
                                    DetailsPanel.Children.Remove(item);
                                    e.Handled = true;
                                    break;
                                }
                            }

                            EndOfLevel();
                            return;
                        }
                    }
                }
            }
        }

        private void EndOfLevel()
        {
            if (DetailsPanel.Children.Count == 0)
            {
                //MessageBox.Show("END OF LEVEL");
                NextLevel();
            }
        }

        private void NextLevel()
        {
            Level++;

            if (Level == 1)
            {
                for (int i = 0; i < StarsArray.Length; i++)
                {
                    StarsArray[i].BeginAnimation(OpacityProperty, null);
                }
            }
            else
            {
                DoubleAnimation opacityAnim = new DoubleAnimation(1, TimeSpan.FromSeconds(0.4));
                opacityAnim.BeginTime = TimeSpan.FromSeconds(1);
                opacityAnim.Completed += OpacityAnim_Completed;
                StarsArray[Level - 2].BeginAnimation(OpacityProperty, opacityAnim);
            }


            switch (Level)
            {
                case 1:
                    CurrentTask = new Task1();
                    break;
                case 2:
                    CurrentTask = new Task2();
                    break;
                case 3:
                    CurrentTask = new Task3();
                    break;
                default:
                    //end game
                    MessageBox.Show("END OF GAME");
                    EndOfGame();
                    break;
            }
        }

        private void EndOfGame()
        {
            bool? Result = new ModalWindow("Молодец! Хочешь сыграть еще раз?", ModalWindowMode.TextWithYesNoBtn).ShowDialog();
            if ((bool)Result)
            {
                Level = 0;
                MainCanvas.Children.Clear();
                DetailsPanel.Children.Clear();
                NextLevel();
                LoadTask();
            }
            else
            {
                ManualClosing = false;
                Close();
            }
        }

        private void OpacityAnim_Completed(object sender, EventArgs e)
        {
            if (Level <= LevelsCount)
            {
                MainCanvas.Children.Clear();
                DetailsPanel.Children.Clear();
                LoadTask();
            }
            else
                return;
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

        //подгрузка уровня
        private void LoadTask()
        {
            ClusteredPoints = new Point[CurrentTask.Images.Length];
            //Array.Copy(CurrentTask.Images, Cluster, CurrentTask.Images.Length);
            int count = 0;
            foreach (Image image in CurrentTask.Images)
            {
                ClusteredPoints[count] = new Point(image.Width, image.Height);
                count++;
            }

            //загрузка элементов для перетаскивания
            for (int i = CurrentTask.ImagesCount - 1; i >= 0; i--)
            {
                Image img = new Image()
                {
                    Source = CurrentTask.Images[i].Source,
                    MaxWidth = 150,
                    MaxHeight = 150,
                    MinWidth = 50,
                    MinHeight = 50
                };
                img.MouseLeftButtonDown += Img_MouseLeftButtonDown;
                DetailsPanel.Children.Add(img);
            }

            //установка размеров канваса
            MainCanvas.Width = CurrentTask.PictureSizeX;
            MainCanvas.Height = CurrentTask.PictureSizeY;
            MainCanvas.Margin = new Thickness(0, CurrentTask.MarginTop, 0, 0);

            //загрузка элементов уровня на канвас
            for (int i = 0; i < CurrentTask.ImagesCount; i++)
            {
                Canvas.SetLeft(CurrentTask.Images[i], CurrentTask.ImagesPositions[i].X);
                Canvas.SetTop(CurrentTask.Images[i], CurrentTask.ImagesPositions[i].Y);
                CurrentTask.Images[i].Source = TaskPattern.LoadImageByPath(CurrentTask.EmptyImagesPathes[i]);
                MainCanvas.Children.Add(CurrentTask.Images[i]);
            }

            ResizeElements(null, null);
        }

        private void Img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataObject data = new DataObject(typeof(ImageSource), ((Image)sender).Source);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Move | DragDropEffects.Copy);
        }
    }
}
