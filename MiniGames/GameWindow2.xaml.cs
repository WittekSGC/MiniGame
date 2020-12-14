using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для GameWindow2.xaml
    /// </summary>
    public partial class GameWindow2 : Window
    {
        private Image[] StarsArray = new Image[10];
        private MainWindow Main;
        private GameLevel[] gameLevels = new GameLevel[10];
        private int Level = -1;
        private bool ManualClosing = true;

        public GameWindow2(MainWindow main, WindowState state)
        {
            InitializeComponent();
            Main = main;
            this.WindowState = state;
            PaintCircles();
            AddStars();
            InitializeLevels();
            NextLevel();
            LeftImage.MouseLeftButtonUp += ImageClickEventHandler;
            RightImage.MouseLeftButtonUp += ImageClickEventHandler;
        }

        private void ImageClickEventHandler(object sender, MouseButtonEventArgs e)
        {
            CheckTrueAnswer(sender);
        }

        private void CheckTrueAnswer(object sender)
        {
            if ((sender as Image).Equals(LeftImage))
                if (gameLevels[Level].First)
                {
                    EndOfLevel();
                    return;
                }

            if ((sender as Image).Equals(RightImage))
                if (gameLevels[Level].Second)
                {
                    EndOfLevel();
                    return;
                }
        }

        private void EndOfLevel()
        {
            DoubleAnimation showStar = new DoubleAnimation(1, TimeSpan.FromSeconds(0.6));
            showStar.Completed += ShowStar_Completed;
            StarsArray[Level].BeginAnimation(OpacityProperty, showStar);
        }

        private void ShowStar_Completed(object sender, EventArgs e)
        {
            NextLevel();
        }


        private void NextLevel()
        {
            Level++;

            if (Level == 10)
            {
                EndOfGame();
                return;
            }

            BitmapImage imgSrc = new BitmapImage();
            imgSrc.BeginInit();
            imgSrc.UriSource = new Uri(gameLevels[Level].ImagePath1, UriKind.Relative);
            imgSrc.EndInit();
            LeftImage.Source = imgSrc;
            imgSrc = new BitmapImage();
            imgSrc.BeginInit();
            imgSrc.UriSource = new Uri(gameLevels[Level].ImagePath2, UriKind.Relative);
            imgSrc.EndInit();
            RightImage.Source = imgSrc;
            QuestionLabel.Text = gameLevels[Level].Question;
        }

        private void EndOfGame()
        {
            bool? Result = new ModalWindow("Молодец! Хочешь сыграть еще раз?", ModalWindowMode.TextWithYesNoBtn).ShowDialog();
            if ((bool)Result)
            {
                for (int i = 0; i < 10; i++)
                {
                    StarsArray[i].Opacity = 0.2;
                }
                StarsStackPanel.Children.Clear();
                Level = -1;
                NextLevel();
                AddStars();
            }
            else
            {
                ManualClosing = false;
                Close();
                return;
            }
        }

        private void InitializeLevels()
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open("newFile.bin", FileMode.Open)))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        string a = reader.ReadString();
                        bool b = reader.ReadBoolean();
                        string c = reader.ReadString();
                        string d = reader.ReadString();
                        gameLevels[i] = new GameLevel(a, b, c, d);
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show("Произошла ошибка: " + x.Message);
            }
        }

        private void AddStars()
        {
            BitmapImage imgSrc = new BitmapImage();
            imgSrc.BeginInit();
            imgSrc.UriSource = new Uri("/Resources/Game2/star.png", UriKind.Relative);
            imgSrc.EndInit();
            for (int i = 0; i < 10; i++)
            {
                StarsArray[i] = new Image();
                StarsArray[i].Source = imgSrc;
                StarsArray[i].Width = 50;
                StarsArray[i].Height = 50;
                StarsArray[i].Opacity = 0.2;
                StarsArray[i].Margin = new Thickness(0, 0, 4, 0);
                StarsStackPanel.Children.Add(StarsArray[i]);
            }
        }

        private void PaintCircles()
        {
            for (int i = 0; i < 100; i++)
            {
                Ellipse el = new Ellipse();
                el.Height = 10;
                el.Width = 10;
                el.Margin = new Thickness(0, 2, 0, 2);
                el.Fill = new SolidColorBrush(Color.FromRgb(255, 69, 142));
                Circles.Children.Add(el);
            }
        }

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
    }
}
