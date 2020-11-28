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

namespace MiniGames
{
    /// <summary>
    /// Логика взаимодействия для GameWindow1.xaml
    /// </summary>
    public partial class GameWindow1 : Window
    {
        private MainWindow Main;
        private WindowState State;

        private int Level = 1;
        private Image[] ImagesOffspings;
        private Image[] ImagesParent;


        public GameWindow1(MainWindow main, WindowState windowState)
        {
            InitializeComponent();
            this.Closing += GameWindow1_Closing;
            this.StateChanged += GameWindow1_StateChanged;
            Main = main;
            State = windowState;
            WindowState = windowState;

        }

        private void ShowPictures()
        {
            for (int i = 0; i < 4; i++)
            {
                ImagesOffspings[(Level - 1) * 4 + i].Margin = new Thickness(10);
                Grid.SetColumn(ImagesOffspings[(Level - 1) * 4 + i], i);
                Grid.SetRow(ImagesOffspings[(Level - 1) * 4 + i], 0);
                GameGrid.Children.Add(ImagesOffspings[(Level - 1) * 4 + i]);

                ImagesParent[(Level - 1) * 4 + i].Margin = new Thickness(10);
                Grid.SetColumn(ImagesParent[(Level - 1) * 4 + i], i);
                Grid.SetRow(ImagesParent[(Level - 1) * 4 + i], 2);
                GameGrid.Children.Add(ImagesParent[(Level - 1) * 4 + i]);

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
    }
}
