using System.Collections.Generic;
using Ammo;
using CameraModule;
using Canvases;
using ContactZones;
using Enemies;
using GameAnalyticsSDK;
using Infrastructure.Factory.Pools;
using Markers;
using Modules;
using Player.Animation;
using Player.ShootingModule;
using Plugins.MonoCache;
using Services.Bank;
using Services.Inputs;
using SO;
using Triggers;
using Turrets.Children;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(HeroAnimation))]
    [RequireComponent(typeof(HeroMovement))]
    [RequireComponent(typeof(HeroShooting))]
    [RequireComponent(typeof(AmmoTriggers))]
    [RequireComponent(typeof(LootTriggers))]
    public class Hero : MonoCache
    {
        [SerializeField] private AudioSource _hitSound;
        [SerializeField] private HeroMovement _heroMovement;
        [SerializeField] private RootCamera _rootCamera;
        [SerializeField] private BasketPlayer _basketPlayer;
        [SerializeField] private AmmoTriggers _ammoTriggers;
        [SerializeField] private LootTriggers _lootTriggers;
        [SerializeField] private WeaponHolder _weaponHolder;
        [SerializeField] private HeroAnimation _heroAnimation;
        [SerializeField] private HeroShooting _heroShooting;

        [SerializeField] private GameObject _healingEffect;

        private IWallet _wallet;
        
        private HeroHealthModule _heroHealthModule;
        private WindowModule _windowModule;
        private HeroData _heroData;
        private HeroAimRing _aimRing;
        private Hud _hud;
        private PoolData _poolData;
        private PoolBosses _poolBosses;
        private PoolEnemies _poolEnemies;

        public void Construct(IInputService input, IWallet wallet, HeroData heroData, PoolAmmoBoxPlayer pool,
            int maxCountBullets, EnemyRing enemyRing, PoolEnemies poolEnemies,
            PoolBosses poolBosses, Hud hud, WindowModule windowModule, CameraFollow cameraFollow, HeroAimRing aimRing,
            BulletData bulletData, EffectModule effectModule, IReadOnlyList<Bullet> poolBullets, PoolData poolData)
        {
            _poolEnemies = poolEnemies;
            _poolBosses = poolBosses;
            _poolData = poolData;
            _hud = hud;
            _aimRing = aimRing;
            _heroData = heroData;
            _windowModule = windowModule;
            _wallet = wallet;
            
            _aimRing.MagnetEffect.Construct(this);
            _heroHealthModule = new HeroHealthModule(hud.GetHeroHealthView, heroData);
            _heroHealthModule.Died += OnDied;

            _heroAnimation.Construct(heroData);
            _heroMovement.Construct(input, heroData.Speed, _heroAnimation);
            _basketPlayer.Construct(pool, heroData.SizeBasket);
            _weaponHolder.Construct(cameraFollow, bulletData, effectModule, _hud, poolBullets);
            _heroShooting.Construct(_heroMovement, _weaponHolder, heroData.RadiusDetection, enemyRing);
            _heroShooting.MergeEnemies(_poolEnemies.Enemies, _poolBosses.Bosses);
            
            _healingEffect.SetActive(false);
        }

        public HeroAnimation AnimationController => 
            _heroAnimation;

        protected override void OnEnabled()
        {
            _ammoTriggers.StorageEntered += OnStorageEntered;
            _ammoTriggers.StorageExited += OnStorageExited;

            _ammoTriggers.CartridgeGunEntered += OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited += OnCartridgeGunExited;
            
            _lootTriggers.StorageGemEntered += OnStorageGemEntered;
            _lootTriggers.StorageGemExited += OnStorageGemExited;

            _lootTriggers.OnPickUpMoney += ApplyMoney;
        }

        protected override void OnDisabled()
        {
            _ammoTriggers.StorageEntered -= OnStorageEntered;
            _ammoTriggers.StorageExited -= OnStorageExited;

            _ammoTriggers.CartridgeGunEntered -= OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited -= OnCartridgeGunExited;
            
            _lootTriggers.StorageGemEntered -= OnStorageGemEntered;
            _lootTriggers.StorageGemExited -= OnStorageGemExited;

            _lootTriggers.OnPickUpMoney -= ApplyMoney;
            
            _heroHealthModule.Died -= OnDied;
        }

        private void OnValidate()
        {
            _heroMovement ??= Get<HeroMovement>();
            _heroShooting ??= Get<HeroShooting>();
            _ammoTriggers ??= Get<AmmoTriggers>();
            _lootTriggers ??= Get<LootTriggers>();
            _heroAnimation ??= Get<HeroAnimation>();
            
            _weaponHolder ??= ChildrenGet<WeaponHolder>();
        }

        public void SetShootingState(bool heroOnBase)
        {
            _heroShooting.SetOnBase(heroOnBase);
            _hud.HealthView(!heroOnBase);
        }

        public Transform GetCameraRoot() =>
            _rootCamera.transform;

        public void ApplyDamage(int damage)
        {
            _hitSound.Play();
            _heroHealthModule.ApplyDamage(damage);
        }

        public void OnHealing()
        {
            _healingEffect.SetActive(false);
            _healingEffect.SetActive(true);
            _heroHealthModule.Reset();
        }

        public void Upgrade()
        {
            _heroHealthModule.Reset();
            _heroMovement.Upgrade(_heroData.Speed);
            _basketPlayer.Upgrade(_heroData.SizeBasket);
            _heroShooting.Upgrade(_heroData.RadiusDetection);
            _aimRing.ChangeRadius(_heroData.RadiusDetection);
        }

        public void ApplyMoney(int money) =>
            _wallet.ApplyMoney(money);

        public void ApplyGem(int gem) =>
            _wallet.ApplyGem(gem);

        public void UpdateLevel()
        {
            _heroShooting.SetOnBase(true);
            _heroMovement.SetStartPosition();
            _heroMovement.SetStateBattle(false, null);
            _heroShooting.MergeEnemies(_poolEnemies.Enemies, _poolBosses.Bosses);
            _heroAnimation.EnableIdle();
            _weaponHolder.UpdateLevel();
        }

        public void OnMagnetEffect() =>
            _aimRing.MagnetEffect.OnActive();

        public void TryAgain()
        {
            _heroHealthModule.Reset();
            _heroMovement.SetStartPosition();
            _aimRing.InActive();
            _heroShooting.SetOnBase(true);
        }

        public void BasketClear() => 
            _basketPlayer.Clear();

        private void OnDied()
        {

#if !UNITY_EDITOR
            GameAnalytics.NewDesignEvent($"Hero:Died:OnLevel:{_poolData.CurrentLevelGame}");
#endif
            
            _windowModule.HeroDied();
        }

        private void OnStorageEntered() =>
            _basketPlayer.Replenishment().Forget();

        private void OnStorageExited() =>
            _basketPlayer.StopReplenishment();

        private void OnStorageGemEntered(StorageGem storageGem)
        {
            if (storageGem.IsEmpty)
                return;

            storageGem.GetGem(_wallet.ApplyGem).Forget();
        }

        private void OnStorageGemExited(StorageGem storageGem) => 
            storageGem.HeroOut();

        private void OnCartridgeGunEntered(CartridgeGun cartridgeGun)
        {
            if (_basketPlayer.IsEmpty)
                return;

            cartridgeGun.SetPresenceCourier(false);
            cartridgeGun.ApplyBox(_basketPlayer);
        }

        private void OnCartridgeGunExited(CartridgeGun cartridgeGun) =>
            cartridgeGun.SetPresenceCourier(true);
    }
}