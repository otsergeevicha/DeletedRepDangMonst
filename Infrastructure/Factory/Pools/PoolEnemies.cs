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
    public class PoolEnemies
    {
        private readonly Dictionary<int, string[]> _levelEnemies = new Dictionary<int, string[]>
        {
            { 1, new[] { Constants.ZeroPath, Constants.OnePath, Constants.TwoPath } },
            { 2, new[] { Constants.ZeroPath, Constants.OnePath, Constants.TwoPath, Constants.ThreePath } },
            { 3, new[] { Constants.ZeroPath, Constants.OnePath, Constants.TwoPath, Constants.ThreePath, Constants.FourPath } },
            { 4, new[] { Constants.OnePath, Constants.TwoPath, Constants.ThreePath, Constants.FourPath, Constants.FivePath } },
            { 5, new[] { Constants.TwoPath, Constants.ThreePath, Constants.FourPath, Constants.FivePath, Constants.SixPath } },
            { 6, new[] { Constants.ThreePath, Constants.FourPath, Constants.FivePath, Constants.SixPath, Constants.SevenPath } },
            { 7, new[] { Constants.FourPath, Constants.FivePath, Constants.SixPath, Constants.SevenPath, Constants.EightPath } },
            { 8, new[] { Constants.FivePath, Constants.SixPath, Constants.SevenPath, Constants.EightPath, Constants.NinePath } },
            { 9, new[] { Constants.SixPath, Constants.SevenPath, Constants.EightPath, Constants.NinePath } },
            { 10, new[] { Constants.SevenPath, Constants.EightPath, Constants.NinePath } }
        };
        
        private readonly IGameFactory _factory;
        private readonly EnemyHealthModule _enemyHealthModule;
        private readonly LootSpawner _lootSpawner;
        private readonly PoolData _poolData;
        private readonly EnemyData _enemyData;
        private readonly FinishPlate _finishPlate;
        private int[] _levelCounts;
        private Hero _hero;
        private Transform _baseGate;

        public PoolEnemies(IGameFactory factory, PoolData poolData, EnemyData enemyData,EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner,
            FinishPlate finishPlate, Hero hero, Transform baseGate)
        {
            _baseGate = baseGate;
            _hero = hero;
            _factory = factory;
            _finishPlate = finishPlate;
            _enemyData = enemyData;
            _poolData = poolData;
            _lootSpawner = lootSpawner;
            _enemyHealthModule = enemyHealthModule;
            
            Create();
        }

        public List<Enemy> Enemies { get; private set; }

        public void AdaptingLevel()
        {
            foreach (Enemy enemy in Enemies)
                enemy.OnDestroy();

            Enemies.Clear();
            Create();
        }

        private void Create()
        {
            Enemies = new List<Enemy>();
            
            _levelCounts = new[]
            {
                _poolData.OneLevelCountEnemy, _poolData.TwoLevelCountEnemy, _poolData.ThreeLevelCountEnemy,
                _poolData.FourLevelCountEnemy, _poolData.FiveLevelCountEnemy, _poolData.SixLevelCountEnemy,
                _poolData.SevenLevelCountEnemy, _poolData.EightLevelCountEnemy, _poolData.NineLevelCountEnemy,
                _poolData.TenLevelCountEnemy
            };

            string[] paths = _levelEnemies[_poolData.CurrentLevelGame];

            foreach (string path in paths)
            {
                Enemy[] enemies = new Enemy[_levelCounts[_poolData.CurrentLevelGame - 1]];
                CreateEnemies(_levelCounts[_poolData.CurrentLevelGame - 1], _factory, enemies, path, _enemyData, _finishPlate);
            }
        }

        private void CreateEnemies(int requiredCount, IGameFactory factory, Enemy[] enemies, string currentPath,
            EnemyData enemyData, FinishPlate finishPlate)
        {
            for (int i = 0; i < requiredCount; i++)
            {
                var hpBar = factory.CreateHealthBar();
                Enemy enemy = factory.CreateEnemy(currentPath);
                
                hpBar.Construct(enemy.transform);

                if (finishPlate != null)
                    enemy.Construct(enemyData, _enemyHealthModule, _lootSpawner, hpBar, finishPlate, _hero, _baseGate);
                
                enemy.InActive();
                enemies[i] = enemy;
            }

            Enemies.AddRange(enemies);
        }
    }
}