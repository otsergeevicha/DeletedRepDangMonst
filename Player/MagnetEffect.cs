using Loots;
using Plugins.MonoCache;
using UnityEngine;

namespace Player
{
    public class MagnetEffect : MonoCache
    {
        [SerializeField] private SphereCollider _collider;
        
        private const string LayerNameMoney = "Money";
        
        private readonly float _duringSeconds = 10f;
        
        private Collider[] _hits = new Collider[10];
        private float _timer;
        private Hero _hero;
        
        private int _layerMask;

        public void Construct(Hero hero)
        {
            _hero = hero;
            
            _layerMask = 1 << LayerMask.NameToLayer(LayerNameMoney);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out Money money))
            {
                _hero.ApplyMoney(money.CurrentNominal);
                money.PickUp();
            }
        }

        protected override void UpdateCached()
        {
            if (!isActiveAndEnabled)
                return;

            if (_timer > 0)
                _timer -= Time.deltaTime;
            else
                InActive();
        }

        public void OnActive()
        {
            gameObject.SetActive(true);
            _timer = _duringSeconds;

            Physics.OverlapSphereNonAlloc(transform.position, _collider.radius, _hits, _layerMask);

            foreach (Collider hit in _hits)
            {
                if (hit.gameObject.TryGetComponent(out Money money))
                {
                    _hero.ApplyMoney(money.CurrentNominal);
                    money.PickUp();
                }
            }
        }

        public void InActive() => 
            gameObject.SetActive(false);
    }
}