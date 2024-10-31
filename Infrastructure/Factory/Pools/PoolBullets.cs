using System.Collections.Generic;
using Ammo;
using Modules;
using Services.Factory;
using SO;

namespace Infrastructure.Factory.Pools
{
    public class PoolBullets
    {
        private readonly List<Bullet> _bullets = new();

        public IReadOnlyList<Bullet> Bullets =>
            _bullets.AsReadOnly();

        public PoolBullets(IGameFactory factory, int maxCountBullets, BulletData bulletData, EffectModule effectModule)
        {
            for (int i = 0; i < maxCountBullets; i++)
            {
                Bullet bullet = factory.CreateBullet();
                bullet.Construct(bulletData, effectModule);
                bullet.InActive();
                _bullets.Add(bullet);
            }
        }
        
        public void AdaptingLevel()
        {
            foreach (Bullet bullet in _bullets) 
                bullet.InActive();
        }
    }
}