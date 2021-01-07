using System;
using Zen.Utilities;

namespace Phoenix2
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //--resolution 800x600
            var desiredResolution = PointI.Empty;
            if (args.Length == 2 && args[0].ToLower() == "--resolution")
            {
                var splitString = args[1].Split('x');
                var width = Convert.ToInt32(splitString[0]);
                var height = Convert.ToInt32(splitString[1]);
                desiredResolution = new PointI(width, height);
            }

            using var game = new MainGame(desiredResolution);
            game.Run();
        }
    }
}