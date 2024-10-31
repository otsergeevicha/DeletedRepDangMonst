using Assistant.AI.Parents;

namespace Assistant.AI
{
    public class MoveStorage : CargoAssistantAction
    {
        public override void OnStart()
        {
            CargoAssistant.AssistantAnimation.EnableRun();
            Agent.speed = CargoAssistant.AssistantData.Speed;
            Agent.destination = CargoAssistant.StorageAmmoPlate.transform.position;
        }
    }
}