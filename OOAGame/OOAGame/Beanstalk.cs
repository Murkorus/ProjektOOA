using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    public static class Beanstalk
    {
        public static float height;
        public static float beans;
        public static float beansPerSecondPerHeight = 0.01f;
        public static float beansPerSecond;
        public static float heightPerSecond;
        public static float heightPerClick = 1;
        public static Dictionary<string, int> acquiredUpgrades;
    }
}
