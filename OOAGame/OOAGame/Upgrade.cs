using System;
using System.Collections.Generic;
using System.Text;

namespace OOAGame
{
    internal class Upgrade
    {
        public string name;
        public float cost;
        public int maxCount;
        private float beansPerSecond;
        private float heightPerSecond;
        private float heightPerClick;
        public Upgrade(string name, float cost, int maxCount, float beansPerSecond, float heightPerSecond, float heightPerClick)
        {
            this.name = name;
            this.cost = cost;
            this.maxCount = maxCount;
            this.beansPerSecond = beansPerSecond;
            this.heightPerSecond = heightPerSecond;
            this.heightPerClick = heightPerClick;
        }
    }
}
