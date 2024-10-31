using Cysharp.Threading.Tasks;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using Services.Basket;

namespace Player
{
    public class BasketPlayer : MonoCache, IBasket
    {
        private const int MillisecondsDelay = 500;
        
        private bool _isReplenishment;
        
        private int _maxSizeBasket;
        private int _currentCount = 0;
        private PoolAmmoBoxPlayer Pool { get; set; }
        
        public bool IsEmpty => 
            _currentCount == 0;

        public void Construct(PoolAmmoBoxPlayer pool, int sizeBasket)
        {
            _maxSizeBasket = sizeBasket;

            pool.FirstPointPosition(transform);
            Pool = pool;
        }

        public void StopReplenishment() => 
            _isReplenishment = false;

        public void SpendBox()
        {
            Pool.SpendBox();
            _currentCount--;

            if (_currentCount < 0) 
                _currentCount = 0;
        }

        public void Upgrade(int newSizeBasket) => 
            _maxSizeBasket = newSizeBasket;

        public void Clear()
        {
            _currentCount = 0;
            Pool.AllInActive();
        }

        private bool CheckFull() => 
            _currentCount == _maxSizeBasket;

        public async UniTaskVoid Replenishment()
        {
            _isReplenishment = true;

            while (_isReplenishment)
            {
                if (CheckFull())
                {
                    _isReplenishment = false;
                }
                else
                {
                    Pool.AcceptBox();
                    _currentCount++;
                }

                await UniTask.Delay(MillisecondsDelay);
            }
        }
    }
}