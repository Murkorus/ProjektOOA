using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace OOAGame
{
    public class Upgrade
    {
        // These are default values
        
        required public int id { get; init; } = 0;
        required public string name { get; init; } = "Sprinkler";
        required public string description { get; init; } = "Default";
        required public float baseCost { get; init; } = 10;
        public int maxCount { get; init; } = int.MaxValue;
        private int currentCount = 0;
        public int CurrentCount
        {
            get
            {
                return currentCount;
            }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value");
                currentCount = value;
            }
        }
        public float beansPerSecond { get; init; } = 0;
        public float heightPerSecond { get; init; } = 0;
        public float heightPerClick { get; init; } = 0;
        public float beansPerSecondPerHeight { get; init; } = 0;
        public bool isVisible = true;
        public float Cost
        {
            // Creates a backing field for this readonly property, and returns the calculated full cost of the Upgrade.
            get => field = baseCost * MathF.Pow(UpgradeManager.UPGRADE_COST_INCREASE, currentCount);
        }
        public int BoxHeight
        {
            // Creates a backing field for this readonly property. When calculating the height of the box, I add three to account for the top and bottom of the box border, as well as the name.
            // I divide the length of the description by the length of the writable area of the box to get a number representing how many lines it takes to fit all the characters.
            // The output of the division is most likely a float, so I round up, such that if it needs e.g. 1.342 lines to write the description, it gets 2 full lines.
            // This approach might cause issues if the output of the divison is very close to the next integer (e.g. 1.94), as the process of writing the description may produce more characters than the description contains due to the word splitting process.
            get => field = 3 + (int)MathF.Ceiling(description.Length / (float)(GraphicsManager.UPGRADE_BOX_WIDTH - 2));
        }
    }
}
