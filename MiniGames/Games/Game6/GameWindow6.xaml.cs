using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
        private bool WindowLoaded = false;

        //переменные множителей для элементов
        double sXmultiple = 1, sYmultiple = 1;

        private readonly string Alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ"; //без ъыь
        private char[] CurrentTask = new char[6];
        private int Level = 1;
        private readonly int LevelsCount = 10;
        private int RightAnswersCount = 0;

        //объект, сохраняющий последнее нажатие на вариантовую картинку
        private Image LastClickedAnswer;

        private Image[] StarsArray;

        public GameWindow6(MainWindow main, WindowState windowState)
        {
            InitializeComponent();
            this.WindowState = windowState;
            Main = main;

            GenerateCurrentTask();
            LoadAnswersImages();
            AddStars();

            Loaded += GameWindow6_Loaded;
        }

        private void GameWindow6_Loaded(object sender, RoutedEventArgs e)
        {
            WindowLoaded = true;

            SizeMultipler();

            RunTrainAnimation();
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
                if (numbers[i] > 2)
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
        }

        //Подгрузка картинок по CurrentTask
        private void LoadAnswersImages()
        {
            string path = "/Resources/Game6/alphabet/";
            ImageAnswer1.Source = App.LoadImageByPath(path + CurrentTask[0].ToString().ToUpper() + ".png");
            ImageAnswer2.Source = App.LoadImageByPath(path + CurrentTask[1].ToString().ToUpper() + ".png");
            ImageAnswer3.Source = App.LoadImageByPath(path + CurrentTask[2].ToString().ToUpper() + ".png");
            ImageAnswer4.Source = App.LoadImageByPath(path + CurrentTask[3].ToString().ToUpper() + ".png");
            ImageAnswer5.Source = App.LoadImageByPath(path + CurrentTask[4].ToString().ToUpper() + ".png");
            ImageAnswer6.Source = App.LoadImageByPath(path + CurrentTask[5].ToString().ToUpper() + ".png");
        }

        //Подгрузка нужных букв для заданий
        private void LoadLiters()
        {
            FirstTextBlock.Text = Alphabet.Substring(3 * (Level - 1), 1);
            SecondTextBlock.Text = Alphabet.Substring(3 * (Level - 1) + 1, 1);
            ThirdTextBlock.Text = Alphabet.Substring(3 * (Level - 1) + 2, 1);
        }

        //АНИМАЦИИ
        //Анимация появления поезда
        private void RunTrainAnimation()
        {
            LoadLiters();
            //ResetSlotPosition();

            TrainPanel.Margin = new Thickness(0);
            DoubleAnimation Anim = new DoubleAnimation
            {
                From = this.ActualWidth,
                To = 136 * sXmultiple,
                Duration = TimeSpan.FromSeconds(3),
                EasingFunction = new QuadraticEase()
                {
                    EasingMode = EasingMode.EaseOut,
                }
            };
            Anim.Completed += Anim_Completed;

            TrainImage.BeginAnimation(Canvas.LeftProperty, Anim);
        }

        //окончание анимации появления поезда
        private void Anim_Completed(object sender, EventArgs e)
        {
            RunLiteralAnimation();
            AnswersCardsAnimation();
        }

        //Анимация  отезда поезда 
        private void RunOutTrainAnimation()
        {
            //здесь поезд должен уехать
            ThicknessAnimation Anim = new ThicknessAnimation()
            {
                EasingFunction = new QuadraticEase()
                {
                    EasingMode = EasingMode.EaseIn,
                },
                To = new Thickness(-this.ActualWidth, 0, 0, 0),
                Duration = TimeSpan.FromSeconds(3),
                //FillBehavior = FillBehavior.Stop
            };
            Anim.Completed += TrainPanelOuted;
            TrainPanel.BeginAnimation(MarginProperty, Anim);
        }
        private void TrainPanelOuted(object sender, EventArgs e)
        {
            SlotImage1.Source = App.LoadImageByPath("/Resources/Game6/alphabet/!empty.png");
            SlotImage2.Source = App.LoadImageByPath("/Resources/Game6/alphabet/!empty.png");
            SlotImage3.Source = App.LoadImageByPath("/Resources/Game6/alphabet/!empty.png");

            /*
            FirstTextBlock.Opacity = 1;
            SecondTextBlock.Opacity = 1;
            ThirdTextBlock.Opacity = 1;
            */

            FirstTextBlock.BeginAnimation(OpacityProperty, null);
            SecondTextBlock.BeginAnimation(OpacityProperty, null);
            ThirdTextBlock.BeginAnimation(OpacityProperty, null);
            SlotImage1.BeginAnimation(OpacityProperty, null);
            SlotImage2.BeginAnimation(OpacityProperty, null);
            SlotImage3.BeginAnimation(OpacityProperty, null);

            UpdateLayout();
            if (Level <= LevelsCount)
            {
                TrainPanel.BeginAnimation(MarginProperty, null);
                RunTrainAnimation();
            }
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

        //запуск анимации появления карточек
        private void AnswersCardsAnimation()
        {
            for (int i = 0; i < 6; i++)
            {
                RunCardAnimation(i, 1);
            }
        }
        //Анимация последовательного появления карточек
        private void RunCardAnimation(int N, int to)
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                To = to,
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

        //Анимация исчезновения карточек
        private void AnswersCardsOutAnimation()
        {
            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(400)
            };
            opacityAnimation.Completed += AnswersCardsOuted;
            for (int i = 0; i < 6; i++)
            {
                switch (i)
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
        }
        private void AnswersCardsOuted(object sender, EventArgs e)
        {
            LoadAnswersImages();
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

        //возврат поезда в начальную точку уровня
        private void ReturnPositions()
        {
            //поезд и другие элементы должны вернуться в начальные точки
            SlotImage1.Opacity = 0;
            SlotImage2.Opacity = 0;
            SlotImage3.Opacity = 0;
        }


        //Логика перетаскивания элементов
        private void DragDropActivation(object sender, MouseButtonEventArgs e)
        {
            LastClickedAnswer = ((Image)sender);
            DataObject data = new DataObject(typeof(ImageSource), ((Image)sender).Source);
            DragDrop.DoDragDrop(this, data, DragDropEffects.Move | DragDropEffects.Copy);
        }
        private void SlotDragDrop(object sender, DragEventArgs e)
        {
            string sub = Alphabet.Substring((Level - 1) * 3, 3);
            string subName;

            TextBlock Literal;

            switch ((sender as Image).Name)
            {
                case "SlotImage1":
                    subName = sub[0].ToString() + ".png";
                    Literal = FirstTextBlock;
                    break;
                case "SlotImage2":
                    subName = sub[1].ToString() + ".png";
                    Literal = SecondTextBlock;
                    break;
                case "SlotImage3":
                    subName = sub[2].ToString() + ".png";
                    Literal = ThirdTextBlock;
                    break;
                default:
                    throw new Exception("Error on dragdropcheck");
            }

            if (!LastClickedAnswer.Source.ToString().Contains(subName))
                return;

            RightAnswersCount++;

            (sender as Image).Source = e.Data.GetData(typeof(ImageSource)) as ImageSource;
            UpdateLayout();

            DoubleAnimation opacityIncrease = new DoubleAnimation();
            opacityIncrease.To = 1;
            opacityIncrease.Duration = TimeSpan.FromSeconds(0.4);

            DoubleAnimation opacityDecrease = new DoubleAnimation();
            opacityDecrease.To = 0;
            opacityDecrease.Duration = TimeSpan.FromSeconds(0.4);

            (sender as Image).BeginAnimation(OpacityProperty, opacityIncrease);
            LastClickedAnswer.BeginAnimation(OpacityProperty, opacityDecrease);
            Literal.BeginAnimation(OpacityProperty, opacityDecrease);

            if (RightAnswersCount == 3) NextLevel();
        }

        //переход к следующему уровню
        private void NextLevel()
        {
            DoubleAnimation opacityAnim = new DoubleAnimation(1, TimeSpan.FromSeconds(0.4));
            opacityAnim.BeginTime = TimeSpan.FromSeconds(1);
            StarsArray[Level - 1].BeginAnimation(OpacityProperty, opacityAnim);

            RunOutTrainAnimation();
            RightAnswersCount = 0;
            Level++;
            AnswersCardsOutAnimation();

            if (Level <= LevelsCount)
            {
                GenerateCurrentTask();
                ReturnPositions();
            }
            else
            {
                EndOfGame();
            }
        }

        //окончание игры
        private void EndOfGame()
        {
            bool? Result = new ModalWindow("Молодец! Хочешь сыграть еще раз?", ModalWindowMode.TextWithYesNoBtn).ShowDialog();
            if ((bool)Result)
            {
                //вернуть исходные значения по состоянию на начало игры
                Level = 1;
                GenerateCurrentTask();
                LoadAnswersImages();

                StarsPanel.Children.Clear();
                AddStars();

                ReturnPositions();

                TrainPanel.BeginAnimation(MarginProperty, null);
                RunTrainAnimation();
            }
            else
            {
                ManualClosing = false;
                Close();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeMultipler();
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            SizeMultipler();
        }

        //метод для отслеживания и изменения размеров элементов при масштабировании окна
        private void SizeMultipler()
        {
            if (!WindowLoaded) return;

            sXmultiple = ActualWidth / (816 + 30);
            sYmultiple = ActualHeight / (620 + 4);
            if (sXmultiple > sYmultiple)
                sXmultiple = sYmultiple;
            else
                sYmultiple = sXmultiple;

            GameGrid.Height = 389 * sXmultiple;
            GameGrid.Width = 808 * sYmultiple;

            //паровозик
            Canvas.SetTop(TrainImage, 238 * sXmultiple);
            Canvas.SetLeft(TrainImage, 136 * sXmultiple);
            TrainImage.Height = 142 * sXmultiple;
            TrainImage.Width = 520 * sXmultiple;
            TrainImage.BeginAnimation(Canvas.LeftProperty, null);
            if (ImageAnswer1.Opacity == 1)
                RunLiteralAnimation();

            //слоты и буквы
            Canvas.SetTop(SlotImage1, 270 * sXmultiple);
            Canvas.SetLeft(SlotImage1, 314 * sXmultiple);
            SlotImage1.Height = 70 * sXmultiple;
            SlotImage1.Width = 70 * sXmultiple;

            Canvas.SetTop(FirstTextBlock, 268 * sXmultiple);
            Canvas.SetLeft(FirstTextBlock, 316 * sXmultiple);
            FirstTextBlock.Height = 66 * sXmultiple;
            FirstTextBlock.Width = 66 * sXmultiple;
            FirstTextBlock.FontSize = 64 * sXmultiple;

            Canvas.SetTop(SlotImage2, 268 * sXmultiple);
            Canvas.SetLeft(SlotImage2, 440 * sXmultiple);
            SlotImage2.Height = 70 * sXmultiple;
            SlotImage2.Width = 70 * sXmultiple;

            Canvas.SetTop(SecondTextBlock, 266 * sXmultiple);
            Canvas.SetLeft(SecondTextBlock, 442 * sXmultiple);
            SecondTextBlock.Height = 66 * sXmultiple;
            SecondTextBlock.Width = 66 * sXmultiple;
            SecondTextBlock.FontSize = 64 * sXmultiple;

            Canvas.SetTop(SlotImage3, 270 * sXmultiple);
            Canvas.SetLeft(SlotImage3, 566 * sXmultiple);
            SlotImage3.Height = 70 * sXmultiple;
            SlotImage3.Width = 70 * sXmultiple;

            Canvas.SetTop(ThirdTextBlock, 266 * sXmultiple);
            Canvas.SetLeft(ThirdTextBlock, 564 * sXmultiple);
            ThirdTextBlock.Height = 66 * sXmultiple;
            ThirdTextBlock.Width = 66 * sXmultiple;
            ThirdTextBlock.FontSize = 64 * sXmultiple;

            ImageAnswer1.Margin = new Thickness(64 * sXmultiple, 72 * sXmultiple, 660 * sXmultiple, 233 * sXmultiple);
            ImageAnswer1.Width = Math.Truncate((80 * sXmultiple) / 10) * 10;
            ImageAnswer1.Height = Math.Truncate((80 * sXmultiple) / 10) * 10;

            ImageAnswer2.Margin = new Thickness(186 * sXmultiple, 72 * sXmultiple, 538 * sXmultiple, 233 * sXmultiple);
            ImageAnswer2.Width = Math.Truncate((80 * sXmultiple) / 10) * 10;
            ImageAnswer2.Height = Math.Truncate((80 * sXmultiple) / 10) * 10;

            ImageAnswer3.Margin = new Thickness(299 * sXmultiple, 72 * sXmultiple, 425 * sXmultiple, 233 * sXmultiple);
            ImageAnswer3.Width = Math.Truncate((80 * sXmultiple) / 10) * 10;
            ImageAnswer3.Height = Math.Truncate((80 * sXmultiple) / 10) * 10;

            ImageAnswer4.Margin = new Thickness(414 * sXmultiple, 72 * sXmultiple, 310 * sXmultiple, 233 * sXmultiple);
            ImageAnswer4.Width = Math.Truncate((80 * sXmultiple) / 10) * 10;
            ImageAnswer4.Height = Math.Truncate((80 * sXmultiple) / 10) * 10;

            ImageAnswer5.Margin = new Thickness(535 * sXmultiple, 72 * sXmultiple, 189 * sXmultiple, 233 * sXmultiple);
            ImageAnswer5.Width = Math.Truncate((80 * sXmultiple) / 10) * 10;
            ImageAnswer5.Height = Math.Truncate((80 * sXmultiple) / 10) * 10;

            ImageAnswer6.Margin = new Thickness(656 * sXmultiple, 72 * sXmultiple, 68 * sXmultiple, 233 * sXmultiple);
            ImageAnswer6.Width = Math.Truncate((80 * sXmultiple) / 10) * 10;
            ImageAnswer6.Height = Math.Truncate((80 * sXmultiple) / 10) * 10;
        }

        private void AddStars()
        {
            StarsArray = new Image[LevelsCount];

            for (int i = 0; i < LevelsCount; i++)
            {
                StarsArray[i] = new Image
                {
                    Source = App.LoadImageByPath("/Resources/Game2/star.png"),
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
