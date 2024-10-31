using System.Linq;
using Effects;
using Infrastructure.Factory.Pools;
using Loots;
using SO;
using UnityEngine;

namespace Spawners
{
    public class LootSpawner
    {
        private readonly Transform[] _squareLootSpawner;
        private readonly PoolLootBoxes _poolLootBoxes;
        private readonly PoolMoney _poolMoney;
        private readonly PoolData _poolData;
        private readonly float _offSet = 5f;
        private readonly PoolEffects _poolEffects;

        public LootSpawner(PoolMoney poolMoney, PoolLootBoxes poolLootBoxes, Transform[] squareLootSpawner,
            PoolData poolData, PoolEffects poolEffects)
        {
            _poolEffects = poolEffects;
            _poolData = poolData;
            _squareLootSpawner = squareLootSpawner;
            _poolLootBoxes = poolLootBoxes;
            _poolMoney = poolMoney;

            foreach (LootPoint lootPoint in _poolLootBoxes.LootPoints)
            {
                lootPoint.OnActive(Random.Range(0, (int)TypeLoot.Health), _poolData.PercentSpawnFreeLoot);
                lootPoint.transform.position = GetRandomPoint();
                lootPoint.OnPickUp += ReSpawnLoot;
            }
        }

        public void Dispose()
        {
            foreach (LootPoint lootPoint in _poolLootBoxes.LootPoints)
                lootPoint.OnPickUp -= ReSpawnLoot;
        }
        
        public void SpawnMoney(int enemyId, Vector3 position)
        {
            _poolEffects.Effects.FirstOrDefault(effect => 
                effect.isActiveAndEnabled == false 
                && effect.TryGetComponent(out CoinBlastVfx _))
                ?.OnActive(position);

            int moneyCount = 0;
            
            float radius = 0.8f;
            int totalMoneys = 3;

            foreach (Money money in _poolMoney.Moneys.Where(money => 
                         money.isActiveAndEnabled == false))
            {
                float angle = (moneyCount * 360f / totalMoneys) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Vector3 newPosition = position + offset;

                money.OnActive(enemyId, newPosition);
                moneyCount++;

                if (moneyCount >= totalMoneys)
                    break;
            }
        }

        private void ReSpawnLoot()
        {
            LootPoint lootPoint = _poolLootBoxes.LootPoints.FirstOrDefault(box =>
                box.isActiveAndEnabled == false);

            if (lootPoint != null)
            {
                lootPoint.transform.position = GetRandomPoint();
                lootPoint.OnActive(Random.Range(0, (int)TypeLoot.Health), _poolData.PercentSpawnFreeLoot);
            }
        }

        private Vector3 GetRandomPoint()
        {
            float randomX = Random.Range(Mathf.Min(_squareLootSpawner[0].position.x, _squareLootSpawner[1].position.x),
                Mathf.Max(_squareLootSpawner[2].position.x, _squareLootSpawner[3].position.x));

            float randomZ = Random.Range(Mathf.Min(_squareLootSpawner[0].position.z, _squareLootSpawner[2].position.z),
                Mathf.Max(_squareLootSpawner[1].position.z, _squareLootSpawner[3].position.z));

            Vector3 newPoint = new Vector3(randomX, 0f, randomZ);

            return _poolLootBoxes.LootPoints.Any(lootPoint =>
                lootPoint.transform.position != newPoint)
                ? newPoint
                : GetCorrectOffset(newPoint);
        }

        private Vector3 GetCorrectOffset(Vector3 newPoint) => 
            new (newPoint.x + Random.Range(-_offSet, _offSet), 0f, newPoint.z + Random.Range(-_offSet, _offSet));
    }
}