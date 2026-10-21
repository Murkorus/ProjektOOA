using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    internal static class UpgradeManager
    {
        public const float UPGRADE_COST_INCREASE = 1.15f;

        public static Upgrade[] upgrades =
        {
            new(){id = 0, name = "Sprinkler", description = "+1 height per second", baseCost = 10, heightPerSecond = 1, maxCount = 1},
            new(){id = 1, name = "Gardener", description = "+0.01 beans per second per height", baseCost = 30, maxCount = 3, beansPerSecondPerHeight = 0.01f},
            new(){id = 2, name = "Fertilizer", description = "+20% chance to double amount of beans gained whenever you gain beans", baseCost = 200, maxCount = 1},
            new(){id = 3, name = "A", description = "AAAAA", baseCost = 20, maxCount = 1},
            new(){id = 4, name = "B", description = "BBBBB", baseCost = 20, maxCount = 1},
            new(){id = 5, name = "C", description = "CCCC", baseCost = 20, maxCount = 1},
            new(){id = 6, name = "D", description = "DDDD", baseCost = 20, maxCount = 1},
            new(){id = 7, name = "E", description = "EEEE", baseCost = 20, maxCount = 1},
            new(){id = 8, name = "F", description = "FFFF", baseCost = 20, maxCount = 1},
        };

        public static Upgrade[] GetVisibleUpgrades()
        {
            return upgrades.Where(x => x.isVisible).ToArray();
        }

        public static int GetUpgradeCount(int index)
        {
            return upgrades[index].currentCount;
        }

        public static int GetUpgradeCount(string name)
        {
            for(int i = 0; i < upgrades.Length; i++)
            {
                if (upgrades[i].name == name) return GetUpgradeCount(i);
            }

            return 0;
        }

        public static void BuyUpgrade(int index)
        {
            Upgrade upgrade = upgrades[index];
            if (upgrade.currentCount >= upgrade.maxCount) return; // If the player has already bought the max amount they can
            if (Beanstalk.beans < upgrade.Cost) return; // If the player doesn't have enough money

            Beanstalk.beans -= upgrade.Cost;

            upgrades[index].currentCount++;
            upgrade.Apply();
        }
    }
}
