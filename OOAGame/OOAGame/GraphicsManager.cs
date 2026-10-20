using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    public static class GraphicsManager
    {
        public enum UIElement
        {
            Beanstalk,
            Upgrades
        }
        public const int UPGRADE_BOX_WIDTH = 40;
        public const int BEANSTALK_BOX_WIDTH = 80;
        public static Upgrade[] upgradesToShow;
        public static int selectedUpgradeIndex = 0;
        public static UIElement selectedUI = 0;
        private static ConsoleColor selectedColor = ConsoleColor.White;
        private static ConsoleColor unselectedColor = ConsoleColor.DarkGreen;

        public static void Load()
        {
            UpdateShownUpgrades(0);
            for(int i = 0; i < upgradesToShow.Length; i++)
            {
                PrintUpgrade(i);
            }
        }

        public static void ExecuteInput(ConsoleKey key)
        {
            if (key == ConsoleKey.Spacebar)
            {
                if (selectedUI == UIElement.Beanstalk)
                {
                    Beanstalk.Click();
                }
                else if (selectedUI == UIElement.Upgrades)
                {
                    UpgradeManager.BuyUpgrade(upgradesToShow[selectedUpgradeIndex].id);
                    PrintUpgrade(selectedUpgradeIndex);
                }
            }
            else if (key == ConsoleKey.A)
            {
                selectedUI -= 1;
                if (selectedUI < 0) selectedUI = (UIElement)(Enum.GetValues(typeof(UIElement)).Length - 1);

                for (int i = 0; i < upgradesToShow.Length; i++)
                {
                    PrintUpgrade(i);
                }
            }
            else if (key == ConsoleKey.D)
            {
                selectedUI += 1;
                if (selectedUI > (UIElement)(Enum.GetValues(typeof(UIElement)).Length - 1)) selectedUI = 0;

                for (int i = 0; i < upgradesToShow.Length; i++)
                {
                    PrintUpgrade(i);
                }
            }
            else if (key == ConsoleKey.W)
            {
                if (selectedUI == UIElement.Upgrades)
                {
                    selectedUpgradeIndex--;

                    if(selectedUpgradeIndex == -1)
                    {
                        selectedUpgradeIndex = 0;
                        if (upgradesToShow[selectedUpgradeIndex].id > 0)
                        {
                            for (int i = upgradesToShow[selectedUpgradeIndex].id - 1; i >= 0; i--)
                            {
                                if (UpgradeManager.upgrades[i].isVisible)
                                {
                                    UpdateShownUpgrades(i);
                                    break;
                                }
                            }
                        }
                    }

                    for(int i = 0; i < upgradesToShow.Length; i++)
                    {
                        PrintUpgrade(i);
                    }
                }
            }
            else if (key == ConsoleKey.S)
            {
                if (selectedUI == UIElement.Upgrades)
                {
                    selectedUpgradeIndex++;

                    if(selectedUpgradeIndex == upgradesToShow.Length)
                    {
                        selectedUpgradeIndex = upgradesToShow.Length - 1;
                        if (upgradesToShow[selectedUpgradeIndex].id < UpgradeManager.upgrades.Length - 1)
                        {
                            for (int i = upgradesToShow[selectedUpgradeIndex].id + 1; i < UpgradeManager.upgrades.Length; i++)
                            {
                                if (UpgradeManager.upgrades[i].isVisible)
                                {
                                    UpdateShownUpgrades(i, true);
                                    break;
                                }
                            }
                        }
                    }
                    

                    for (int i = 0; i < upgradesToShow.Length; i++)
                    {
                        PrintUpgrade(i);
                    }
                }
            }
        }

        private static void UpdateShownUpgrades(int firstUpgradeIndex, bool startFromBottom = false)
        {
            List<Upgrade> newUpgrades = new();
            int totalHeight = 1;
            if (startFromBottom)
            {
                for (int i = firstUpgradeIndex; i >= 0; i--)
                {
                    if (!UpgradeManager.upgrades[i].isVisible)
                    {
                        continue;
                    }

                    if (totalHeight + UpgradeManager.upgrades[i].BoxHeight > Console.WindowHeight)
                    {
                        break;
                    }
                    else
                    {
                        totalHeight += UpgradeManager.upgrades[i].BoxHeight;
                        newUpgrades.Insert(0, UpgradeManager.upgrades[i]);
                    }
                }
            }
            else
            {
                for (int i = firstUpgradeIndex; i < UpgradeManager.upgrades.Length; i++)
                {
                    if (!UpgradeManager.upgrades[i].isVisible)
                    {
                        continue;
                    }

                    if (totalHeight + UpgradeManager.upgrades[i].BoxHeight > Console.WindowHeight)
                    {
                        break;
                    }
                    else
                    {
                        totalHeight += UpgradeManager.upgrades[i].BoxHeight;
                        newUpgrades.Add(UpgradeManager.upgrades[i]);
                    }
                }
            }

            
            upgradesToShow = newUpgrades.ToArray();
        }

        public static void PrintStatCounters()
        {
            string heightCounter = ((int)Beanstalk.height).ToString() + " cm";
            Console.SetCursorPosition(BEANSTALK_BOX_WIDTH / 2 - heightCounter.Length / 2, 1);
            Console.Write(heightCounter);

            Console.SetCursorPosition(Console.WindowWidth - UPGRADE_BOX_WIDTH + 1, 0);
            string beansCounter = "Beans: $" + (int)Beanstalk.beans + " +" + (int)Beanstalk.GetAverageBeansPerSecond() + "/s";
            Console.Write(beansCounter + new string(' ', UPGRADE_BOX_WIDTH - beansCounter.Length - 1));
        }

        public static void PrintBeanstalk(int index)
        {
            PrintBorder((0, 0, BEANSTALK_BOX_WIDTH, Console.WindowHeight), selectedUI == UIElement.Beanstalk, false);

            List<string> art = new();
            using (StreamReader sr = new StreamReader($"Art\\Art{index}.txt"))
            {
                string? line;
                // Read lines from the file until the end of the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    art.Add(line);
                }
            }

            for (int i = 0; i < art.Count; i++)
            {
                Console.SetCursorPosition(1, 1 + i);
                Console.Write(art[i]);
            }
        }

        public static void PrintUpgrade(int index)
        {
            // I want to print the provided upgrade in the right spot, so I sum up the height of each box that's printed before this one.
            // I add 1 to the total because the bean counter takes up one line at the top of the upgrade list.
            int collectiveYPos = 1;
            for(int i = 0; i < index; i++)
            {
                if(upgradesToShow[i].isVisible) collectiveYPos += upgradesToShow[i].BoxHeight;
            }

            // I have defined the upgrade box width as a constant value for the sake of having one definitive constraint to work from.
            // Since the upgrades are supposed to be on the right side of the screen, I get the top left corner x position by subtracting the width of the box by the rightmost position on the screen.
            // Then the y position is just the number I summed up earlier. This will put the top left position of the box directly under the bottom left corner of the box above it (if there is any).
            // The width of the box is defined as a constant (as of writing, 40).
            // I am calculating the height of the box based on the number of lines the description needs to be printed properly.
            (int posX, int posY, int sizeX, int sizeY) box = 
                (Console.WindowWidth - UPGRADE_BOX_WIDTH,
                collectiveYPos,
                UPGRADE_BOX_WIDTH,
                upgradesToShow[index].BoxHeight);

            // Printing the box itself. If upgrades are the currently selected UI element, and this is the currently selected upgrade, parse that info to the PrintBorder method.
            PrintBorder(box, selectedUI == UIElement.Upgrades && selectedUpgradeIndex == index);

            // Printing the name of the upgrade. The box border takes up one space inside the box area, meaning the actual writable area is 2 smaller on each axis than the box size values.
            // When printing the name, I am accounting for this by adding 1 to the position on the x and y axis of the positon of the string origin.
            Console.SetCursorPosition(box.posX + 1, box.posY + 1);
            Console.Write(upgradesToShow[index].name);
            // Here I'm printing out the number of times this upgrade has been bought, as well as the maximum number of times it can be bought (if it's not unlimited).
            Console.Write(" [" + UpgradeManager.upgrades[upgradesToShow[index].id].currentCount + (UpgradeManager.upgrades[upgradesToShow[index].id].maxCount == int.MaxValue ? "" : ("/" + UpgradeManager.upgrades[upgradesToShow[index].id].maxCount)) + "]");

            // Printing the cost of the upgrade. I am doing this one the same line as the name. This does have the downside of meaning that if the cost gets insanely high, it will eventually override the name, and potentially break out of the box to the left.
            // I could account for this by simply shortening the cost to E+ some number if it becomes a problem.
            // The origin of the cost string needs to be the rightmost point on the box (so boxPosX + boxSizeX), minus the length of the cost as displayed, minus 2 to account for the box border and a dollar sign.
            Console.SetCursorPosition(box.posX + box.sizeX - ((int)upgradesToShow[index].Cost).ToString().Length - 2, box.posY + 1);
            Console.Write("$" + (int)UpgradeManager.upgrades[upgradesToShow[index].id].Cost);

            // Now I'm printing the description, which startes in the same spot as the name, except 1 lower on the y axis.
            Console.SetCursorPosition(box.posX + 1, box.posY + 2);

            // I'm splitting the description up into individual words and printing them one by one so I can measure if it needs to add a new line every time I print a new word.
            string[] wordsInDescription = upgradesToShow[index].description.Split(' ');
            for(int i = 0; i < wordsInDescription.Length; i++)
            {
                // If printing the new word would go outside the writable area in the upgrade box, push the cursor down by 1 line.
                // I'm adding 1 to the length of the word being checked to account for a space being added when it's printed.
                // And I'm subtracting 1 from the box size I'm comparing against to once again account for the box border itself.
                if (Console.GetCursorPosition().Left + wordsInDescription[i].Length + 1 > box.posX + box.sizeX - 1)
                {
                    Console.SetCursorPosition(box.posX + 1, Console.GetCursorPosition().Top + 1);
                }
                // Here I'm printing the word out, and then checking if it's the last word to be printed. If it is, don't add a space, otherwise, add a space.
                Console.Write(wordsInDescription[i] + (i == wordsInDescription.Length - 1 ? "" : " "));
            }
        }

        public static void PrintBorder(ConsoleColor borderColor, (int x, int y) position, (int x, int y) size, bool fillBackground)
        {
            Console.ForegroundColor = borderColor;
            size = (size.x - 1, size.y - 1);
            for (int y = 0; y <= size.y; y++)
            {
                for (int x = 0; x <= size.x; x++)
                {
                    char character = ' ';
                    if (x == 0 && y == 0) character = '┌';
                    else if (x == size.x && y == 0) character = '┐';
                    else if (x == 0 && y == size.y) character = '└';
                    else if (x == size.x && y == size.y) character = '┘';
                    else if (y == 0 || y == size.y) character = '─';
                    else if (x == 0 || x == size.x) character = '│';

                    if (!fillBackground && character == ' ') continue;

                    Console.SetCursorPosition(position.x + x, position.y + y);
                    Console.Write(character);
                }
            }
            Console.ResetColor();
        }

        public static void PrintBorder((int posX, int posY, int sizeX, int sizeY) box, bool selected = false, bool fillBackground = true)
        {
            PrintBorder(selected ? selectedColor : unselectedColor, (box.posX, box.posY), (box.sizeX, box.sizeY), fillBackground);
        }
    }
} 