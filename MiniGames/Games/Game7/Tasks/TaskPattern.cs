using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MiniGames
{
    public class TaskPattern
    {
        public int PictureSizeX, PictureSizeY; //52.6%
        public int MarginTop;
        public string TaskName;
        public int ImagesCount;

        public Point[] ImagesPositions;
        public Image[] Images;
        public string[] EmptyImagesPathes;


        public static BitmapImage LoadImageByPath(string path)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(path, UriKind.Relative);
            img.EndInit();
            return img;
        }
    }
}
