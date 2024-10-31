using Plugins.MonoCache;

namespace Loots
{
    public class Gem : MonoCache
    {
        public void OnActive() =>
            gameObject.SetActive(true);

        public void InActive() =>
            gameObject.SetActive(false);
    }
}