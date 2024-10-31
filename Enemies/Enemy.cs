using System;
using System.Linq;
using BehaviorDesigner.Runtime;
using ContactZones;
using Enemies.Animation;
using HpBar;
using Modules;
using Player;
using Plugins.MonoCache;
using SO;
using Spawners;
using Triggers;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    enum EnemyId
    {
        ZeroLevel,
        OneLevel,
        TwoLevel,
        ThreeLevel,
        FourLevel,
        FiveLevel,
        SixLevel,
        SevenLevel,
        EightLevel,
        NineLevel
    }

    enum BossId
    {
        OneLevel = 10,
        TwoLevel = 20,
        ThreeLevel = 30,
        FourLevel = 40,
        FiveLevel = 50,
        SixLevel = 60,
        SevenLevel = 70,
        EightLevel = 80,
        NineLevel = 90,
        TenLevel = 100
    }

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class Enemy : MonoCache
    {
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private EnemyTriggers _enemyTriggers;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private BehaviorTree _tree;

        private readonly BossId[] _bossLevels = Enum.GetValues(typeof(BossId)).Cast<BossId>().ToArray();

        public EnemyAnimation EnemyAnimation;
        public Transform CashTransform;
        
        protected int MaxHealth;
        protected int Damage;

        private HealthBar _healthBar;
        private LootSpawner _lootSpawner;
        private EnemyHealthModule _enemyHealthModule;
        private FinishPlate _finishPlate;

        private int _currentHealth;
        private bool _onMap;

        private Hero _hero;

        public bool IsIdleBoss  { get; private set; } = true;

        public bool IsAgro { get; private set; }
        public bool IsDie { get; private set; }
        public EnemyData EnemyData { get; private set; }
        public Transform GetCurrentTarget { get; private set; }
        public float CurrentAttackDistance { get; private set; }
        public int CurrentAgroDistance { get; private set; }
        
        protected abstract int GetId();
        protected abstract void SetCurrentHealth();
        protected abstract void SetCurrentDamage();
        public event Action Died;

        public void Construct(EnemyData enemyData,
            EnemyHealthModule enemyHealthModule, LootSpawner lootSpawner, HealthBar healthBar, FinishPlate finishPlate,
            Hero hero, Transform portal)
        {
            GetCurrentTarget = portal;
            
            _enemyTriggers.SetRadius(enemyData.AgroDistance);

            CashTransform = transform;
            _tree.enabled = false;

            _hero = hero;
            _finishPlate = finishPlate;
            _healthBar = healthBar;
            _lootSpawner = lootSpawner;
            _enemyHealthModule = enemyHealthModule;
            EnemyData = enemyData;

            CurrentAttackDistance = _bossLevels.Contains((BossId)GetId()) 
                ? EnemyData.BossAttackDistance 
                : EnemyData.AttackDistance;
            
            CurrentAgroDistance = EnemyData.AgroDistance;
            
            SetCurrentHealth();
            SetCurrentDamage();

            EnemyAnimation.Construct(enemyData, this, _agent, _bossLevels.Contains((BossId)GetId()));

            if (_bossLevels.Contains((BossId)GetId())) 
                IsIdleBoss = true;

            _enemyTriggers.OnAgroGate += 
                gate =>
            {
                IsAgro = true;
                GetCurrentTarget = gate.AgroPoint;
                ResetBehaviorTree();

                gate.Destroyed += () =>
                {
                    IsAgro = false;
                    GetCurrentTarget = portal;
                    ResetBehaviorTree();
                };
            };
            
            _enemyTriggers.NonAgroGate += () =>
            {
                IsAgro = false;
                GetCurrentTarget = portal;
                ResetBehaviorTree();
            };
            
            _enemyTriggers.OnAgro += () =>
            {
                if (_bossLevels.Contains((BossId)GetId())) 
                    IsIdleBoss = false;
                
                IsAgro = true;
                GetCurrentTarget = _hero.transform;
                ResetBehaviorTree();
            };

            _enemyTriggers.NonAgro += () =>
            {
                if (_bossLevels.Contains((BossId)GetId())) 
                    IsIdleBoss = true;
                
                IsAgro = false;
                GetCurrentTarget = portal;
                ResetBehaviorTree();
            };

            ResetHealth();

            _agent.stoppingDistance = enemyData.AttackDistance;
        }

        private void OnValidate()
        {
            EnemyAnimation ??= ChildrenGet<EnemyAnimation>();
            _enemyTriggers ??= ChildrenGet<EnemyTriggers>();
            _agent ??= Get<NavMeshAgent>();
            _tree ??= Get<BehaviorTree>();
            _deathSound ??= Get<AudioSource>();
        }

        public void Escape()
        {
            _deathSound.Play();
            _healthBar.InActive();
            Died?.Invoke();
            IsDie = true;
            _agent.isStopped = true;
            InActive();
        }

        public virtual void OnActive()
        {
            IsDie = false;
            gameObject.SetActive(true);
            IsAgro = false;
            _tree.enabled = true;
            
            ResetHealth();
        }

        public virtual void InActive()
        {
            if (_bossLevels.Contains((BossId)GetId())) 
                IsIdleBoss = true;
            
            IsDie = true;
            gameObject.SetActive(false);
        }

        public void ApplyDamage(int damage)
        {
            _currentHealth = _enemyHealthModule.CalculateDamage(_currentHealth, damage);

            _healthBar.ChangeValue(_currentHealth, MaxHealth, damage);

            if (_currentHealth <= 0)
            {
                if (_bossLevels.Contains((BossId)GetId()))
                    _finishPlate.OnActive();

                _tree.enabled = false;

                _currentHealth = 0;
                _healthBar.InActive();
                Died?.Invoke();
                IsDie = true;

                EnemyAnimation.EnableDie();
                _deathSound.Play();
            }
        }

        public void TakeDamage() =>
            _hero.ApplyDamage(Damage);

        public void Death()
        {
            SpawnLoot();
            InActive();
        }

        public void OnDestroy()
        {
            Destroy(_healthBar);
            Destroy(this);
        }

        public void ResetBehaviorTree()
        {
            _tree.enabled = false;
            _tree.enabled = true;
        }

        public void OnSleep() => 
            _tree.enabled = false;

        public void UnSleep() => 
            _tree.enabled = true;

        public bool InZone() =>
            Vector3.Distance(CashTransform.position, _hero.transform.position) <=
            EnemyData.AttackDistance;

        private void SpawnLoot()
        {
            if (_bossLevels.Contains((BossId)GetId()))
                return;

            _lootSpawner.SpawnMoney(GetId(), transform.position);
        }

        private void ResetHealth() =>
            _currentHealth = MaxHealth;
    }
}