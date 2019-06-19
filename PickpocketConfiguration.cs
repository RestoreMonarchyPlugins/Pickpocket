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
        public double PickpocketCooldown;
        public bool NotifyVictimOnSuccess;
        public bool NotifyVictimOnFail;
        public bool UsePermissions;
        public bool NotifyPolice;
        public string PoliceGroupId;

        public void LoadDefaults()
        {
            this.PickpocketTime = 2;
            this.PickpocketCooldown = 30;
            this.NotifyVictimOnSuccess = false;
            this.NotifyVictimOnFail = true;
            this.UsePermissions = false;
            this.NotifyPolice = false;
            this.PoliceGroupId = "police";
        }
    }
}
