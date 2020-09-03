﻿using System;
using PhoenixGameLibrary;
using PhoenixGamePresentation;
using Utilities;

namespace Phoenix2
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var gameMetadata = new GameMetadata();
            var presentationContext = new GlobalContextPresentation();

            //--resolution 800x600
            if (args.Length == 2 && args[0].ToLower() == "--resolution")
            {
                var splitString = args[1].Split('x');
                var width = Convert.ToInt32(splitString[0]);
                var height = Convert.ToInt32(splitString[1]);
                var desiredResolution = new PointI(width, height);
                presentationContext.DesiredResolution = desiredResolution;
            }

            CallContext<GameMetadata>.SetData("GameMetadata", gameMetadata);
            CallContext<GlobalContextPresentation>.SetData("GlobalContextPresentation", presentationContext);

            //using (var game = new Game1())
            using (var game = new MainGame())
            {
                game.Run();
            }
        }
    }
}