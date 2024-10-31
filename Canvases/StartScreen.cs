using System;
using Plugins.MonoCache;
using UnityEngine;

namespace Canvases
{
    public class StartScreen : MonoCache
    {
        [HideInInspector] [SerializeField] private Canvas _canvas;
        
        private Hud _hud;

        public event Action OnClickStart;

        public void Construct(Hud hud) => 
            _hud = hud;

        private void OnValidate() => 
            _canvas ??= Get<Canvas>();

        public void OnActive()
        {
            _hud.InActive();
            Time.timeScale = 0;
            _canvas.enabled = true;
        }

        public void InActive()
        {
            _hud.OnActive();
            _canvas.enabled = false;
            OnClickStart?.Invoke();
            Time.timeScale = 1;
        }
    }
}