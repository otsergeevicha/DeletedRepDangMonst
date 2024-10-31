using Plugins.MonoCache;
using UnityEngine;

namespace Markers
{
    public class TutorialMarker : MonoCache
    {
        public void OnActive(Vector3 newPosition) => 
            transform.position = newPosition;

        public void InActive() =>
            gameObject.SetActive(false);
    }
}