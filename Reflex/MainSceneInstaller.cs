using System;
using Agava.YandexGames;
using CameraModule;
using Canvases;
using Canvases.UpgradePlayer;
using ContactZones;
using Infrastructure.Factory.Pools;
using Infrastructure.SDK;
using Markers;
using Modules;
using Player;
using Plugins.MonoCache;
using Reflex.Core;
using Services.Bank;
using Services.Factory;
using Services.Inputs;
using Services.SaveLoad;
using Services.SDK;
using SO;
using Spawners;
using Triggers;
using Turrets;
using Turrets.Children;
using UnityEngine;

namespace Reflex
{
    [RequireComponent(typeof(FocusGame))]
    public class MainSceneInstaller : MonoCache, IInstaller
    {
        [SerializeField] private bool _firstLaunch;

        [Header("Objects with scene")] 
        [SerializeField] private BaseView _baseView;
        [SerializeField] private FocusGame _focusGame;
        [SerializeField] private UpgradePlayerBoard _upgradePlayerBoard;
        [SerializeField] private FinishPlate _finishPlate;
        [SerializeField] private Transform _spawnPointBoss;
        [SerializeField] private Workplace _workplace;
        [SerializeField] private StorageGem _storageGem;
        [SerializeField] private Transform[] _gemMiners = new Transform[3];
        [SerializeField] private TransitionPlate[] _transitionPlates = new TransitionPlate[2];
        [SerializeField] private CartridgeGun[] _cartridgeGuns;
        [SerializeField] private StorageAmmoPlate _storageAmmoPlate;
        [SerializeField] private Transform[] _squareEnemySpawner = new Transform[4];
        [SerializeField] private Transform[] _squareLootSpawner = new Transform[4];
        [SerializeField] private TurretPlate[] _turretPlates;
        [SerializeField] private StoreAssistantPlate _storeAssistantPlate;
        [SerializeField] private StoreTurretPlate[] _storeTurretPlates;
        [SerializeField] private SectionPlate[] _sectionPlates;
        [SerializeField] private BaseGate _baseGate;
        [SerializeField] private MonstersPortal _monstersPortal;
        
        [Header("Required configurations")]
        [SerializeField] private HeroData _heroData;
        [SerializeField] private AssistantData _assistantData;
        [SerializeField] private EnemyData _enemyData;
        [SerializeField] private PoolData _poolData;
        [SerializeField] private EnemySpawnerData _enemySpawnerData;
        [SerializeField] private BulletData _bulletData;
        [SerializeField] private TurretData _turretData;
        [SerializeField] private PriceListData _priceList;
        [SerializeField] private WorkerData _workerData;

        private WindowModule _windowModule;
        private LevelModule _levelModule;
        private EffectModule _effectModule;
        private TutorialModule _tutorialModule;
        private ISave _save;
        private ISDKService _sdk;
        private IInputService _input;
        private IGameFactory _gameFactory;
        private IWallet _wallet;

        public void InstallBindings(ContainerBuilder descriptor) => 
            descriptor.OnContainerBuilt += LoadLevel;

        private void LoadLevel(Container container)
        {
            _save = container.Single<ISave>();

            _sdk = container.Single<ISDKService>();
            _input = container.Single<IInputService>();
            _gameFactory = container.Single<IGameFactory>();
            _wallet = container.Single<IWallet>();

            if (_save.AccessProgress().DataStateGame.FirstLaunch)
                RecordData(CreateGame);
            else
                CreateGame();
        }

