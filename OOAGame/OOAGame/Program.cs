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
                        switch (GraphicsManager.SelectedUIElement)
                        {
                            default:
                                Click();
                                break;
                        }
                    }
                    // Temporary
                    else if(key.Key == ConsoleKey.D1)
                    {
                        UpgradeManager.BuyUpgrade(0);
                    }
                    else if (key.Key == ConsoleKey.D2)
                    {
                        UpgradeManager.BuyUpgrade(1);
                    }
                    else if (key.Key == ConsoleKey.D3)
                    {
                        UpgradeManager.BuyUpgrade(2);
                    }
                }

                if(Stopwatch.GetElapsedTime(lastCheckedTime).TotalSeconds >= frameTime) // Run this code if it's been at least frameTime number of seconds since it was last executed
                {
                    lastCheckedTime = Stopwatch.GetTimestamp();
                    NextFrame();
                }

                GraphicsManager.PrintAllUI();
                Console.SetCursorPosition(0, 3);
                for(int i = 0; i < UpgradeManager.upgrades.Length; i++)
                {
                    Upgrade upgrade = UpgradeManager.upgrades[i];
                    Console.WriteLine($"{upgrade.name}: {(int)upgrade.Cost}\n {upgrade.description}");
                }
                GraphicsManager.PrintBeanstalk();
            }
        }

        private static void Click()
        {
            Beanstalk.AddOnClick();
        }

        private static void NextFrame()
        {
            Beanstalk.AddBeansPerSecond();
            Beanstalk.AddHeightPerSecond();
        }
    }
}
