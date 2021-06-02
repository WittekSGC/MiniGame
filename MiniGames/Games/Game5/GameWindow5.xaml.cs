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
    public partial class GameWindow5 : Window
    {
        MainWindow Main;
        private bool ManualClosing = true;
        private bool WindowLoaded = false;
        int Level = 0, LevelsCount = 3, RightsAnswers = 0;

        int LeftPick = -1, RightPick = -1;
        Canvas LeftLastClickedCanvas, RightLastClickedCanvas;

        //сохраняет порядок загрузки картинок
        int[] LeftImagesOrder, RightImagesOrder;

        //переменные множителей для элементов
        double sXmultiple = 1, sYmultiple = 1;

        //массив звездочек
        private Image[] StarsArray = new Image[3];

        //массив чекеров для адаптации
        private List<Image> CheckersList = new List<Image>();

        public GameWindow5(MainWindow main, WindowState state)
        {
            InitializeComponent();
            this.WindowState = state;
            Main = main;
        }

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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeMultipler();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowLoaded = true;
            LoadLevel();
            AddStars();
        }

        //метод, который загружает уровень
        private void LoadLevel()
        {
            Level++;
            PicturesLoad();
            AddClickGrid();
            SizeMultipler();
        }

        //метод, который выполняет действия по загрузке следующего уровня
        private void NextLevel()
        {
            if (Level > 0) EndOfLevel();

            if (Level != LevelsCount)
            {
                LeftClickGrid.Children.Clear();
                RightClickGrid.Children.Clear();
                LeftPick = -1;
                RightPick = -1;
                LeftLastClickedCanvas = null;
                RightLastClickedCanvas = null;

                LoadLevel();
            }
            else EndOfGame();

        }

        //действия по окончанию уровня
        private void EndOfLevel()
        {
            DoubleAnimation showStar = new DoubleAnimation(1, TimeSpan.FromSeconds(0.6));
            StarsArray[Level - 1].BeginAnimation(OpacityProperty, showStar);
        }

        //метод действий по окончанию всех уровней
        private void EndOfGame()
        {
            DoubleAnimation opacityAnim = new DoubleAnimation(1, TimeSpan.FromSeconds(0.4));

            LeftLastClickedCanvas.MouseLeftButtonUp -= LeftCanvas_MouseLeftButtonUp;
            LeftLastClickedCanvas.MouseEnter -= LeftCanvas_MouseEnter;
            LeftLastClickedCanvas.MouseLeave -= LeftCanvas_MouseLeave;
            LeftLastClickedCanvas.BeginAnimation(OpacityProperty, opacityAnim);
            LeftLastClickedCanvas.Cursor = Cursors.Arrow;
            (LeftLastClickedCanvas.Parent as Border).BorderThickness = new Thickness(0);

            RightLastClickedCanvas.MouseLeftButtonUp -= RightCanvas_MouseLeftButtonUp;
            RightLastClickedCanvas.MouseEnter -= RightCanvas_MouseEnter;
            RightLastClickedCanvas.MouseLeave -= RightCanvas_MouseLeave;
            RightLastClickedCanvas.BeginAnimation(OpacityProperty, opacityAnim);
            RightLastClickedCanvas.Cursor = Cursors.Arrow;
            (RightLastClickedCanvas.Parent as Border).BorderThickness = new Thickness(0);

            bool? Result = new ModalWindow("Молодец! Хочешь сыграть еще раз?", ModalWindowMode.TextWithYesNoBtn).ShowDialog();
            if ((bool)Result)
            {
                //вернуть исходные значения по состоянию на начало игры
                LeftClickGrid.Children.Clear();
                RightClickGrid.Children.Clear();
                LeftPick = -1;
                RightPick = -1;
                LeftLastClickedCanvas = null;
                RightLastClickedCanvas = null;
                Level = 0;

                StarsPanel.Children.Clear();
                AddStars();

                //прогрузить 1ый уровень
                LoadLevel();
            }
            else
            {
                ManualClosing = false;
                Close();
            }
        }

        //два метода для создания случайных массивов, помогающих расположить цветки каждый раз по разному
        private int[] RandomNumberCreator(int start, int end)
        {
            if (start > end) return null;

            int arrLength = end - start + 1;
            int[] resArr = new int[arrLength];

            int counter = 0;
            Random r;

            if (arrLength > 0)
                while (counter < arrLength)
                {
                    r = new Random();
                    int temp = r.Next(start, end + 1);
                    if (!CheckNumbInArr(resArr, temp))
                    {
                        resArr[counter] = temp;
                        counter++;
                    }
                }
            else
                resArr[0] = end;

            return resArr;
        }
        private bool CheckNumbInArr(int[] arr, int numb)
        {
            bool res = false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == numb)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        //метод, который создает звездочки прогресса
        private void AddStars()
        {
            BitmapImage imgSrc = App.LoadImageByPath("/Resources/Game2/star.png");

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

        //метод, загружающий и отображающий случайные картинки
        private void PicturesLoad()
        {
            LeftImagesOrder = RandomNumberCreator(1, 3);
            RightImagesOrder = RandomNumberCreator(1, 3);

            //путь создается из basePathFront + Level + / + элемент одного из массивов + -1/-2 + basePathEnd
            string basePathFront = "/Resources/Game5/lvl";
            string basePathEnd = ".png";

            LeftImage1.Source = App.LoadImageByPath(basePathFront + Level.ToString() + "/" + LeftImagesOrder[0] + "-1" + basePathEnd);
            LeftImage2.Source = App.LoadImageByPath(basePathFront + Level.ToString() + "/" + LeftImagesOrder[1] + "-1" + basePathEnd);
            LeftImage3.Source = App.LoadImageByPath(basePathFront + Level.ToString() + "/" + LeftImagesOrder[2] + "-1" + basePathEnd);

            RightImage1.Source = App.LoadImageByPath(basePathFront + Level.ToString() + "/" + RightImagesOrder[0] + "-2" + basePathEnd);
            RightImage2.Source = App.LoadImageByPath(basePathFront + Level.ToString() + "/" + RightImagesOrder[1] + "-2" + basePathEnd);
            RightImage3.Source = App.LoadImageByPath(basePathFront + Level.ToString() + "/" + RightImagesOrder[2] + "-2" + basePathEnd);
        }

        //метод создания сетки для нажатий
        private void AddClickGrid()
        {
            //создание сетки для левой части
            for (int i = 0; i < LevelsCount; i++)
            {
                Border border = new Border
                {
                    BorderThickness = new Thickness(0),
                    BorderBrush = Brushes.Gold
                };
                Grid.SetColumn(border, i);
                Canvas canvas = new Canvas
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Background = new SolidColorBrush(Color.FromArgb(122, 0, 0, 0)),
                    Opacity = 0,
                    Cursor = Cursors.Hand
                };
                Image checker = new Image()
                {
                    Name = "LeftCheck" + i.ToString(),
                    Source = App.LoadImageByPath("/Resources/Game5/check.png"),
                    Width = 90,
                    Height = 76,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                if (i != 0)
                    checker.Margin = new Thickness(7, 100, 0, 0);
                else
                    checker.Margin = new Thickness(-1, 100, 0, 0);

                CheckersList.Add(checker);

                canvas.MouseEnter += LeftCanvas_MouseEnter;
                canvas.MouseLeave += LeftCanvas_MouseLeave;
                canvas.MouseLeftButtonUp += LeftCanvas_MouseLeftButtonUp;
                border.Child = canvas;
                canvas.Children.Add(checker);
                LeftClickGrid.Children.Add(border);
            }

            //создание сетки для правой части
            for (int i = 0; i < LevelsCount; i++)
            {
                Border border = new Border
                {
                    BorderThickness = new Thickness(0),
                    BorderBrush = Brushes.Gold
                };
                Grid.SetColumn(border, i);
                Canvas canvas = new Canvas
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    Background = new SolidColorBrush(Color.FromArgb(122, 0, 0, 0)),
                    Opacity = 0,
                    Cursor = Cursors.Hand
                };
                Image checker = new Image()
                {
                    Name = "RightCheck" + i.ToString(),
                    Source = App.LoadImageByPath("/Resources/Game5/check.png"),
                    Width = 90,
                    Height = 76,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                if (i != 0)
                    checker.Margin = new Thickness(7, 100, 0, 0);
                else
                    checker.Margin = new Thickness(-1, 100, 0, 0);

                CheckersList.Add(checker);

                canvas.MouseEnter += RightCanvas_MouseEnter;
                canvas.MouseLeave += RightCanvas_MouseLeave;
                canvas.MouseLeftButtonUp += RightCanvas_MouseLeftButtonUp;
                border.Child = canvas;
                canvas.Children.Add(checker);
                RightClickGrid.Children.Add(border);
            }
        }

        //метод обработки нажатия на соотвествующий Canvas слева
        private void LeftCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border border = (sender as Canvas).Parent as Border;

            //нажатие на нажатый canvas снимает выделение
            if (LeftLastClickedCanvas == sender as Canvas)
            {
                LeftPick = -1;
                border.BorderThickness = new Thickness(2 * sXmultiple);
                LeftLastClickedCanvas = null;
                return;
            }

            //если выбора уже был
            if (LeftPick != -1) (LeftLastClickedCanvas.Parent as Border).BorderThickness = new Thickness(0 * sXmultiple);

            LeftPick = Grid.GetColumn(border);
            border.BorderThickness = new Thickness(5 * sXmultiple);
            LeftLastClickedCanvas = sender as Canvas;

            CheckAnswer();
        }

        //метод обработки нажатия на соотвествующий Canvas справа
        private void RightCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border border = (sender as Canvas).Parent as Border;

            //нажатие на нажатый canvas снимает выделение
            if (RightLastClickedCanvas == sender as Canvas)
            {
                RightPick = -1;
                border.BorderThickness = new Thickness(2 * sXmultiple);
                RightLastClickedCanvas = null;
                return;
            }

            //если выбора уже был
            if (RightPick != -1) (RightLastClickedCanvas.Parent as Border).BorderThickness = new Thickness(0 * sXmultiple);

            RightPick = Grid.GetColumn(border);
            border.BorderThickness = new Thickness(5 * sXmultiple);
            RightLastClickedCanvas = sender as Canvas;

            CheckAnswer();
        }

        //анимация при наведении на Canvas
        private void LeftCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            Border border = canvas.Parent as Border;

            if (canvas == LeftLastClickedCanvas)
            {
                border.BorderThickness = new Thickness(5 * sXmultiple);
            }
            else
            {
                border.BorderThickness = new Thickness(2 * sXmultiple);
            }
        }
        private void RightCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            Border border = canvas.Parent as Border;

            if (canvas == RightLastClickedCanvas)
            {
                border.BorderThickness = new Thickness(5 * sXmultiple);
            }
            else
            {
                border.BorderThickness = new Thickness(2 * sXmultiple);
            }
        }

        //анимация при сведении курсора c Canvas
        private void LeftCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            Border border = canvas.Parent as Border;
            if (canvas == LeftLastClickedCanvas)
            {
                border.BorderThickness = new Thickness(5 * sXmultiple);
            }
            else
            {
                border.BorderThickness = new Thickness(0 * sXmultiple);
            }
        }
        private void RightCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            Border border = canvas.Parent as Border;
            if (canvas == RightLastClickedCanvas)
            {
                border.BorderThickness = new Thickness(5 * sXmultiple);
            }
            else
            {
                border.BorderThickness = new Thickness(0 * sXmultiple);
            }
        }

        //метод для отслеживания и изменения размеров элементов при масштабировании окна
        private void SizeMultipler()
        {
            if (!WindowLoaded) return;

            sXmultiple = ActualWidth / (800 + 30);
            sYmultiple = ActualHeight / (450 + 4);
            if (sXmultiple > sYmultiple)
                sXmultiple = sYmultiple;
            else
                sYmultiple = sXmultiple;

            LeftBackgroundImage.Width = 300 * sXmultiple;
            LeftBackgroundImage.Height = 200 * sYmultiple;

            LeftImage1.Width = 90 * sXmultiple;
            LeftImage1.Height = 76 * sYmultiple;
            LeftImage1.Margin = new Thickness(44 * sXmultiple, 30 * sYmultiple, 255 * sXmultiple, 113 * sYmultiple);

            LeftImage2.Width = 90 * sXmultiple;
            LeftImage2.Height = 76 * sYmultiple;
            LeftImage2.Margin = new Thickness(150 * sXmultiple, 50 * sYmultiple, 156 * sXmultiple, 95 * sYmultiple);

            LeftImage3.Width = 90 * sXmultiple;
            LeftImage3.Height = 76 * sYmultiple;
            LeftImage3.Margin = new Thickness(242 * sXmultiple, 23 * sYmultiple, 64 * sXmultiple, 121 * sYmultiple);

            LeftClickGrid.Width = 300 * sXmultiple;
            LeftClickGrid.Height = 200 * sYmultiple;
            LeftClickGrid.Margin = new Thickness(48 * sXmultiple, 10 * sYmultiple, 48 * sXmultiple, 9 * sYmultiple);


            RightBackgroundImage.Width = 300 * sXmultiple;
            RightBackgroundImage.Height = 200 * sYmultiple;
            RightBackgroundImage.Margin = new Thickness(31 * sXmultiple, 10 * sYmultiple, 48 * sXmultiple, 9 * sYmultiple);


            RightImage1.Width = 90 * sXmultiple;
            RightImage1.Height = 76 * sYmultiple;
            RightImage1.Margin = new Thickness(31 * sXmultiple, 30 * sYmultiple, 258 * sXmultiple, 113 * sYmultiple);

            RightImage2.Width = 90 * sXmultiple;
            RightImage2.Height = 76 * sYmultiple;
            RightImage2.Margin = new Thickness(136 * sXmultiple, 50 * sYmultiple, 153 * sXmultiple, 95 * sYmultiple);

            RightImage3.Width = 90 * sXmultiple;
            RightImage3.Height = 76 * sYmultiple;
            RightImage3.Margin = new Thickness(231 * sXmultiple, 23 * sYmultiple, 58 * sXmultiple, 121 * sYmultiple);

            RightClickGrid.Width = 300 * sXmultiple;
            RightClickGrid.Height = 200 * sYmultiple;
            RightClickGrid.Margin = new Thickness(31 * sXmultiple, 10 * sYmultiple, 48 * sXmultiple, 9 * sYmultiple);

            foreach (Image item in CheckersList)
            {
                double width = 90, height = 76; //1.185 width = 1 height
                double leftMargin = 7;
                if (item.Name == "LeftCheck0" || item.Name == "RightCheck0")
                {
                    leftMargin = 1;
                }
                item.Width = width * sXmultiple;
                item.Height = height * sYmultiple;
                item.Margin = new Thickness(leftMargin * sXmultiple, 100 * sYmultiple, 0, 0);
            }
        }

        //метод, проверяющий правильность нажатия на Canvas
        private void CheckAnswer()
        {
            if (CheckTrueAnswer())
            {
                RightsAnswers++;

                if (RightsAnswers == LevelsCount)
                {
                    NextLevel();
                    RightsAnswers = 0;
                    return;
                }

                DoubleAnimation opacityAnim = new DoubleAnimation(1, TimeSpan.FromSeconds(0.4));

                LeftLastClickedCanvas.MouseLeftButtonUp -= LeftCanvas_MouseLeftButtonUp;
                LeftLastClickedCanvas.MouseEnter -= LeftCanvas_MouseEnter;
                LeftLastClickedCanvas.MouseLeave -= LeftCanvas_MouseLeave;
                LeftLastClickedCanvas.BeginAnimation(OpacityProperty, opacityAnim);
                LeftLastClickedCanvas.Cursor = Cursors.Arrow;
                (LeftLastClickedCanvas.Parent as Border).BorderThickness = new Thickness(0);

                RightLastClickedCanvas.MouseLeftButtonUp -= RightCanvas_MouseLeftButtonUp;
                RightLastClickedCanvas.MouseEnter -= RightCanvas_MouseEnter;
                RightLastClickedCanvas.MouseLeave -= RightCanvas_MouseLeave;
                RightLastClickedCanvas.BeginAnimation(OpacityProperty, opacityAnim);
                RightLastClickedCanvas.Cursor = Cursors.Arrow;
                (RightLastClickedCanvas.Parent as Border).BorderThickness = new Thickness(0);

                LeftPick = -1;
                RightPick = -1;
            }
            else
            if (LeftPick != -1 && RightPick != -1)
            {
                LeftPick = -1;
                RightPick = -1;

                Canvas TempLeft = LeftLastClickedCanvas;
                LeftLastClickedCanvas = null;
                Canvas TempRight = RightLastClickedCanvas;
                RightLastClickedCanvas = null;

                LeftCanvas_MouseLeave(TempLeft, new MouseEventArgs(Mouse.PrimaryDevice, 0));
                RightCanvas_MouseLeave(TempRight, new MouseEventArgs(Mouse.PrimaryDevice, 0));
            }
        }
        private bool CheckTrueAnswer()
        {
            if (LeftPick == -1 || RightPick == -1) return false;

            if (LeftImagesOrder[LeftPick] == RightImagesOrder[RightPick]) return true;
            else return false;
        }
    }
}
