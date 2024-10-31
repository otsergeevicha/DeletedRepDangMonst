using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Player.ShootingModule
{
    public class MagazineReload
    {
        private const float DelayReload = .5f;
        
        private readonly IMagazine _magazine;
        
        private CancellationTokenSource _tokenReplenishment = new();

        private bool _isCharge;
        private bool _isReplenishment;

        public MagazineReload(IMagazine magazine) => 
            _magazine = magazine;

        public bool IsCharge =>
            _isCharge;
        
        public async UniTaskVoid Launch(int delayRegeneration)
        {
            _isCharge = true;
            await UniTask.Delay(delayRegeneration);
            _isCharge = false;

            Replenishment(_tokenReplenishment.Token).Forget();
        }
        
        private async UniTaskVoid Replenishment(CancellationToken token)
        {
            _isReplenishment = true;

            while (_isReplenishment)
            {
                _magazine.Replenishment(() => _isReplenishment = false);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayReload), cancellationToken: token);
            }
        }

        public void StopReplenishment()
        {
            _tokenReplenishment.Cancel();
            _tokenReplenishment.Dispose();
            _tokenReplenishment = new CancellationTokenSource();
            _isReplenishment = false;
        }
    }
}