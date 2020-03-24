using Rocket.API;

namespace RestoreMonarchy.Pickpocket
{
    public class PickpocketConfiguration : IRocketPluginConfiguration
    {
        public double PickpocketTime { get; set; }
        public double PickpocketCooldown { get; set; }
        public bool NotifyVictimOnSuccess { get; set; }
        public bool NotifyVictimOnFail { get; set; }
        public bool UsePermissions { get; set; }
        public bool NotifyPolice { get; set; }
        public string PoliceGroupId { get; set; }
        public float MaxDistance { get; set; }
        public bool UseBypass { get; set; }
        public string MessageColor { get; set; }


        public void LoadDefaults()
        {
            PickpocketTime = 2;
            PickpocketCooldown = 30;
            NotifyVictimOnSuccess = false;
            NotifyVictimOnFail = true;
            UsePermissions = false;
            NotifyPolice = false;
            PoliceGroupId = "police";
            MaxDistance = 5;
            UseBypass = true;
            MessageColor = "yellow";
        }
    }
}
