using System;
using System.Runtime.Remoting.Messaging;
using Utilities;

namespace Phoenix
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var context = new GlobalContext();

            if (args.Length == 2 && args[0].ToLower() == "--resolution")
            {
                var splitString = args[1].Split('x');
                var width = Convert.ToInt32(splitString[0]);
                var height = Convert.ToInt32(splitString[1]);
                var desiredResolution = new Point(width, height);
                context.DesiredResolution = desiredResolution;
            }

            CallContext.LogicalSetData("AmbientGlobalContext", context);

            using (var game = new MainGame())
            {
                game.Run();
            }
        }
    }
}