        private void CreateGame()
        {

#if !UNITY_EDITOR
            CollationData();
#endif
            
            _windowModule = new WindowModule();
            _effectModule = new EffectModule();
            
            Hud hud = _gameFactory.CreateHud();
            WinScreen winScreen = _gameFactory.CreateWinScreen();
            LoseScreen loseScreen = _gameFactory.CreateLoseScreen();
            StartScreen startScreen = _gameFactory.CreateStartScreen();
            UpgradeHeroScreen upgradeHeroScreen = _gameFactory.CreateUpgradeHeroScreen();
            Pool pool = _gameFactory.CreatePool();
            CameraFollow cameraFollow = _gameFactory.CreateCamera();
            Hero hero = _gameFactory.CreateHero();
            EnemySpawner enemySpawner = _gameFactory.CreateEnemySpawner();
            WorkerSpawner workerSpawner = _gameFactory.CreateWorkerSpawner();
            HeroAimRing heroAimRing = _gameFactory.CreateHeroAimRing();
            EnemyRing enemyRing = _gameFactory.CreateEnemyRing();

            _sdk.Inject(cameraFollow.GetMusic);
            
            _focusGame.Construct(cameraFollow.GetMusic);
            
            _baseView.UpdateText(_poolData.CurrentLevelGame.ToString());
            
            pool.Construct(_gameFactory, _poolData, _assistantData, _enemyData, _cartridgeGuns, _storageAmmoPlate, 
                _turretPlates, _bulletData, _turretData, _squareLootSpawner, _sdk, _workerData, _gemMiners, _storageGem, 
                _spawnPointBoss.position, _finishPlate, hero, _monstersPortal.transform, _effectModule, _baseGate, _sectionPlates);

            _effectModule.Construct(pool.PoolEffects);
            
            heroAimRing.Construct(hero.transform, _heroData.RadiusDetection);
            
            hero.Construct(_input, _wallet, _heroData, pool.PoolAmmoBox, _poolData.MaxCountBullets, 
                enemyRing, pool.PoolEnemies, pool.PoolBosses, hud, _windowModule, cameraFollow, heroAimRing, 
                _bulletData, _effectModule, pool.PoolBullets.Bullets, _poolData);
            
            cameraFollow.Construct(hero.GetCameraRoot());
            enemySpawner.Construct(_squareEnemySpawner, pool.PoolEnemies, _enemySpawnerData, pool.PoolBosses, _poolData, _firstLaunch);
            workerSpawner.Construct(pool.PoolWorkers, _workplace.gameObject.transform.position, _poolData);
            _workplace.Construct(_poolData.MaxCountWorkers);

            foreach (SectionPlate sectionPlate in _sectionPlates) 
                sectionPlate.Construct(_wallet, _priceList, _poolData, _sdk);

            foreach (TransitionPlate plate in _transitionPlates)
                plate.Construct(_wallet, _priceList);

            hud.Construct(cameraFollow.GetCameraMain, _monstersPortal, _input, upgradeHeroScreen, cameraFollow, _upgradePlayerBoard.GetRootCamera());

            _windowModule.Construct(_storeAssistantPlate, _storeTurretPlates, _poolData, 
                pool, _wallet, hud, loseScreen, startScreen, winScreen, _input, _upgradePlayerBoard, upgradeHeroScreen, _heroData, _priceList, hero, _sdk, _baseGate, enemySpawner);
            
            _baseGate.Construct(heroAimRing, cameraFollow, hero, _poolData);
            
            _levelModule = new LevelModule(_poolData, _finishPlate, _windowModule,  pool, hero, workerSpawner, _sectionPlates, _transitionPlates, _baseGate, enemySpawner, _baseView, _storeTurretPlates, _cartridgeGuns);

            _finishPlate.InActive();
            
#if !UNITY_EDITOR
            YandexGamesSdk.GameReady();
#endif
            
            if (_firstLaunch)
            {
                _firstLaunch = false;
                
                _save.AccessProgress().DataStateGame.FirstLaunch = _firstLaunch;
                _save.Save();
                
                _tutorialModule = new TutorialModule(_gameFactory, _storeTurretPlates, _storageAmmoPlate, 
                    _storeAssistantPlate, workerSpawner, 
                    _storageGem, _transitionPlates, _upgradePlayerBoard, cameraFollow,
                     _windowModule, enemySpawner, _turretPlates[0], hud);
            }
        }

        private void CollationData()
        {
            _firstLaunch = _save.AccessProgress().DataStateGame.FirstLaunch;

            _poolData.CurrentLevelGame = _save.AccessProgress().DataStateGame.CurrentLevel;

            _heroData.MaxHealth = _save.AccessProgress().DataStateGame.HeroHealth;
            _heroData.Speed = _save.AccessProgress().DataStateGame.HeroSpeed;
            _heroData.SizeBasket = _save.AccessProgress().DataStateGame.HeroSizeBasket;
            _heroData.RadiusDetection = _save.AccessProgress().DataStateGame.HeroRadiusDetection;
        }

        private void RecordData(Action completed = null)
        {
            _save.AccessProgress().DataStateGame.FirstLaunch = _firstLaunch;

            _save.AccessProgress().DataStateGame.CurrentLevel = _poolData.CurrentLevelGame;

            _save.AccessProgress().DataStateGame.HeroHealth = _heroData.MaxHealth;
            _save.AccessProgress().DataStateGame.HeroSpeed = _heroData.Speed;
            _save.AccessProgress().DataStateGame.HeroSizeBasket = _heroData.SizeBasket;
            _save.AccessProgress().DataStateGame.HeroRadiusDetection = _heroData.RadiusDetection;
            
            _save.Save();
            completed?.Invoke();
        }

        protected override void OnDisabled()
        {
#if !UNITY_EDITOR
            RecordData();
#endif
            _windowModule.Dispose();
            _levelModule.Dispose();
            _tutorialModule?.Dispose();
        }
    }
}