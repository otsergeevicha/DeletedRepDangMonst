using System;
using Player;
using Plugins.MonoCache;
using SO;
using Triggers;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Animation
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimation : MonoCache
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _attackPoint;

        private const string LayerNameHero = "Player";
        private const string LayerNameGate = "BaseGate";
        private float _cleavage = 2f;

        private Collider[] _hits = new Collider[1];
        private int _layerMask;
        private EnemyData _enemyData;
        private Enemy _enemy;
        private NavMeshAgent _agent;

        public event Action OnAttacked;

        public void Construct(EnemyData enemyData, Enemy enemy, NavMeshAgent agent, bool isBoss)
        {
            _agent = agent;
            _enemy = enemy;
            _enemyData = enemyData;
            
            int mask1 = 1 << LayerMask.NameToLayer(LayerNameHero);
            int mask2 = 1 << LayerMask.NameToLayer(LayerNameGate);
            
            _layerMask = mask1 | mask2;

            if (isBoss) 
                _cleavage *= 2;
        }

        private void OnValidate() =>
            _animator ??= Get<Animator>();

        private void HitHero()
        {
            Physics.OverlapSphereNonAlloc(_attackPoint.position, _cleavage, _hits, _layerMask);

            if (_hits[0] != null)
            {
                if (_hits[0].gameObject.TryGetComponent(out Hero _)) 
                    _enemy.TakeDamage();

                if (_hits[0].gameObject.TryGetComponent(out BaseGate gate)) 
                    gate.TakeDamage();

                _hits[0] = null;
            }
        }

        private void Death() => 
            _enemy.Death();

        private void EndAttack() =>
            OnAttacked?.Invoke();

        public void EnableIdle()
        {
            _animator.SetBool(_enemyData.WalkHash, false);
            _animator.SetBool(_enemyData.AttackHash, false);
        }

        public void EnableRun()
        {
            _agent.isStopped = false;
            _animator.SetBool(_enemyData.AttackHash, false);
            _animator.SetBool(_enemyData.WalkHash, true);
        }

        public void EnableAgro()
        {
            _animator.SetBool(_enemyData.WalkHash, true);
            _animator.SetBool(_enemyData.AttackHash, false);
        }

        public void EnableAttack()
        {
            _agent.isStopped = true;
            _animator.SetBool(_enemyData.WalkHash, false);
            _animator.SetBool(_enemyData.AttackHash, true);
        }

        public void EnableDie()
        {
            _agent.isStopped = true;
            _animator.SetTrigger(_enemyData.DeathHash);
        }
    }
}