using Ammo;
using Assistant;
using CameraModule;
using Canvases;
using Canvases.UpgradePlayer;
using Effects;
using Enemies;
using HpBar;
using Infrastructure.Factory.Pools;
using Loots;
using Markers;
using Modules;
using Player;
using Spawners;
using Turrets;
using Workers;

namespace Services.Factory
{
    public interface IGameFactory
    {
        Hero CreateHero();
        CameraFollow CreateCamera();
        AmmoBox CreateAmmoBox();
        Pool CreatePool();
        CargoAssistant CreateCargoAssistant();
        Enemy CreateEnemy(string currentPath);
        EnemySpawner CreateEnemySpawner();
        Turret CreateTurret();
        Missile CreateMissile();
        Hud CreateHud();
        Money CreateMoney();
        HeroAimRing CreateHeroAimRing();
        EnemyRing CreateEnemyRing();
        HealthBar CreateHealthBar();
        LoseScreen CreateLoseScreen();
        StartScreen CreateStartScreen();
        LootPoint CreateLootPoint();
        Worker CreateWorker();
        WorkerSpawner CreateWorkerSpawner();
        WinScreen CreateWinScreen();
        UpgradeHeroScreen CreateUpgradeHeroScreen();
        VfxHitRed CreateVfxHit();
        VfxMissileExplosion CreateVfxExplosion();
        TutorialMarker CreateTutorialMarker();
        CoinBlastVfx CreateCoinBlastVfx();
        Bullet CreateBullet();
    }
}