using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleCity
{
    class TankData
    {        
        public double speed;
        public byte reloadTime;
        public double bulletSpeed;
        public bool destroysIron;

        public TankData(double speed, byte reloadTime, double bulletSpeed, bool destroysIron)
        {
            this.speed = speed;
            this.reloadTime = reloadTime;
            this.bulletSpeed = bulletSpeed;
            this.destroysIron = destroysIron;
        }
    }
}
