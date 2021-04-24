using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiniGames
{
    public partial class GameWindow3 : Window
    {
        MainWindow Main;
        bool ManualClosing = true;
        Image[] TargetImages = new Image[16];
        Image[] Slots = new Image[16];
        Image LastClicked;
        readonly string NullImage = "/Resources/Game3/nullImage.png";
        BitmapImage NullImg;

        public GameWindow3(MainWindow main, WindowState state)
        {
            InitializeComponent();
            Main = main;
            WindowState = state;
            InitializeTargetImages();
            CreateSlots();
        }

        private void CreateSlots()
        {
            int counter = 0;
            NullImg = new BitmapImage();
            try
            {
                NullImg.BeginInit();
                NullImg.UriSource = new Uri(NullImage, UriKind.Relative);
                NullImg.EndInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            for (int i = 3; i <= 6; i++)
            {
                for (int j = 3; j <= 6; j++)
                {
                    string name = "";
                    switch (j)
                    {
                        case 3:
                            name += "blue";
                            break;
                        case 4:
                            name += "green";
                            break;
                        case 5:
                            name += "yellow";
                            break;
                        case 6:
                            name += "red";
                            break;
                        default:
                            break;
                    }
                    switch (i)
                    {
                        case 3:
                            name += "ellipse";
                            break;
                        case 4:
                            name += "rectangle";
                            break;
                        case 5:
                            name += "rhomb";
                            break;
                        case 6:
                            name += "triangle";
                            break;
                        default:
                            break;
                    }
                    Slots[counter] = new Image()
                    {
                        Name = name,
                        AllowDrop = true,
                        Margin = new Thickness(15), 
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Source = NullImg,
                    };
                    Grid.SetColumn(Slots[counter], i);
                    Grid.SetRow(Slots[counter], j);
                    Slots[counter].Drop += SlotDrop;
                    int ret = GameGrid.Children.Add(Slots[counter]);
                    //MessageBox.Show(ret.ToString());
                    counter++;
                }
            }
        }

        private void SlotDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ImageSource)))
            {
                if ((sender as Image).Source.ToString().Contains(NullImage))
                {
                    if ((sender as Image).Parent is Grid)
                    {
                        //MessageBox.Show((!(sender as Image).Source.ToString().Contains(NullImage)).ToString() + (sender as Image).Source.ToString());
                        
                        LastClicked.Source = NullImg;
                    }
                    (sender as Image).Source = e.Data.GetData(typeof(ImageSource)) as ImageSource;
                    (sender as Image).MouseMove += TargetImageMove;
                    TargetImagesWrapPanel.Children.Remove(LastClicked);
                }
                else
                {
                    //MessageBox.Show("Else");
                    //извлечение имени текущего элемента
                    string temp = (sender as Image).Source.ToString();
                    temp = temp.Remove(0, temp.LastIndexOf('/') + 1);
                    temp = temp.Remove(temp.IndexOf('.'));

                    (sender as Image).Source = e.Data.GetData(typeof(ImageSource)) as ImageSource;

                    // создание изображения для замены
                    BitmapImage imgSrc = new BitmapImage();
                    string path = "/Resources/Game3/";
                    imgSrc.BeginInit();
                    imgSrc.UriSource = new Uri(path+temp+".png", UriKind.Relative);
                    imgSrc.EndInit();
                    Image swap = new Image()
                    {
                        Source = imgSrc,
                        Margin = new Thickness(10),
                        Width = 64,
                        Height = 64,
                        Cursor = Cursors.Hand
                    };
                    swap.MouseMove += TargetImageMove;

                    //добавление замененного элемента в список и удаление занесенного
                    TargetImagesWrapPanel.Children.Add(swap);
                    TargetImagesWrapPanel.Children.Remove(LastClicked);
                    LastClicked.Source = NullImg;
                }
            }
        }

        private void CheckEndOfRound()
        {
            if (TargetImagesWrapPanel.Children.Count == 0)
            {
                bool? Result = new ModalWindow("Молодец! Хочешь сыграть еще раз?", ModalWindowMode.TextWithYesNoBtn).ShowDialog();
                if ((bool)Result)
                {
                    RemoveSlots();
                    InitializeTargetImages();
                    CreateSlots();
                }
                else
                {
                    ManualClosing = false;
                    Close();
                }
            }

        }

        private void RemoveSlots()
        {
            foreach (Image item in Slots)
            {
                GameGrid.Children.Remove(item);
            }
            Slots = new Image[16];
        }

        private bool CheckRightPositions(int I, int J) //private bool
        {
            int counter = 0;
            bool end = false;

            for (int i = 0; i <4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (I == i && J == j)
                    {
                        end = true;
                        break;
                    }
                    counter++;
                }
                if (end)
                    break;
            }

            Image check = GameGrid.Children[counter + 24] as Image;
            if (check.Source.ToString().Contains(check.Name))
                return true;
            else
                return false;
        }

        private void InitializeTargetImages()
        {
            BitmapImage imgSrc;
            string path;
            int counter = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    path = "/Resources/Game3/";
                    switch (i)
                    {
                        case 0:
                            path += "red";
                            break;
                        case 1:
                            path += "green";
                            break;
                        case 2:
                            path += "blue";
                            break;
                        case 3:
                            path += "yellow";
                            break;
                        default:
                            break;
                    }

                    switch (j)
                    {
                        case 0:
                            path += "rhomb";
                            break;
                        case 1:
                            path += "rectangle";
                            break;
                        case 2:
                            path += "ellipse";
                            break;
                        case 3:
                            path += "triangle";
                            break;
                        default:
                            break;
                    }

                    path += ".png";

                    imgSrc = new BitmapImage();
                    imgSrc.BeginInit();
                    imgSrc.UriSource = new Uri(path, UriKind.Relative);
                    imgSrc.EndInit();

                    Random r = new Random();

                    TargetImages[counter] = new Image()
                    {
                        Source = imgSrc,
                        Margin = new Thickness(10),
                        Width = 64,
                        Height = 64,
                        Cursor = Cursors.Hand
                    };
                    TargetImages[counter].MouseMove += TargetImageMove;
                    counter++;
                }
            }

            List<int> RandomNumbers = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                Random r = new Random();
                int number = r.Next(0, 16);
                while (RandomNumbers.Contains(number))
                    number = r.Next(0, 16);
                RandomNumbers.Add(number);
            }

            for (int i = 0; i < 16; i++)
            {
                TargetImagesWrapPanel.Children.Add(TargetImages[RandomNumbers[i]]);
            }
        }

        private void TargetImageMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                LastClicked = (Image)sender;
                DataObject data = new DataObject(typeof(ImageSource), ((Image)sender).Source);
                DragDrop.DoDragDrop(this, data, DragDropEffects.Move | DragDropEffects.Copy);
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
            else
            {
                Main.WindowState = WindowState;
                Main.Show();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RecreateSize();
        }

        private void RecreateSize()
        {
            int NextWidthTarget;
            int NextSizeColumn;
            if (ActualWidth > 1200) NextWidthTarget = 96;
            else NextWidthTarget = 64;
            if (!(TargetImages[0].Width == NextWidthTarget))
            {
                foreach (Image item in TargetImages)
                {
                    item.Width = NextWidthTarget;
                    item.Height = NextWidthTarget;
                }
            }
            if (ActualWidth > 1200 && ActualHeight > 750)
                NextSizeColumn = 120;
            else
                NextSizeColumn = 80;

            if (GameGrid.ColumnDefinitions[1].Width.Value != NextSizeColumn)
            {
                foreach (ColumnDefinition item in GameGrid.ColumnDefinitions)
                {
                    if (item.Width.Value == 80 || item.Width.Value == 120)
                        item.Width = new GridLength(NextSizeColumn);
                }
                foreach (RowDefinition item in GameGrid.RowDefinitions)
                {
                    if (item.Height.Value == 80 || item.Height.Value == 120)
                        item.Height = new GridLength(NextSizeColumn);
                }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            RecreateSize();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool b;
            bool endGame = true;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    b = CheckRightPositions(i, j);

                    if (!b) { 
                        ReturnFigureToPanel(i, j);
                        endGame = false;
                    }
                }
            }

            if (endGame)
                CheckEndOfRound();
        }

        private void ReturnFigureToPanel(int I, int J)
        {
            int counter = 0;
            bool end = false;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (I == i && J == j)
                    {
                        end = true;
                        break;
                    }
                    counter++;
                }
                if (end)
                    break;
            }

            //извлечение имени текущего элемента
            string temp = (GameGrid.Children[counter + 24] as Image).Source.ToString();

            //ЕСЛИ УЖЕ КАРТИНКА NULL
            if (temp.Contains(NullImage))
                return;

            temp = temp.Remove(0, temp.LastIndexOf('/') + 1);
            temp = temp.Remove(temp.IndexOf('.'));

            (GameGrid.Children[counter + 24] as Image).Source = NullImg;

            // создание изображения для замены
            BitmapImage imgSrc = new BitmapImage();
            string path = "/Resources/Game3/";
            imgSrc.BeginInit();
            imgSrc.UriSource = new Uri(path + temp + ".png", UriKind.Relative);
            imgSrc.EndInit();
            Image swap = new Image()
            {
                Source = imgSrc,
                Margin = new Thickness(10),
                Width = 64,
                Height = 64,
                Cursor = Cursors.Hand
            };
            swap.MouseMove += TargetImageMove;
            TargetImagesWrapPanel.Children.Add(swap);
        }
    }
}
