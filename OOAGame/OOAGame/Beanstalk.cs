using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    public static class Beanstalk
    {
        public static float height = 1000;
        public static float beans = 0;
        public static float beansPerSecondPerHeight = 0.01f;
        public static float beansPerSecond = 0;
        public static float heightPerSecond = 0;
        public static float heightPerClick = 1;
        public static float beansPerClick = 0;

        public static void AddBeans(float beansToAdd)
        {
            // If player has fertilizer upgrade, there is a 20% chance to gain double beans.
            if (UpgradeManager.GetUpgradeCount("Fertilizer") > 0 && new Random().Next(0, 5) == 0)
            {
                beansToAdd *= 2;
            }
            beans += beansToAdd;
        }

        public static void AddHeight(float heightToAdd)
        {
            height += heightToAdd;
        }

        public static float GetAverageBeansPerSecond()
        {
            float perSecBeans = beansPerSecond;
            perSecBeans += beansPerSecondPerHeight * height;
            if (UpgradeManager.GetUpgradeCount("Fertilizer") > 0) perSecBeans *= 1.1f;
            return perSecBeans;
        }

        private static void AddHeightPerSecond()
        {
            AddHeight(heightPerSecond);
        }

        private static void AddBeansPerSecond()
        {
            AddBeans(beansPerSecond + beansPerSecondPerHeight * height);
        }

        private static void AddOnClick()
        {
            AddHeight(heightPerClick);
            AddBeans(beansPerClick);
        }

        public static void Click()
        {
            AddOnClick();
        }

        public static void NextFrame()
        {
            AddBeansPerSecond();
            AddHeightPerSecond();
        }
    }
}
