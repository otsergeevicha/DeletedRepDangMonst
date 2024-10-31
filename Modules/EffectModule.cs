using System.Linq;
using Effects;
using Infrastructure.Factory.Pools;
using UnityEngine;

namespace Modules
{
    public class EffectModule
    {
        private PoolEffects _poolEffects;

        public void Construct(PoolEffects pool) => 
            _poolEffects = pool;

        public void OnHitEnemy(Vector3 spawnPoint)
        {
          _poolEffects.Effects.FirstOrDefault(effect =>
                effect.isActiveAndEnabled == false && effect.TryGetComponent(out VfxHitRed _))
              ?.OnActive(spawnPoint);
        }

        public void OnExplosion(Vector3 spawnPoint)
        {
            _poolEffects.Effects.FirstOrDefault(effect =>
                    effect.isActiveAndEnabled == false && effect.TryGetComponent(out VfxMissileExplosion _))
                ?.OnActive(spawnPoint);
        }
    }
}