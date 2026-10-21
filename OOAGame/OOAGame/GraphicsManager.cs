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
        public const int BEANSTALK_BOX_WIDTH = 80; // Beanstalk ascii art must at most be this width minus 2 to account for the box border. So if width is 80, ascii art can at most have 78 characters per line.
        private static Upgrade[] upgradesToShow;
        public static int selectedUpgradeIndex = 0;
        private static int SelectedUpgradeID
        {
            get => field = upgradesToShow[selectedUpgradeIndex].id;
        }
        private static Upgrade SelectedUpgrade // Note that a struct is a value-type, not a reference type. If you want to change the values, you still need a direct reference to the UpgraderManager Upgrade instance.
        {
            get => UpgradeManager.upgrades[SelectedUpgradeID];
        }
        public static UIElement selectedUI = 0;
        private static ConsoleColor selectedColor = ConsoleColor.White;
        private static ConsoleColor unselectedColor = ConsoleColor.DarkGreen;
        private static int currentBeanstalkSprite = 0;

        public static void Load()
        {
            UpdateShownUpgrades(0);
            PrintAllUpgrades();
            PrintBeanstalk();
            PrintStatCounters();
        }

        /// <summary>
        /// Takes input in the form of a pressed ConsoleKey, and then decides what functions to execute based on that input.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        public static void ExecuteInput(ConsoleKey key)
        {
            if (key == ConsoleKey.Spacebar)
            {
                if (selectedUI == UIElement.Beanstalk)
                {
                    // If the player currently has the beanstalk viewport selected, and they press space, execute a click.
                    Beanstalk.Click();
                }
                else if (selectedUI == UIElement.Upgrades && upgradesToShow.Length > 0)
                {
                    // If the player currently has an upgrade selected, and they press space, buy that upgrade and update its visuals.
                    UpgradeManager.BuyUpgrade(SelectedUpgradeID);

                    // If the upgrade has been bought the max number of times, hide it from the list.
                    if (SelectedUpgrade.currentCount >= SelectedUpgrade.maxCount)
                    {
                        // Firstly I want to check if this is the last visible upgrade left.
                        // If it is, then there will be no upgrades left to print after this one is gone, so just remove it from the list and move on.
                        Upgrade[] visibleUpgrades = UpgradeManager.GetVisibleUpgrades();

                        // Checking if there is at least 1 upgrade other than the one about to be removed that's visible.
                        if(visibleUpgrades.Length <= 1)
                        {
                            // If there isn't, and this is the last visible upgrade, then just set it to be hidden, and update the list of shown upgrades.
                            UpgradeManager.upgrades[SelectedUpgradeID].isVisible = false;
                            UpdateShownUpgrades(0);
                        }
                        else
                        {
                            // If there is at least one upgrade left after the current one has been removed, then we go down here.

                            // I want the list of upgrades to be intuitive to navigate, so there are a couple of edge-cases to keep in mind that will change what happens depending on where on the list this upgrade was at.
                            // If the upgrade to be removed was the first in the list of shown upgrades, then start the new list at the next index.
                            // If the upgrade to be removed was between the first and the last in the list of shown upgrades, then do the same, except if there is a visible upgrade further up on the list that is not currently shown, then start the list from that upgrade instead.
                            // If the upgrade to be removed was the last in the list of shown upgrades, then start the new list from the next visible upgrade above the currently highest upgrade in the list of shown upgrades.

                            // This offset will be important for an upcoming for-loop.
                            // I'm checking if the last upgrade in the list of shown upgrades is also the last upgrade that is visible.
                            // If it is, then the new list of shown upgrades should start one higher up than it currently does.
                            // UNLESS the current highest shown upgrade is also the first visible upgrade in the list of upgrades, because if it is, then there is no visible upgrade that comes before it, and instead the list of shown upgrades should just get shorter.
                            int startIndexOffset = 0;
                            if (upgradesToShow[upgradesToShow.Length - 1].id == visibleUpgrades[visibleUpgrades.Length - 1].id && upgradesToShow[0].id > visibleUpgrades[0].id)
                            {
                                startIndexOffset = -1;
                            }

                            // Update the status of the selected upgrade to be hidden.
                            // The order is important, I'm doing this down here rather than at the start of the big if-statment, because if I remove it before I check how many visible upgrades there are, then I will just get 0, and I can't check if the length of the array is less than 0, because that's impossible.
                            // It's also important that it happens after I calculate the offset, because otherwise I would not be able to pull data about how many visible upgrades are left, since the array would then be empty and there would be no data left to access.
                            UpgradeManager.upgrades[SelectedUpgradeID].isVisible = false;

                            // Since one of the upgrades on the list is now supposed to be hidden, I need to update the list of shown upgrades and remove the current one.
                            // It will be removed automatically during the evaluation in the UpdateShownUpgrades method, because it was set to be hidden earlier.
                            // The method simply skips over all hidden upgrades, so it will not even be considered.
                            // So here I start a new list of shown upgrades by starting the list from the next visible upgrade after I apply the previously calculated offset.
                            // So in most cases, it starts from the same spot, but in the case that we would run out of upgrades to display at the bottom, it will try to go up by 1 instead if possible.
                            // If that is also impossible (which happens if the list of visible upgrades has gotten too small to fill the entire screen), then it just displays less upgrades.
                            for (int i = upgradesToShow[0].id + startIndexOffset; i < UpgradeManager.upgrades.Length; i++)
                            {
                                if (UpgradeManager.upgrades[i].isVisible)
                                {
                                    UpdateShownUpgrades(i);
                                    break;
                                }
                            }

                            if (selectedUpgradeIndex > upgradesToShow.Length - 1) selectedUpgradeIndex = upgradesToShow.Length - 1;
                        }
                    }

                    PrintAllUpgrades();
                }
            }
            else if (key == ConsoleKey.A)
            {
                selectedUI -= 1;
                if (selectedUI < 0) selectedUI = (UIElement)(Enum.GetValues(typeof(UIElement)).Length - 1);

                PrintAllUpgrades();
            }
            else if (key == ConsoleKey.D)
            {
                selectedUI += 1;
                if (selectedUI > (UIElement)(Enum.GetValues(typeof(UIElement)).Length - 1)) selectedUI = 0;

                PrintAllUpgrades();
            }
            else if (key == ConsoleKey.W)
            {
                if (selectedUI == UIElement.Upgrades && upgradesToShow.Length > 1)
                {
                    selectedUpgradeIndex--;
                    if(selectedUpgradeIndex == -1)
                    {
                        selectedUpgradeIndex = 0;
                        if (SelectedUpgradeID > 0)
                        {
                            for (int i = SelectedUpgradeID - 1; i >= 0; i--)
                            {
                                if (UpgradeManager.upgrades[i].isVisible)
                                {
                                    UpdateShownUpgrades(i);
                                    break;
                                }
                            }
                        }
                    }

                    PrintAllUpgrades();
                }
            }
            else if (key == ConsoleKey.S)
            {
                if (selectedUI == UIElement.Upgrades && upgradesToShow.Length > 1)
                {
                    selectedUpgradeIndex++;
                    if(selectedUpgradeIndex == upgradesToShow.Length)
                    {
                        selectedUpgradeIndex = upgradesToShow.Length - 1;
                        if (SelectedUpgradeID < UpgradeManager.upgrades.Length - 1)
                        {
                            for (int i = SelectedUpgradeID + 1; i < UpgradeManager.upgrades.Length; i++)
                            {
                                if (UpgradeManager.upgrades[i].isVisible)
                                {
                                    UpdateShownUpgrades(i, true);
                                    break;
                                }
                            }
                        }
                    }

                    PrintAllUpgrades();
                }
            }
        }

        private static void PrintAllUpgrades()
        {
            int totalHeight = 1;
            for (int i = 0; i < upgradesToShow.Length; i++)
            {
                PrintUpgrade(i);
                totalHeight += upgradesToShow[i].BoxHeight;
            }

            for(int i = totalHeight; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth - UPGRADE_BOX_WIDTH, i);
                Console.Write(new string(' ', UPGRADE_BOX_WIDTH));
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
            // I want to display the height counter at the top of the middle of the beanstalk viewbox.
            // To display it in the middle, I need to know how long the string to display is, so I can measure the halfway-point along the string.
            // So I start by setting up the full string to be displayed.
            string heightCounter = ((int)Beanstalk.height).ToString() + " cm";
            // Since I know the beanstalk viewport box will be on the left side of the screen, I can simply take half the width of that box as the middle point.
            // Then I need the string start point to be half of the string's length further to the left, so that the middle of the string will line up with the middle of the box.
            // The y position of the cursor is 1 to account for the box border of the beanstalk viewbox.
            Console.SetCursorPosition(BEANSTALK_BOX_WIDTH / 2 - heightCounter.Length / 2, 1);
            Console.Write(heightCounter);

            // I want to display the beans counter over the upgrades section. Right now, I'm working on the principle that the beanstalk window and upgrade windows are sitting right next to each other.
            // So I could in theory just place the beans counter next to the beanstalk window and get the same effect.
            // However if I one day change this positional relationship, the beanstalk counter will then be displayed in the wrong place.
            // Therefore I need to use the upgrade box dimensions as a reference point rather than the beanstalk box.
            Console.SetCursorPosition(Console.WindowWidth - UPGRADE_BOX_WIDTH, 0);
            // Simlarly to the height counter, I start by creating the full string to be displayed.
            string beansCounter = "Beans: $" + (int)Beanstalk.beans + " +" + (int)Beanstalk.GetAverageBeansPerSecond() + "/s";
            // I am appending a bunch of spaces at the end of the beans counter such that when the player purchases an upgrade and the number of beans is reduced, there are no lingering artifacts.
            // The spaces are there to erase previously written bean counter text by writing on top of it. It creates spaces out to the last point along the upgrade box width.
            Console.Write(beansCounter + new string(' ', UPGRADE_BOX_WIDTH - beansCounter.Length - 1));
        }

        public static void PrintBeanstalk()
        {
            PrintBorder((0, 0, BEANSTALK_BOX_WIDTH, Console.WindowHeight), selectedUI == UIElement.Beanstalk, false);

            List<string> art = new();
            using (StreamReader sr = new StreamReader($"Art\\Art{currentBeanstalkSprite}.txt"))
            {
                string? line;
                // Read lines from the file until the end of the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    if(line.Length > BEANSTALK_BOX_WIDTH - 2) line = line.Substring(0, BEANSTALK_BOX_WIDTH - 2);
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