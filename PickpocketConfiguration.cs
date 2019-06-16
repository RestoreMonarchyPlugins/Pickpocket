using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.Pickpocket
{
    public class PickpocketConfiguration : IRocketPluginConfiguration
    {
        public double PickpocketTime;
        public bool NotifyVictim;

        public void LoadDefaults()
        {
            this.PickpocketTime = 2;
            this.NotifyVictim = true;
        }
    }
}
