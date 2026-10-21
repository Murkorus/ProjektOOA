using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace OOAGame
{
    public struct Upgrade
    {
        // These are default values
        required public int id = 0;
        required public string name = "Sprinkler";
        required public string description = "Default";
        required public float baseCost = 10;
        public int maxCount = int.MaxValue;
        public int currentCount = 0;
        public float beansPerSecond = 0;
        public float heightPerSecond = 0;
        public float heightPerClick = 0;
        public float beansPerSecondPerHeight = 0;
        public bool isVisible = true;
        public float Cost
        {
            get => field = baseCost * MathF.Pow(UpgradeManager.UPGRADE_COST_INCREASE, currentCount);
            private set;
        }
        public int BoxHeight
        {
            get => field = 3 + (int)MathF.Ceiling(description.Length / (float)(GraphicsManager.UPGRADE_BOX_WIDTH - 2));
            private set;
        }
        public Upgrade()
        {

        }

        /*
        /// <summary>
        /// Compares the name of two Upgrades to check if they are equal, since all other variables do not matter.
        /// </summary>
        /// <param name="obj">The other Upgrade to compare equality with</param>
        /// <returns></returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (!(obj is Upgrade)) return false;
            if (((Upgrade)obj).name == name) return true;
            return false;
        }
        */
    }
}
