using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Логика взаимодействия для GameWindow1.xaml
    /// </summary>
    public partial class GameWindow1 : Window
    {
        private const string UnknownImage = @"Resources/Game1/unknown.jpg";

        private MainWindow Main;
        private WindowState State;

        private int Level = 1;
        private Image[] ImagesOffspings;
        private Image[] ImagesParent;

        private Image LastClicked;
        private bool[] TargetsEntries = new bool[] {false, false, false, false };
        private List<int> RandomNumbers = new List<int>();
        private Timer PauseForStarAnimation;


        public GameWindow1(MainWindow main, WindowState windowState)
        {
            InitializeComponent();
            this.Closing += GameWindow1_Closing;
            this.StateChanged += GameWindow1_StateChanged;
            Main = main;
            State = windowState;
            WindowState = windowState;
            RandomizeNumbers();
        }

        private void RandomizeNumbers()
        {
            if (RandomNumbers != null) RandomNumbers.Clear();
            for (int i = 0; i < 4; i++)
            {
                Random r = new Random();
                int pick;
                do
                {
                    pick = r.Next(0, 4);
                }
                while (RandomNumbers.Contains(pick));
                RandomNumbers.Add(pick);
            }
        }

        private void ShowPictures()
        {
            for (int i = 0; i < 4; i++)
            {
                ImagesOffspings[(Level - 1) * 4 + i].Margin = new Thickness(10);
                Grid.SetColumn(ImagesOffspings[(Level - 1) * 4 + i], i);
                Grid.SetRow(ImagesOffspings[(Level - 1) * 4 + i], 0);
                GameGrid.Children.Add(ImagesOffspings[(Level - 1) * 4 + i]);

                ImagesParent[(Level - 1) * 4 + RandomNumbers[i]].Margin = new Thickness(10);
                Grid.SetColumn(ImagesParent[(Level - 1) * 4 + RandomNumbers[i]], i);
                Grid.SetRow(ImagesParent[(Level - 1) * 4 + RandomNumbers[i]], 2);
                GameGrid.Children.Add(ImagesParent[(Level - 1) * 4 + RandomNumbers[i]]);
            }
        }

        private void LoadImages()
        {
            ImagesOffspings = new Image[12];
            ImagesParent = new Image[12];

            for (int i = 1; i <= 12; i++)
            {
                ImagesOffspings[i - 1] = new Image();
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("Resources/Game1/A" + i.ToString() + ".jpg", UriKind.Relative);
                image.EndInit();
                ImagesOffspings[i-1].Source = image;

                ImagesParent[i - 1] = new Image();
                image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("Resources/Game1/B" + i.ToString() + ".jpg", UriKind.Relative);
                image.EndInit();
                ImagesParent[i-1].Source = image;
                ImagesParent[i - 1].MouseMove += ImageOffspringMouseMove;
            }
        }

        private void ImageOffspringMouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                LastClicked = (Image)sender;
                DataObject data = new DataObject(typeof(ImageSource), ((Image)sender).Source);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Move | DragDropEffects.Copy);
            }
        }

        private void GameWindow1_StateChanged(object sender, EventArgs e)
        {
            State = WindowState;
        }

        private void GameWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Main.WindowState = State;
            Main.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImages();
            ShowPictures();
        }

        private void Target_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ImageSource)))
                if (CheckRightPosition(sender))
                {
                    (sender as Image).Source = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                    LastClicked.Visibility = Visibility.Hidden;
                    CheckEndOfRound();
                }
        }

        private void CheckEndOfRound()
        {
            if (!TargetsEntries.Contains(false))
            {
                StarOpacityAnimation();
                PauseForStarAnimation = new Timer(1000);
                PauseForStarAnimation.Elapsed += PauseForStarAnimation_Elapsed;
                PauseForStarAnimation.Start();
            }
        }

        private bool CheckRightPosition(object sender)
        {
            string temp = LastClicked.Source.ToString();
            temp = temp.Remove(0, temp.LastIndexOf('/')+2);
            temp = temp.Remove(temp.IndexOf('.'));
            int pick = int.Parse(temp);
            bool result = false;
            switch (pick % 4)
            {
                case 1:
                    if (sender as Image == Target1)
                    {
                        result = true;
                        TargetsEntries[0] = true;
                    }
                    break;
                case 2:
                    if (sender as Image == Target2)
                    {
                        result = true;
                        TargetsEntries[1] = true;
                    }
                    break;
                case 3:
                    if (sender as Image == Target3)
                    {
                        result = true;
                        TargetsEntries[2] = true;
                    }
                    break;
                case 0:
                    if (sender as Image == Target4)
                    {
                        result = true;
                        TargetsEntries[3] = true;
                    }
                    break;
                default:
                    break;
            }
            return result;/////////////////////////
        }

        private void NextLevel()
        {
            Level++;
            if (Level == 4)
            {
                MessageBox.Show("END OF GAME!");
                new ModalWindow("END OF GAME!", ModalWindowMode.TextShow).ShowDialog();
                Close();
                return;
            }
            
            ShowPictures();
            RandomizeNumbers();
            TargetsEntries = new bool[] { false, false, false, false };
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(UnknownImage, UriKind.Relative);
            image.EndInit();
            Target1.Source = image;
            Target2.Source = image;
            Target3.Source = image;
            Target4.Source = image;
        }

        private void StarOpacityAnimation()
        {
            double StarAnimationDuration = 0.6;
            DoubleAnimation starAnimation = new DoubleAnimation(1, TimeSpan.FromSeconds(StarAnimationDuration));
            starAnimation.Completed += StarAnimation_Completed;
            switch (Level)
            {
                case 1:
                    Star1.BeginAnimation(OpacityProperty, starAnimation);
                    break;
                case 2:
                    Star2.BeginAnimation(OpacityProperty, starAnimation);
                    break;
                case 3:
                    Star3.BeginAnimation(OpacityProperty, starAnimation);
                    break;
                default:
                    return;
            }
        }

        private void StarAnimation_Completed(object sender, EventArgs e)
        {
            NextLevel();
        }

        private void PauseForStarAnimation_Elapsed(object sender, ElapsedEventArgs e)
        {
            PauseForStarAnimation.Stop();
        }
    }
}
