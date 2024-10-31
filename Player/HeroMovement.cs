using System;
using Player.Animation;
using Plugins.MonoCache;
using Services.Inputs;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Hero))]
    public class HeroMovement : MonoCache
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _controller;

        private readonly float _rotationSpeed = 5.5f;

        private IInputService _input;
        private int _speed;
        private HeroAnimation _heroAnimation;
        private bool _isBattle;
        private Transform _currentTarget;
        private Vector3 _startPosition;

        public void Construct(IInputService input, int speed, HeroAnimation heroAnimation)
        {
            _heroAnimation = heroAnimation;
            _speed = speed;
            _input = input;
            _input.OnControls();

            _startPosition = transform.position;
        }

        private void OnValidate()
        {
            _controller ??= Get<CharacterController>();
            _animator ??= Get<Animator>();
        }

        protected override void UpdateCached()
        {
            Vector3 movementDirection = Vector3.zero;

            if (_input.MoveAxis.sqrMagnitude > Single.Epsilon)
            {
                _heroAnimation.EnableRun();
                movementDirection = new Vector3(_input.MoveAxis.x, Single.Epsilon, _input.MoveAxis.y);
            }
            else
            {
                _heroAnimation.EnableIdle();
            }
            
            Vector3 targetDirection = _isBattle
                ? (_currentTarget.position - transform.position).normalized
                : movementDirection.normalized;
                    
            Rotate(targetDirection);

            movementDirection += Physics.gravity;

            _controller.Move(movementDirection * (_speed * Time.deltaTime));
        }

        protected override void OnDisabled() =>
            _input.OffControls();

        public void SetStateBattle(bool status, Transform target)
        {
            _currentTarget = target;
            _isBattle = status;
        }
        
        public void Upgrade(int newSpeed) => 
            _speed = newSpeed;

        private void Rotate(Vector3 targetDirection)
        {
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, _rotationSpeed * Time.deltaTime, 0.0f);
            newDirection.y = 0.0f;
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        public void SetStartPosition()
        {
            _controller.enabled = false;
            transform.position = _startPosition;
            _controller.enabled = true;
            _input.OnControls();
        }
    }
}