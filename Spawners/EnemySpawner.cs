using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enemies;
using Infrastructure.Factory.Pools;
using Plugins.MonoCache;
using SO;
using UnityEngine;

namespace Spawners
{
    public class EnemySpawner : MonoCache
    {
        public Enemy ActiveBoss { get; private set; }

        private CancellationTokenSource _token = new ();


        private Transform[] _squarePoints;
        private PoolEnemies _poolSimpleEnemies;
        private EnemySpawnerData _enemySpawnerData;

        private bool _isWork = true;
        private PoolData _poolData;
        private PoolBosses _poolBosses;

        public void Construct(Transform[] squareEnemySpawner, PoolEnemies poolSimpleEnemies,
            EnemySpawnerData enemySpawnerData, PoolBosses poolBosses, PoolData poolData, bool isFirstLaunch)
        {
            _poolBosses = poolBosses;
            _poolData = poolData;
            _enemySpawnerData = enemySpawnerData;
            _poolSimpleEnemies = poolSimpleEnemies;
            _squarePoints = squareEnemySpawner;

            foreach (Enemy enemy in _poolSimpleEnemies.Enemies)
                enemy.Died += ReuseEnemy;

            if (!isFirstLaunch)
                OnStart();

            ActiveCurrentBoss();
        }

        public void OnStart() =>
            LaunchSpawn(_token.Token).Forget();

        protected override void OnDisabled()
        {
            foreach (Enemy enemy in _poolSimpleEnemies.Enemies)
                enemy.Died -= ReuseEnemy;
        }

        public void ActiveCurrentBoss()
        {
            if (ActiveBoss != null)
                ActiveBoss = null;

            ActiveBoss = _poolBosses.Bosses[_poolData.CurrentLevelGame - 1];
            ActiveBoss.OnActive();
        }

        public void ClearField()
        {
            if (_token != null)
            {
                _token.Cancel();
                _token.Dispose();

                _token = new CancellationTokenSource();
                
                _isWork = true;
            }

            foreach (Enemy enemy in _poolSimpleEnemies.Enemies)
                enemy.InActive();
        }

        private void ReuseEnemy()
        {
            Enemy currentEnemy = _poolSimpleEnemies.Enemies.FirstOrDefault(enemy =>
                enemy.isActiveAndEnabled == false);

            if (currentEnemy != null)
            {
                currentEnemy.transform.position = GetRandomPoint();
                currentEnemy.OnActive();
            }
            else
            {
                _isWork = false;
            }
        }

        private Vector3 GetRandomPoint()
        {
            float randomX = Random.Range(Mathf.Min(_squarePoints[0].position.x, _squarePoints[1].position.x),
                Mathf.Max(_squarePoints[2].position.x, _squarePoints[3].position.x));

            float randomZ = Random.Range(Mathf.Min(_squarePoints[0].position.z, _squarePoints[2].position.z),
                Mathf.Max(_squarePoints[1].position.z, _squarePoints[3].position.z));

            return new Vector3(randomX, 0f, randomZ);
        }

        private async UniTaskVoid LaunchSpawn(CancellationToken token)
        {
            while (_isWork)
            {
                ReuseEnemy();
                await UniTask.Delay(_enemySpawnerData.IntervalSpawn, cancellationToken: token);
            }
        }
    }
}