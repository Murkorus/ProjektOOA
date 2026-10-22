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
            Console.SetWindowSize(120, 30);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            lastCheckedTime = Stopwatch.GetTimestamp();
            GraphicsManager.Load();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    GraphicsManager.ExecuteInput(Console.ReadKey(true).Key);
                    while (Console.KeyAvailable) // Flushes excess input from receiving input faster than application can process it (reduces lag)
                    {
                        Console.ReadKey(true);
                    }
                }

                if(Stopwatch.GetElapsedTime(lastCheckedTime).TotalSeconds >= frameTime) // Run this code if it's been at least frameTime number of seconds since it was last executed
                {
                    lastCheckedTime = Stopwatch.GetTimestamp();
                    Beanstalk.NextFrame();
                }

                GraphicsManager.PrintBeanstalk();
                GraphicsManager.PrintStatCounters();

                Thread.Sleep(10); // Added small delay between loops to reduce strain on the CPU.
            }
        }
    }
}
