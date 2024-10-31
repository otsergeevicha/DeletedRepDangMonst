using System.Collections.Generic;
using Ammo;
using Modules;
using Services.Factory;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class PoolMissiles
    {
        private readonly List<Missile> _missiles = new();

        public IReadOnlyList<Missile> Missiles =>
            _missiles.AsReadOnly();
        
        public PoolMissiles(IGameFactory factory, int maxCountMissiles, BulletData bulletData, EffectModule effectModule)
        {
            for (int i = 0; i < maxCountMissiles; i++)
            {
                Missile missile = factory.CreateMissile();
                missile.Construct(bulletData, effectModule);
                missile.InActive();
                _missiles.Add(missile);
            }
        }

        public void AdaptingLevel()
        {
            foreach (Missile missile in _missiles) 
                missile.InActive();
        }
    }
}