﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniGames
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int n = 9; //кол-во игр
        private Image[] images = new Image[n];
        private TextBlock[] textBlocks = new TextBlock[n];
        private string[] GamesName = new string[] { "Game1", "Game2", "Game3", "Game4", "Game5", "Game6", "Game7", "Game8", "Game9" };
        private Window[] Games = new Window[n];

        public WindowState windowState;

        public MainWindow()
        {
            InitializeComponent();

            //next games

            this.SizeChanged += MainWindowSizeChanged;
            this.StateChanged += MainWindowStateChanged; ;
            this.Closed += MainWindow_Closed;

            CreateItems();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            
        }

        private void MainWindowStateChanged(object sender, EventArgs e)
        {
            windowState = WindowState;
            CorrectingFontOnTextBlocks();
        }

        private void MainWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CorrectingFontOnTextBlocks();
        }

        private void CorrectingFontOnTextBlocks()
        {
            for (int i = 0; i < n; i++)
                if (this.WindowState != WindowState.Maximized)
                    textBlocks[i].FontSize = 10 + Height / 48;
                else
                    textBlocks[i].FontSize = 30;
        }

        private void CreateItems()
        {
            for (int i = 0; i < 9; i++)
            {
                //images
                images[i] = new Image();
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri("Resources/image" + i.ToString() + ".jpg", UriKind.Relative);
                image.EndInit();

                images[i].Source = image;
                images[i].Margin = new Thickness(10, 10, 10, 50);
                images[i].HorizontalAlignment = HorizontalAlignment.Center;
                images[i].Cursor = Cursors.Hand;

                Grid.SetColumn(images[i], i % 3);
                Grid.SetRow(images[i], i / 3);

                images[i].MouseEnter += Item_MouseEnter; //для анимации
                images[i].MouseLeave += Item_MouseLeave; ; //для анимации
                images[i].MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;

                ItemsGrid.Children.Add(images[i]);

                //text blocks
                textBlocks[i] = new TextBlock();
                textBlocks[i].Text = GamesName[i];
                textBlocks[i].FontSize = Height/24;
                textBlocks[i].TextAlignment = TextAlignment.Center;
                textBlocks[i].VerticalAlignment = VerticalAlignment.Bottom;
                textBlocks[i].Margin = new Thickness(0, 0, 0, 20);

                Grid.SetColumn(textBlocks[i], i / 3);
                Grid.SetRow(textBlocks[i], i % 3);

                ItemsGrid.Children.Add(textBlocks[i]);
            }
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int pick = -1;
            for (int i = 0; i < n; i++)
            {
                if ((sender as Image).Equals(images[i])) {
                    pick = i;
                    break;
                }
            }
            if (pick != -1)
            {
                switch (pick)
                {
                    case 0:
                        Games[0] = new GameWindow1(this, WindowState);
                        Games[0].Show();
                        break;
                    default:
                        MessageBox.Show("Other keys");
                        break;
                }
                Hide();
            }
        }

        private void Item_MouseLeave(object sender, MouseEventArgs e)
        {
            ThicknessAnimation anim = new ThicknessAnimation();
            anim.To = new Thickness(10, 10, 10, 50);
            anim.Duration = TimeSpan.FromSeconds(0.4);
            ((UIElement)sender).BeginAnimation(MarginProperty, anim);
        }

        private void Item_MouseEnter(object sender, MouseEventArgs e)
        {
            ThicknessAnimation anim = new ThicknessAnimation();
            anim.From = new Thickness(10,10,10,50);
            anim.To = new Thickness(0,0,0,10);
            anim.Duration = TimeSpan.FromSeconds(0.4);
            ((UIElement)sender).BeginAnimation(MarginProperty, anim);
        }
    }
}
