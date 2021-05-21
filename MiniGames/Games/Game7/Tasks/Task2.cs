using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiniGames
{
    public class Task2 : TaskPattern 
    {

        public new int PictureSizeX = 405, PictureSizeY = 289; //101%
        public new string TaskName = "Бегемот";
        public new int ImagesCount = 8;
        public new int MarginTop = 100;

        public new Point[] ImagesPositions = new Point[]
            {
                new Point(38,0), //leftEar
                new Point(156,4), //rightEar
                new Point(38,28), //head
                new Point(0,90), //nose
                new Point(39,217), //mouth
                new Point(103,227), //frontLegs
                new Point(180,54), //body
                new Point(323,77), //tail
            }; //просчитать точки
        public new Image[] Images = new Image[]
            {
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task2/leftEar.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 40,
                    Height = 57,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task2/rightEar.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 37,
                    Height = 57,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task2/head.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 154,
                    Height = 86,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task2/nose.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 214,
                    Height = 138,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task2/mouth.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 148,
                    Height = 31,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task2/frontLegs.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 104,
                    Height = 61,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task2/body.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 188,
                    Height = 235,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task2/tail.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 83,
                    Height = 117,
                    AllowDrop = true
                },
            }; //вписать статичные пути
        public new string[] EmptyImagesPathes = new string[] {
            "/Resources/Game7/Task2/empty_leftEar.png",
            "/Resources/Game7/Task2/empty_rightEar.png",
            "/Resources/Game7/Task2/empty_head.png",
            "/Resources/Game7/Task2/empty_nose.png",
            "/Resources/Game7/Task2/empty_mouth.png",
            "/Resources/Game7/Task2/empty_frontLegs.png",
            "/Resources/Game7/Task2/empty_body.png",
            "/Resources/Game7/Task2/empty_tail.png",
        };

        public Task2()
        {
            base.PictureSizeX = PictureSizeX;
            base.PictureSizeY = PictureSizeY;
            base.TaskName = TaskName;
            base.ImagesCount = ImagesCount;
            base.ImagesPositions = ImagesPositions;
            base.Images = Images;
            base.EmptyImagesPathes = EmptyImagesPathes;
            base.MarginTop = MarginTop;
        }
    }
}
