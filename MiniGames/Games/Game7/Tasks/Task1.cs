using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MiniGames
{
    public class Task1 : TaskPattern
    {
        public new int PictureSizeX = 424, PictureSizeY = 284; //52.6%
        public new string TaskName = "Слон";
        public new int ImagesCount = 4;
        public new int MarginTop = 100;

        public new Point[] ImagesPositions = new Point[]
            {
                new Point(123,32), //body
                new Point(131,125), //frontLegs
                new Point(262,118), //backLegs
                new Point(0,0), //head
            }; //просчитать точки
        public new Image[] Images = new Image[]
            {
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task1/body.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 301,
                    Height = 182,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task1/frontLegs.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 83,
                    Height = 145,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task1/backLegs.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 103,
                    Height = 163,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task1/head.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 215,
                    Height = 214,
                    AllowDrop = true
                }
            }; //вписать статичные пути
        public new string[] EmptyImagesPathes = new string[] {
            "/Resources/Game7/Task1/empty_body.png",
            "/Resources/Game7/Task1/empty_frontLegs.png",
            "/Resources/Game7/Task1/empty_backLegs.png",
            "/Resources/Game7/Task1/empty_head.png",
        };

        public Task1()
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
