using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    public static class Beanstalk
    {
        // Stats that need to publicly readable
        public static float height { get; private set; } = 0;
        public static float beans { get; private set; } = 0;

        // Stats that don't need to be publicly readable
        private static float beansPerSecondPerHeight = 0.01f;
        private static float beansPerSecond = 0;
        private static float heightPerSecond = 0;
        private static float heightPerClick = 1;
        private static float beansPerClick = 0;

        // Private functions for manipulating data.
        // I have split it up into many different functions that mostly just call each other because then we have lots of hooks for upgrade logic to change what happens.

        /// <summary>
        /// Adds the specified number of beans to the player's "wallet" after applying potential changes based on game state.
        /// </summary>
        /// <param name="beansToAdd">The base number of beans to be added. May not be the same as the actual number of beans added after calculations.</param>
        private static void AddBeans(float beansToAdd)
        {
            // If player has fertilizer upgrade, there is a 20% chance to gain double beans per upgrade.
            if (UpgradeManager.HasBought("Fertilizer") && new Random().Next(0, Math.Clamp(6 - UpgradeManager.GetUpgradeCount("Fertilizer"),1, int.MaxValue)) == 0) beansToAdd *= 2;

            beans += beansToAdd;
        }

        /// <summary>
        /// Adds the specified amount of height to the beanstalk after applying potential changes based on game state.
        /// </summary>
        /// <param name="heightToAdd">The base amount of height to be added. May not be the same as the actual height added after calculations.</param>
        private static void AddHeight(float heightToAdd)
        {
            height += heightToAdd;
        }

        /// <summary>
        /// Calls <see cref="AddHeight(float)"/> with <see cref="heightPerSecond"/> as the argument.
        /// </summary>
        private static void AddHeightPerSecond()
        {
            AddHeight(heightPerSecond);
        }

        /// <summary>
        /// Calls <see cref="AddBeans(float)"/>, calculating the total amount of beans to be added per second.
        /// </summary>
        private static void AddBeansPerSecond()
        {
            AddBeans(beansPerSecond + beansPerSecondPerHeight * height);
        }

        /// <summary>
        /// Calls <see cref="AddHeight(float)"/> and <see cref="AddBeans(float)"/> passing the relevant perClick variables as arguments to each.
        /// </summary>
        private static void AddOnClick()
        {
            AddHeight(heightPerClick);
            AddBeans(beansPerClick);
        }

        // Public methods for interacting with the Beanstalk data.

        /// <summary>
        /// Public wrapper for <see cref="AddOnClick()"/>.
        /// </summary>
        public static void Click()
        {
            AddOnClick();
        }

        /// <summary>
        /// Runs all "per second" logic. Method references frames instead of seconds because the amount of time between each frame might be changed from 1 second by upgrades.
        /// </summary>
        public static void NextFrame()
        {
            AddBeansPerSecond();
            AddHeightPerSecond();
        }

        /// <summary>
        /// Applies the stats of an Upgrade to the Beanstalk stats and subtracts the cost of said upgrade from the player's "wallet".
        /// </summary>
        /// <param name="upgrade">A copy of the Upgrade data to be applied.</param>
        public static void ApplyUpgrade(Upgrade upgrade)
        {
            beans -= upgrade.Cost;

            heightPerSecond += upgrade.heightPerSecond;
            heightPerClick += upgrade.heightPerClick;
            beansPerSecond += upgrade.beansPerSecond;
            beansPerSecondPerHeight += upgrade.beansPerSecondPerHeight;

            upgrade.currentCount++;
        }

        /// <summary>
        /// Calculates the average number of beans produced per second, not including those potentially gotten from clicking.
        /// Logic from upgrades are applied mathematically instead of being fully simulated, hence the "average".
        /// </summary>
        /// <returns>The average number of beans produced per second.</returns>
        public static float GetAverageBeansPerSecond()
        {
            float perSecBeans = beansPerSecond;
            perSecBeans += beansPerSecondPerHeight * height;
            if (UpgradeManager.HasBought("Fertilizer")) perSecBeans *= 1 + 0.2f * Math.Clamp(UpgradeManager.GetUpgradeCount("Fertilizer"), 1, 5);
            return perSecBeans;
        }
    }
}
