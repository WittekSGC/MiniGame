namespace MiniGames
{
    public class GameLevel
    {
        public string Question;
        public bool First;
        public bool Second;
        public string ImagePath1;
        public string ImagePath2;

        public GameLevel(string question, bool first, string imagePath1, string imagePath2)
        {
            Question = question;
            First = first;
            Second = !first;
            ImagePath1 = imagePath1;
            ImagePath2 = imagePath2;
        }
    }
}
