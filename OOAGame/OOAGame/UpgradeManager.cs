using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    internal class UpgradeManager
    {
        public static Upgrade[] upgrades = new Upgrade[]
        {
            new Upgrade("Sprinkler", 10f, int.MaxValue, 0, 1, 0)
        };

        public static void BuyUpgrade(int index)
        {

        }
    }
}
