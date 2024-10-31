using System.Collections.Generic;
using Effects;
using Services.Factory;

namespace Infrastructure.Factory.Pools
{
    public class PoolEffects
    {
        private readonly List<Effect> _effects = new ();

        public IReadOnlyList<Effect> Effects =>
            _effects.AsReadOnly();

        public PoolEffects(IGameFactory factory, int maxCountBullets, int maxCountMissile, int maxCountCoinBlast)
        {
            for (int i = 0; i < maxCountBullets / 2; i++)
            {
                VfxHitRed vfxHit = factory.CreateVfxHit();
                vfxHit.InActive();
                _effects.Add(vfxHit);
            }

            for (int i = 0; i < maxCountMissile; i++)
            {
                VfxMissileExplosion vfxMissileExplosion = factory.CreateVfxExplosion();
                vfxMissileExplosion.InActive();
                _effects.Add(vfxMissileExplosion);
            }

            for (int i = 0; i < maxCountCoinBlast; i++)
            {
                CoinBlastVfx coinBlastVfx = factory.CreateCoinBlastVfx();
                coinBlastVfx.InActive();
                _effects.Add(coinBlastVfx);
            }
        }

        public void AdaptingLevel()
        {
            foreach (Effect effect in _effects) 
                effect.InActive();
        }
    }
}