using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MiniGames
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static BitmapImage LoadImageByPath(string path)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.EndInit();
            return image;
        }
    }
}
