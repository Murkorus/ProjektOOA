using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    internal static class UpgradeManager
    {
        // Each time an upgrade is bought, the price will be multiplied by this number. E.g. if the number is 1.15, the price of an upgrade goes up by 15% each time it's bought.
        public const float UPGRADE_COST_INCREASE = 1.15f;

        // A comprehensive list of upgrades. All upgrades are defined here directly in the code. They cannot be added at runtime.
        private static Upgrade[] upgrades =
        {
            new(){id = 0, name = "Sprinkler", description = "+1 height per second", baseCost = 5, heightPerSecond = 1},
            new(){id = 1, name = "Watering Can", description = "+1 height per click", baseCost = 15, heightPerClick = 1},
            new(){id = 2, name = "Gardener", description = "+0.01 beans per second per height", baseCost = 50, maxCount = 1, beansPerSecondPerHeight = 0.01f},
            new(){id = 3, name = "Fertilizer", description = "+20% chance to double amount of beans gained whenever you gain beans", baseCost = 250, maxCount = 1},
            new(){id = 4, name = "Sell Trimmings", description = "+5% of height in beans, -10% height", baseCost = 5},
            new(){id = 5, name = "Irrigation", description = "+10 height per second", baseCost = 200, heightPerSecond = 10},
            new(){id = 6, name = "Garden Hose", description = "+10 height per click", baseCost = 500, heightPerClick = 10},
            new(){id = 7, name = "Gardener 2", description = "Gardener's brother, +0.01 beans per second per height", baseCost = 1000, maxCount = 1, beansPerSecondPerHeight = 0.01f},
        };

        public static Upgrade[] Upgrades { get => upgrades; private set => upgrades = value; }

        // The Where() function returns a sub-list of all items that match the given condition, in this case all Upgrades where .isVisible is true.
        public static Upgrade[] VisibleUpgrades { get => upgrades.Where(x => x.isVisible).ToArray(); private set => upgrades = value; }

        /// <summary>
        /// Shorthand for checking if at least 1 upgrade of the given index (id) has been bought.
        /// </summary>
        /// <param name="index">The index of the Upgrade in <see cref="upgrades"/>.</param>
        /// <returns>True if the Upgrade at the given index has been bought at least once.</returns>
        public static bool HasBought(int index)
        {
            return GetUpgradeCount(index) > 0;
        }

        /// <summary>
        /// Shorthand for checking if at least 1 upgrade of the given name has been bought.
        /// </summary>
        /// <param name="name">The name of the upgrade. Must have correct capitalization.</param>
        /// <returns>True if the Upgrade with the given name has been bought at least once.</returns>
        public static bool HasBought(string name)
        {
            return GetUpgradeCount(name) > 0;
        }

        /// <summary>
        /// Shorthand for checking how many times an upgrade at the given index has been bought.
        /// </summary>
        /// <param name="index">The index of the Upgrade in <see cref="upgrades"/>.</param>
        /// <returns>The number of times the Upgrade at the given index has been bought.</returns>
        public static int GetUpgradeCount(int index)
        {
            return upgrades[index].CurrentCount;
        }

        /// <summary>
        /// Shorthand for checking how many times an upgrade with the given name has been bought.
        /// </summary>
        /// <param name="index">The name of the Upgrade.</param>
        /// <returns>The number of times the Upgrade with the given name has been bought.</returns>
        public static int GetUpgradeCount(string name)
        {
            return upgrades.Where(x => x.name == name).First().CurrentCount;
        }

        /// <summary>
        /// Applies the effects of the Upgrade at the given index, on the conditions that the max amount has not already been bought, and the player has enough beans.
        /// </summary>
        /// <param name="index">The index of the Upgrade in <see cref="upgrades"/>.</param>
        public static void BuyUpgrade(int index)
        {
            if (upgrades[index].CurrentCount >= upgrades[index].maxCount) return; // If the player has already bought the max amount they can
            if (Beanstalk.beans < upgrades[index].Cost) return; // If the player doesn't have enough money

            Beanstalk.ApplyUpgrade(upgrades[index]);
        }
    }
}
