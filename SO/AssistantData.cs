using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewAssistant", menuName = "Characters/Assistant", order = 1)]
    public class AssistantData : ScriptableObject
    {
        [Range(1f, 4f)] public float Speed = 1f;

        [Range(3, 8)] public int SizeBasket = 3;

        [HideInInspector] public int IdleHash = Animator.StringToHash("Idle");
        [HideInInspector] public int RunHash = Animator.StringToHash("Run");
    }
}