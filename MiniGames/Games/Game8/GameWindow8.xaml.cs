using MiniGames.Games.Game8.Games;
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
    /// Логика взаимодействия для GameWindow8.xaml
    /// </summary>
    public partial class GameWindow8 : Window
    {
        private MainWindow Main;


        public GameWindow8(MainWindow main, WindowState window)
        {
            InitializeComponent();
            Main = main;
            this.WindowState = window;

            Closed += GameWindow8_Closed;
        }

        private void GameWindow8_Closed(object sender, EventArgs e)
        {
            Main.WindowState = WindowState;
            Main.Show();
        }

        private void btnGamePlay1_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new Bathroom(this).Show();
        }

        private void btnGamePlay2_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new Jobs(this).Show();
        }

        private void btnGamePlay3_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new Fruits(this).Show();
        }
    }
}
