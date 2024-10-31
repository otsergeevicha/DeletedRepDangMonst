using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewAssistant", menuName = "Characters/Worker", order = 1)]
    public class WorkerData : ScriptableObject
    {
        [Range(1f, 7f)] public float Speed = 1f;

        [HideInInspector] public int IdleSitingHash = Animator.StringToHash("IdleSiting");
        [HideInInspector] public int MiningHash = Animator.StringToHash("Mining");
        [HideInInspector] public int WalkingHash = Animator.StringToHash("Walking");
        [HideInInspector] public int SlowRunHash = Animator.StringToHash("SlowRun");
    }
}