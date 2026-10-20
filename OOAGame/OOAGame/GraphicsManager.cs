using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    public static class GraphicsManager
    {
        private static int selectedUIElement = 0;
        public static int SelectedUIElement
        {
            get { return selectedUIElement; }
            set
            {
                if (value > TOTAL_UI_ELEMENTS - 1) selectedUIElement = TOTAL_UI_ELEMENTS - 1;
                else if (value < 0) selectedUIElement = 0;
                else selectedUIElement = value;
            }
        }
        public const int TOTAL_UI_ELEMENTS = 2;
        public static void PrintBeanstalk()
        {
            string[] art = GetBeanstalkArt(0);
            for(int i = 0; i < art.Length; i++)
            {
                Console.SetCursorPosition(5, 10 + i);
                Console.Write(art[i]);
            }
        }

        private static string[] GetBeanstalkArt(int index)
        {
            List<string> art = new();
            using (StreamReader sr = new StreamReader($"Art\\Art{index}.txt"))
            {
                string line;
                // Read and display lines from the file until the end of the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    art.Add(line);
                }
            }
            return art.ToArray();
        }

        public static void PrintUIElement(int id)
        {
            (int x, int y) origin = (0, 0);
            string toPrint = "";
            Console.ForegroundColor = id == selectedUIElement ? ConsoleColor.White : ConsoleColor.Gray;
            switch (id)
            {
                case 0: // Height counter
                    origin = (0, 0);
                    toPrint = $"Height: {Beanstalk.height}";
                    break;
                case 1: // Beans counter
                    origin = (0, 1);
                    toPrint = $"Beans: {(int)Beanstalk.beans}";
                    break;
            }

            origin = (Math.Clamp(origin.x, 0, Console.LargestWindowWidth), Math.Clamp(origin.y, 0, Console.LargestWindowHeight));
            Console.SetCursorPosition(origin.x, origin.y);
            Console.Write(toPrint);
            Console.ResetColor();
        }

        public static void PrintAllUI()
        {
            for(int i = 0; i < TOTAL_UI_ELEMENTS; i++)
            {
                PrintUIElement(i);
            }
        }
    } 
}
