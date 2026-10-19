using System.Diagnostics;

namespace OOAGame
{
    internal class Program
    {
        private static float frameTime = 1; // Number of seconds between each "frame" of running "per second" code
        private static long lastCheckedTime;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            lastCheckedTime = Stopwatch.GetTimestamp();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if(key.Key == ConsoleKey.Spacebar)
                    {
                        Click();
                    }
                }

                if(Stopwatch.GetElapsedTime(lastCheckedTime).TotalSeconds >= frameTime) // Run this code if it's been at least frameTime number of seconds since it was last executed
                {
                    lastCheckedTime = Stopwatch.GetTimestamp();
                    NextFrame();
                }

                GraphicsManager.PrintAllUI();
            }
        }

        private static void Click()
        {
            Beanstalk.height += Beanstalk.heightPerClick;
        }

        private static void NextFrame()
        {
            Beanstalk.beans += Beanstalk.beansPerSecond + Beanstalk.beansPerSecondPerHeight * Beanstalk.height;
            Beanstalk.height += Beanstalk.heightPerSecond;
        }
    }
}
