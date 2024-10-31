using Assistant.AI.Parents;
using BehaviorDesigner.Runtime.Tasks;

namespace Assistant.AI
{
    public class IsBasketUnder : CargoAssistantConditional
    {
        public override TaskStatus OnUpdate() => 
            CargoAssistant.Basket.IsEmpty ? TaskStatus.Success : TaskStatus.Failure;
    }
}