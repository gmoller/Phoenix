using System;
using System.IO;

namespace Utilities
{
    public sealed class Logger
    {
        private const string LOG_FILE = "Phoenix.log";

        private static readonly Lazy<Logger> Lazy = new Lazy<Logger>(() => new Logger());

        //private StreamWriter _fileStream;

        public static Logger Instance => Lazy.Value;

        private Logger()
        {
            if (File.Exists(LOG_FILE))
            {
                File.Delete(LOG_FILE);
            }

            //_fileStream = File.CreateText("Phoenix.log");
        }

        public void Log(string message)
        {
            //using (var tw = new StreamWriter(LOG_FILE, true))
            //{
            //    tw.Write($"{DateTime.Now.ToString("s")} - {message}");
            //}

            //_fileStream.Write($"{DateTime.Now.ToString("s")} - {message}");
            //_fileStream.Flush();
        }

        public void LogComplete()
        {
            //using (var tw = new StreamWriter(LOG_FILE, true))
            //{
            //    tw.WriteLine("  done.");
            //}

            //_fileStream.WriteLine("  done.");
            //_fileStream.Flush();
        }

        public void LogError(Exception ex)
        {
            using (var tw = new StreamWriter(LOG_FILE, true))
            {
                tw.WriteLine($"{DateTime.Now.ToString("s")} - {ex.Message}");
            }

            //_fileStream.Write($"{DateTime.Now.ToString("s")} - {ex.Message}");
            //_fileStream.Flush();
        }
    }
}