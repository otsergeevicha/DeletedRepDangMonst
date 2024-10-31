using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "NewTurretData", menuName = "Ammo/TurretData", order = 1)]
    public class TurretData : ScriptableObject
    {
        [Header("Detection radius")]
        [Range(5f, 35f)]
        public float RadiusDetection = 5f;

        [Range(145f, 195f)] 
        public float RotateSpeed = 195f;

        [Range(.1f, 5f)] 
        public float ShotDelay = .8f;
    }
}