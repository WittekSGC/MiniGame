using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/*
 * CurrentTask - массив из CHAR для хранения буквы картинки в пути к элементу, по ним будет идти далее проверка
 */

namespace MiniGames
{
    /// <summary>
    /// Логика взаимодействия для GameWindow6.xaml
    /// </summary>
    public partial class GameWindow6 : Window
    {
        private MainWindow Main;
        private bool ManualClosing = true;

        private readonly string Alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ"; //без ъыь, но полный
        private string CurrentAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ"; // алфавит для вычеркивания уже использованных букв
        private char[] CurrentTask = new char[6];
        private int Level = 1;

        //объект, сохраняющий последнее нажатие на вариантовую картинку
        private Image LastClickedAnswer;

        public GameWindow6(MainWindow main, WindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
            Main = main;

            GenerateCurrentTask();
            LoadAnswersImages();


            //TEST LAOD ANIMATION CUBIC EASE IN
            Loaded += GameWindow6_Loaded;
        }

        private void GameWindow6_Loaded(object sender, RoutedEventArgs e)
        {
            //TEST LAOD ANIMATION CUBIC EASE IN
            RunTrainAnimation();
        }


        private void TrainImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            /*
        //TEST LAOD ANIMATION CUBIC EASE IN
        ThicknessAnimation Anim = new ThicknessAnimation()
        {
            EasingFunction = new CubicEase()
            {
                EasingMode = EasingMode.EaseIn,
            },
            To = new Thickness(- this.Width - TrainImage.Width, 207, 0, 0),
            Duration = TimeSpan.FromSeconds(3)
        };

        TrainImage.BeginAnimation(MarginProperty, Anim);
            */
            RunOutTrainAnimation();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ManualClosing)
                if (!(bool)new ModalWindow("Вы точно хотите прервать игру?", ModalWindowMode.TextWithYesNoBtn).ShowDialog())
                    e.Cancel = true;
                else
                {
                    Main.WindowState = this.WindowState;
                    Main.Show();
                }
        }

        //Генерация CurrentTask массива
        private void GenerateCurrentTask()
        {
            string temp = Alphabet.Substring((Level - 1) * 3, 3);
            int[] numbers = new int[6] { -1, -1, -1, -1, -1, -1 };

            int counter = 0;
            Random r;

            while (counter < 6)
            {
                r = new Random();
                int NUMBER = r.Next(0, 6);
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i] == NUMBER) NUMBER = -1;
                }

                if (NUMBER != -1)
                {
                    numbers[counter] = NUMBER;
                    counter++;
                }
            }

            for (int i = 0; i < CurrentTask.Length; i++)
            {
                if (numbers[i]>2)
                {
                    while (true)
                    {
                        r = new Random();
                        char LITERAL = Alphabet[r.Next(0, 30)];
                        if (temp.Contains(LITERAL) || CurrentTask.Contains(LITERAL))
                        {
                            continue;
                        }
                        CurrentTask[i] = LITERAL;
                        break;
                    }
                }
                else
                {
                    CurrentTask[i] = temp[numbers[i]];
                }
            }

            //MessageBox.Show(numbers[0].ToString() + numbers[1].ToString() + numbers[2].ToString() + numbers[3].ToString() + numbers[4].ToString() + numbers[5].ToString());
            MessageBox.Show(CurrentTask[0].ToString() + CurrentTask[1] + CurrentTask[2] + CurrentTask[3] + CurrentTask[4] + CurrentTask[5]);
        }

        //Подгрузка картинок по CurrentTask
        private void LoadAnswersImages()
        {
            string path = "Resources/Game6/alphabet/";
            ImageAnswer1.Source = LoadImageByPath(path + CurrentTask[0].ToString().ToUpper() + ".png");
            ImageAnswer2.Source = LoadImageByPath(path + CurrentTask[1].ToString().ToUpper() + ".png");
            ImageAnswer3.Source = LoadImageByPath(path + CurrentTask[2].ToString().ToUpper() + ".png");
            ImageAnswer4.Source = LoadImageByPath(path + CurrentTask[3].ToString().ToUpper() + ".png");
            ImageAnswer5.Source = LoadImageByPath(path + CurrentTask[4].ToString().ToUpper() + ".png");
            ImageAnswer6.Source = LoadImageByPath(path + CurrentTask[5].ToString().ToUpper() + ".png");
        }

        //Загрузка картинок по заданному пути
        private BitmapImage LoadImageByPath(string path)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(path, UriKind.Relative);
            img.EndInit();
            return img;
        }

        //АНИМАЦИИ

        //Анимация появления поезда
        private void RunTrainAnimation()
        {
            ThicknessAnimation Anim = new ThicknessAnimation()
            {
                EasingFunction = new QuadraticEase()
                {
                    EasingMode = EasingMode.EaseIn,
                },
                To = new Thickness(0, 207, 0, 0),
                Duration = TimeSpan.FromSeconds(3)
            };
            Anim.Completed += Anim_Completed;

            TrainImage.Margin = new Thickness(this.ActualWidth, 207, 0, 0);
            TrainImage.BeginAnimation(MarginProperty, Anim);

            TrainImage.MouseLeftButtonUp += TrainImage_MouseLeftButtonUp;
        }
        //окончание анимации появления поезда
        private void Anim_Completed(object sender, EventArgs e)
        {
            RunLiteralAnimation();
            AnswersCardsAnimation();
        }

        //Анимация появления букв
        private void RunLiteralAnimation()
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(400)
            };

            FirstTextBlock.BeginAnimation(OpacityProperty, opacityAnimation);
            SecondTextBlock.BeginAnimation(OpacityProperty, opacityAnimation);
            ThirdTextBlock.BeginAnimation(OpacityProperty, opacityAnimation);
        }

        //Анимация исчезновения букв
        private void RunOutLiteralAnimation()
        {
            //Margin="316,246,410,50" 
            FirstTextBlock.BeginAnimation(MarginProperty, new ThicknessAnimation() {
                To = new Thickness(316 - this.Width, 246, 410, 50),
                Duration = TimeSpan.FromSeconds(3),
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn}
            });
        }

        //запуск анимации появления карточек
        private void AnswersCardsAnimation()
        {
            for (int i = 0; i < 6; i++)
            {
                RunCardAnimation(i);
            }
        }
        //Анимация появления карточек
        private void RunCardAnimation(int N)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(400),
                BeginTime = TimeSpan.FromMilliseconds(400 * N)
            };

            switch (N)
            {
                case 0:
                    ImageAnswer1.BeginAnimation(OpacityProperty, opacityAnimation);
                    break;
                case 1:
                    ImageAnswer2.BeginAnimation(OpacityProperty, opacityAnimation);
                    break;
                case 2:
                    ImageAnswer3.BeginAnimation(OpacityProperty, opacityAnimation);
                    break;
                case 3:
                    ImageAnswer4.BeginAnimation(OpacityProperty, opacityAnimation);
                    break;
                case 4:
                    ImageAnswer5.BeginAnimation(OpacityProperty, opacityAnimation);
                    break;
                case 5:
                    ImageAnswer6.BeginAnimation(OpacityProperty, opacityAnimation);
                    break;
                default:
                    break;
            }
        }

        //Анимация появления слотов для перетаскивания
        private void RunSlotsAnimation(int N)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(400)
            };


        }

        //Анимация  отезда поезда 
        private void RunOutTrainAnimation()
        {
            Storyboard sb = this.FindResource("TrainOut") as Storyboard;
            sb.Begin();
        }

        private void DragDropActivation(object sender, MouseButtonEventArgs e)
        {
            LastClickedAnswer = ((Image)sender);
            DataObject data = new DataObject(typeof(ImageSource), ((Image)sender).Source);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Move | DragDropEffects.Copy);
        }

        private void SlotDragDrop(object sender, DragEventArgs e)
        {
            MessageBox.Show(LastClickedAnswer.Source.ToString().Contains("А.png").ToString());
            (sender as Image).Source = e.Data.GetData(typeof(ImageSource)) as ImageSource;
            (sender as Image).Opacity = 1;
            UpdateLayout();
        }
    }
}
