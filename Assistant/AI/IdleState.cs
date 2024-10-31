using Assistant.AI.Parents;

namespace Assistant.AI
{
    public class IdleState : CargoAssistantAction
    {
        public override void OnStart() => 
            CargoAssistant.AssistantAnimation.EnableIdle();
    }
}