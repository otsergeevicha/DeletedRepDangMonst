using Plugins.MonoCache;
using UnityEngine;

namespace Ammo
{
    public class AmmoBox : MonoCache
    {
        public void OnActive() =>
            gameObject.SetActive(true);

        public void InActive() =>
            gameObject.SetActive(false);

        public void SetPosition(Vector3 newPosition) =>
            transform.position = newPosition;
    }
}