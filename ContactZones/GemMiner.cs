using Plugins.MonoCache;
using UnityEngine;
using Workers;

namespace ContactZones
{
    public class GemMiner : MonoCache
    {
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Worker worker))
            {
                if (worker.IsHandEmpty) 
                    worker.IsProcessMining = true;
            }
        }
    }
}