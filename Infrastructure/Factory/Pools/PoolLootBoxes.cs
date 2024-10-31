using System.Collections.Generic;
using Loots;
using Modules;
using Player;
using Services.Factory;
using Services.SDK;

namespace Infrastructure.Factory.Pools
{
    public class PoolLootBoxes
    {
        private readonly List<LootPoint> _lootPoints = new();

        public IReadOnlyList<LootPoint> LootPoints =>
            _lootPoints.AsReadOnly();

        public PoolLootBoxes(IGameFactory factory, int countLootPoints, ISDKService sdkService, Hero hero)
        {
            for (int i = 0; i < countLootPoints; i++)
            {
                LootPoint lootPoint = factory.CreateLootPoint();
                lootPoint.Construct(sdkService, hero);
                lootPoint.gameObject.SetActive(false);
                _lootPoints.Add(lootPoint);
            }
        }
    }
}