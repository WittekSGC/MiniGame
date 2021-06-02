using System.Windows;
using System.Windows.Controls;

namespace MiniGames
{
    public class Task3 : TaskPattern
    {
        //*0.8
        public new int PictureSizeX = 228, PictureSizeY = 372; //109%
        public new string TaskName = "Жираф";
        public new int ImagesCount = 7;
        public new int MarginTop = 0;

        public new Point[] ImagesPositions = new Point[]
            {
                new Point(4,2), //head
                new Point(100,50), //mane
                new Point(64,54), //neck
                new Point(63,202), //body
                new Point(75,289), //frontLegs
                new Point(165,274), //backLegs
                new Point(190,213), //tail
            }; //просчитать точки
        public new Image[] Images = new Image[]
            {
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task3/head.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 118,
                    Height = 123,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task3/mane.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 25,
                    Height = 156,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task3/neck.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 46,
                    Height = 179,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task3/body.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 150,
                    Height = 98,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task3/frontLegs.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 47,
                    Height = 85,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task3/backLegs.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 41,
                    Height = 101,
                    AllowDrop = true
                },
                new Image()
                {
                    Source = LoadImageByPath("/Resources/Game7/Task3/tail.png"),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Width = 40,
                    Height = 79,
                    AllowDrop = true
                },
            }; //вписать статичные пути
        public new string[] EmptyImagesPathes = new string[] {
            "/Resources/Game7/Task3/empty_head.png",
            "/Resources/Game7/Task3/empty_mane.png",
            "/Resources/Game7/Task3/empty_neck.png",
            "/Resources/Game7/Task3/empty_body.png",
            "/Resources/Game7/Task3/empty_frontLegs.png",
            "/Resources/Game7/Task3/empty_backLegs.png",
            "/Resources/Game7/Task3/empty_tail.png",
        };

        public Task3()
        {
            base.PictureSizeX = PictureSizeX;
            base.PictureSizeY = PictureSizeY;
            base.TaskName = TaskName;
            base.ImagesCount = ImagesCount;
            base.ImagesPositions = ImagesPositions;
            base.Images = Images;
            base.EmptyImagesPathes = EmptyImagesPathes;
            base.MarginTop = 0;
        }
    }
}
