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
        MainWindow Main;
        WindowState State;

        public GameWindow1(MainWindow main, WindowState windowState)
        {
            InitializeComponent();
            this.Closing += GameWindow1_Closing;
            this.StateChanged += GameWindow1_StateChanged;
            Main = main;
            State = windowState;
            WindowState = windowState;
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
    }
}
