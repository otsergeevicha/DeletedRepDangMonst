using Assistant.AI.Parents;
using BehaviorDesigner.Runtime.Tasks;
using Turrets.Children;

namespace Assistant.AI
{
    public class MoveCartridgeGun : CargoAssistantAction
    {
        private CartridgeGun _gun;

        public override void OnStart()
        {
            GetCurrentCartridge();
            EnableState();
        }

        public override TaskStatus OnUpdate() =>
            _gun != null && _gun.IsRequiredDownload ? TaskStatus.Success : TaskStatus.Failure;

        private void EnableState()
        {
            if (_gun != null)
            {
                CargoAssistant.AssistantAnimation.EnableRun();
                Agent.speed = CargoAssistant.AssistantData.Speed;
                Agent.destination = _gun.transform.position;
            }
        }

        private void GetCurrentCartridge()
        {
            _gun = null;

            int minAmountBullet = int.MaxValue;

            foreach (CartridgeGun cartridge in CargoAssistant.CartridgeGuns)
            {
                if (cartridge.isActiveAndEnabled)
                {
                    int currentAmmo = cartridge.ReadCurrentAmmo();
                    
                    if (currentAmmo < minAmountBullet)
                    {
                        minAmountBullet = currentAmmo;
                        _gun = cartridge;
                    }
                }
            }
        }
    }
}