using System;
using Canvases;
using Services.Inputs;
using UnityEngine;

namespace Inputs
{
    public class InputService : IInputService
    {
        private readonly MapInputs _input = new ();

        public event Action OnJoystick;
        public event Action OffJoystick;
        
        public InputService()
        {
            _input.Player.Touch.started += _ =>
                OnJoystick?.Invoke();
            _input.Player.Touch.canceled += _ =>
                OffJoystick?.Invoke();
        }
        
        public Vector2 MoveAxis =>
             _input.Player.Move.ReadValue<Vector2>();

        public Vector2 TouchJoystick => 
            _input.Player.TouchPosition.ReadValue<Vector2>();

        public void SelectUzi(Action<int> onPressed) =>
            _input.Player.Uzi.started += _ =>
                onPressed?.Invoke((int)WeaponType.Uzi);
        
        public void SelectAutoPistol(Action<int> onPressed) =>
            _input.Player.AutoPistol.started += _ =>
                onPressed?.Invoke((int)WeaponType.AutoPistol);
        
        public void SelectRifle(Action<int> onPressed) =>
            _input.Player.Rifle.started += _ =>
                onPressed?.Invoke((int)WeaponType.Rifle);
        
        public void SelectMiniGun(Action<int> onPressed) =>
            _input.Player.MiniGun.started += _ =>
                onPressed?.Invoke((int)WeaponType.MiniGun);

        public void OnControls() => 
            _input.Player.Enable();

        public void OffControls() => 
             _input.Player.Disable();
    }
}