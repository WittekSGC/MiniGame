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

namespace MiniGames.Games.Game8.Games
{
    /// <summary>
    /// Логика взаимодействия для Fruits.xaml
    /// </summary>
    public partial class Fruits : Window
    {
        private GameWindow8 ParentWindow;
        private bool ManualClosing = true;
        private object[][] Levels;
        private Image[] StarsArray = new Image[5];
        private Image CurrentAnswer;
        private int Level = 0, LevelsCount = 5;


        public Fruits(GameWindow8 parent)
        {
            InitializeComponent();

            Closed += Fruits_Closed;
            Closing += Fruits_Closing;
            Loaded += Fruits_Loaded; ;

            ParentWindow = parent;
            WindowState = ParentWindow.WindowState;

            CreateLevels();
        }

        private void Fruits_Loaded(object sender, RoutedEventArgs e)
        {
            SizeChanged += Fruits_SizeChanged; ;
            Fruits_SizeChanged(null, null);
            AddStars();
            NextLevel();
        }

        private void Fruits_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int fontSize = 24; //начальное значение
            if (ActualHeight > 600 && ActualWidth > 900)
            {
                fontSize = 32;
            }

            if (ActualHeight > 700 && ActualWidth > 1000)
            {
                fontSize = 40;
            }

            if (ActualHeight > 800 && ActualWidth > 1100)
            {
                fontSize = 48;
            }

            if (ActualHeight > 900 && ActualWidth > 1200)
            {
                fontSize = 56;
            }

            TextBlock.SetFontSize(tbAnswer, fontSize);
            TextBlock.SetFontSize(tbQuestion1, fontSize);
            TextBlock.SetFontSize(tbQuestion2, fontSize);
            TextBlock.SetFontSize(tbQuestion3, fontSize);
            TextBlock.SetFontSize(tbQuestion4, fontSize);
            TextBlock.SetFontSize(tbQuestion5, fontSize);
        }

        private void Fruits_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ManualClosing)
                if (!(bool)new ModalWindow("Вы точно хотите прервать игру?", ModalWindowMode.TextWithYesNoBtn).ShowDialog())
                {
                    e.Cancel = true;
                    return;
                }
            ParentWindow.WindowState = WindowState;
            ParentWindow.Show();
        }

        private void Fruits_Closed(object sender, EventArgs e)
        {
            ParentWindow.Show();
        }

        private void EndOfGame()
        {
            bool? Result = new ModalWindow("Молодец! Хочешь сыграть еще раз?", ModalWindowMode.TextWithYesNoBtn).ShowDialog();
            if ((bool)Result)
            {
                //вернуть исходные значения по состоянию на начало игры
                Level = 0;
                tbAnswer.BeginAnimation(OpacityProperty, null);
                tbQuestion5.BeginAnimation(OpacityProperty, null);
                foreach (Image item in StarsArray)
                {
                    item.BeginAnimation(OpacityProperty, null);
                }
                NextLevel();
            }
            else
            {
                ManualClosing = false;
                Close();
            }
        }
        private void CreateLevels()
        {
            Levels = new object[5][] {
                new object[]
                {
                    tbQuestion1,    //первый вопрос
                    imgAnswer2,
                    "Арбуз"
                },
                new object[]
                {
                    tbQuestion2,    //второй вопрос
                    imgAnswer5,
                    "Яблоко"
                },
                new object[]
                {
                    tbQuestion3,    //третий вопрос
                    imgAnswer3,
                    "Банан"
                },
                new object[]
                {
                    tbQuestion4,    //четвертый вопрос
                    imgAnswer1,
                    "Апельсин"
                },
                new object[]
                {
                    tbQuestion5,    //пятый вопрос
                    imgAnswer4,
                    "Груша"
                }
            };
        }

        private void NextLevel()
        {
            //убрать окно и событие предыдущего уровня
            if (Level > 0 && Level < LevelsCount)
            {
                DoubleAnimation opacityDown = new DoubleAnimation(0, TimeSpan.FromSeconds(1));
                opacityDown.BeginTime = TimeSpan.FromSeconds(2);
                (Levels[Level - 1][0] as TextBlock).BeginAnimation(OpacityProperty, opacityDown);

                opacityDown.Completed += SwitchAnswerText;
                opacityDown.BeginTime = TimeSpan.FromSeconds(2);
                tbAnswer.BeginAnimation(OpacityProperty, opacityDown);
            }

            Level++;

            //Если Level превысил количество уровней - окончить игру
            if (Level > LevelsCount)
            {
                EndOfGame();
                return;
            }

            CurrentAnswer = Levels[Level - 1][1] as Image;
            CurrentAnswer.MouseLeftButtonUp += CurrentAnswer_MouseLeftButtonUp;

            DoubleAnimation opacityUp = new DoubleAnimation(1, TimeSpan.FromSeconds(1));
            if (Level != 1)
                opacityUp.BeginTime = TimeSpan.FromSeconds(3);
            (Levels[Level - 1][0] as TextBlock).BeginAnimation(OpacityProperty, opacityUp);
        }

        private void CurrentAnswer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CurrentAnswer.MouseLeftButtonUp -= CurrentAnswer_MouseLeftButtonUp;
            DoubleAnimation opacityUp = new DoubleAnimation(1, TimeSpan.FromSeconds(1));
            opacityUp.Completed += Wait1secondAfterAnimation;
            tbAnswer.BeginAnimation(OpacityProperty, opacityUp);
        }

        private void SwitchAnswerText(object sender, EventArgs e)
        {
            tbAnswer.Text = Levels[Level - 1][2] as string;
        }

        private void AddStars()
        {
            BitmapImage imgSrc = LoadImageByPath("/Resources/Game2/star.png");

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

        private BitmapImage LoadImageByPath(string path)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(path, UriKind.Relative);
            img.EndInit();
            return img;
        }

        private void Wait1secondAfterAnimation(object sender, EventArgs e)
        {
            DoubleAnimation opacityUp = new DoubleAnimation(1, TimeSpan.FromSeconds(1));
            StarsArray[Level - 1].BeginAnimation(OpacityProperty, opacityUp);
            NextLevel();
        }
    }
}
