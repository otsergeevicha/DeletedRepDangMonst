using System.Collections.Generic;
using ContactZones;
using Enemies;
using Modules;
using Player;
using Services.Factory;
using SO;
using Spawners;
using UnityEngine;

namespace Infrastructure.Factory.Pools
{
    public class PoolBosses
    {
        private readonly Dictionary<int, string> _levelEnemies = new Dictionary<int, string>
        {
            { 1, Constants.OneBossPath },
            { 2, Constants.TwoBossPath } ,
            { 3, Constants.ThreeBossPath },
            { 4, Constants.FourBossPath},
            { 5, Constants.FiveBossPath },
            { 6, Constants.SixBossPath },
            { 7, Constants.SevenBossPath },
            { 8, Constants.EightBossPath },
            { 9, Constants.NineBossPath },
            { 10, Constants.TenBossPath }
        };
        
        private LootSpawner _lootSpawner;
        private EnemyHealthModule _enemyHealthModule;
        private Hero _hero;
        private Transform _baseGate;

        public PoolBosses(IGameFactory factory, EnemyData enemyData,
            EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner, Vector3 spawnPoint, FinishPlate finishPlate, Hero hero, Transform baseGate)
        {
            _baseGate = baseGate;
            _hero = hero;
            _lootSpawner = lootSpawner;
            _enemyHealthModule = enemyHealthModule;

            CreateBosses(factory, enemyData, spawnPoint, finishPlate);
        }

        public List<Enemy> Bosses { get; private set; } = new();
        
        private void CreateBosses(IGameFactory factory, EnemyData enemyData, Vector3 spawnPoint,
            FinishPlate finishPlate)
        {
            for (int i = 1; i < 11; i++)
            {
                var hpBar = factory.CreateHealthBar();
                Enemy boss = factory.CreateEnemy(_levelEnemies[i]);
                
                hpBar.Construct(boss.transform);
                
                boss.Construct(enemyData, _enemyHealthModule, _lootSpawner, hpBar, finishPlate, _hero, _baseGate);
                boss.transform.position = spawnPoint;
                boss.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                boss.InActive();
                Bosses.Add(boss);
            }
        }

        public void AdaptingLevel()
        {
            foreach (Enemy boss in Bosses) 
                boss.InActive();
        }
    }
}