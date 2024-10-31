using System;
using UnityEngine;

namespace Services.Inputs
{
    public interface IInputService
    {
        event Action OnJoystick;
        event Action OffJoystick;
        Vector2 MoveAxis { get; }
        Vector2 TouchJoystick { get; }
        void SelectUzi(Action<int> onPressed);
        void SelectAutoPistol(Action<int> onPressed);
        void SelectRifle(Action<int> onPressed);
        void SelectMiniGun(Action<int> onPressed);
        void OnControls();
        void OffControls();
    }
}