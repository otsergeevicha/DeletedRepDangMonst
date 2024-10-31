using Plugins.MonoCache;

namespace Turrets.Children
{
    public class CartridgeBox : MonoCache
    {
        public void OnActive() =>
            gameObject.SetActive(true);

        public void InActive() =>
            gameObject.SetActive(false);
    }
}