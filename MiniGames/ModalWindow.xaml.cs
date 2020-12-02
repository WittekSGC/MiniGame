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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiniGames
{
    public enum ModalWindowMode
    {
        TextShow,
        TextWithOkBtn,
        TextWithYesNoBtn
    }

    /// <summary>
    /// Логика взаимодействия для ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        ModalWindowMode Mode;


        public ModalWindow(string text, ModalWindowMode mode)
        {
            InitializeComponent();
            CloseButton.MouseUp += CloseButton_MouseUp;
            ModalText.Text = text;
            Mode = mode;
            ShowButtons();
        }

        private void ShowButtons()
        {
            switch (Mode)
            {
                case ModalWindowMode.TextShow:
                    return;
                case ModalWindowMode.TextWithOkBtn:
                    ShowOkButton();
                    break;
                case ModalWindowMode.TextWithYesNoBtn:
                    ShowYesNoButton();
                    break;
                default:
                    break;
            }
        }

        private void ShowYesNoButton()
        {
            Button b = AddNewButton("Yes");
            b.Click += B_True_Click;
            b.HorizontalAlignment = HorizontalAlignment.Left;
            Button n = AddNewButton("No");
            n.Click += B_False_Click;
            n.HorizontalAlignment = HorizontalAlignment.Right;
        }

        private void ShowOkButton()
        {
            Button b = AddNewButton("OK");
            b.Click += B_True_Click;
        }

        private void B_True_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void B_False_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private Button AddNewButton(string name)
        {
            Button b = new Button();
            b.Content = name;
            b.HorizontalAlignment = HorizontalAlignment.Center;
            b.Width = 200;
            b.VerticalAlignment = VerticalAlignment.Stretch;
            b.FontSize = 18;
            b.Background = new SolidColorBrush(Color.FromRgb(0, 255, 85));
            Grid.SetColumn(b, 0);
            Grid.SetRow(b, 2);
            RootGrid.Children.Add(b);
            return b;
        }

        private void CloseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }
    }
}