using System;
using Cinemachine;
using Plugins.MonoCache;
using UnityEngine;

namespace CameraModule
{
    public class CameraFollow : MonoCache
    {
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private CinemachineVirtualCamera _cameraFollow;
        [SerializeField] private CinemachineVirtualCamera _zoomFollow;
        [SerializeField] private CinemachineVirtualCamera _markerCamera;

        [SerializeField] private Camera _camera;

        private readonly float _shakeIntensity = 1.5f;
        private readonly float _shakeTime = .2f;

        private readonly float _showMarkerTime = 3f;

        private readonly float _showBossTime = 4f;

        private CinemachineBasicMultiChannelPerlin _perlin;

        private float _timer;
        private bool _isShake;
        private bool _isShowMarker;
        private bool _isZoom;
        private bool _isShowBoss;

        public void Construct(Transform cameraRoot)
        {
            _cameraFollow.Follow = cameraRoot;
            _zoomFollow.Follow = cameraRoot;

            _cameraFollow.LookAt = cameraRoot;
            _zoomFollow.LookAt = cameraRoot;

            _perlin = _zoomFollow.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            StopShake();
        }

        public event Action OnShowed;

        protected override void UpdateCached()
        {
            if (_isShake)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                    StopShake();
            }

            if (_isShowMarker)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                    StopShowMarker();
            }

            if (_isShowBoss)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                {
                    OnShowed?.Invoke();

                    _isShowBoss = false;

                    if (_isZoom)
                        OnZoom();
                    else
                        OffZoom();
                }
            }
        }

        public AudioSource GetMusic =>
            _audioSource;

        public void Shake()
        {
            if (_isShake) 
                StopShake();

            _isShake = true;
            _perlin.m_AmplitudeGain = _shakeIntensity;
            _timer = _shakeTime;
        }

        public void OnZoom()
        {
            _isZoom = true;
            _zoomFollow.gameObject.SetActive(true);
            _cameraFollow.gameObject.SetActive(false);
        }

        public void OffZoom()
        {
            _isShake = false;
            _isZoom = false;
            _cameraFollow.gameObject.SetActive(true);
            _zoomFollow.gameObject.SetActive(false);
        }

        public Camera GetCameraMain =>
            _camera;

        private void StopShake()
        {
            _isShake = false;
            _perlin.m_AmplitudeGain = 0f;
            _timer = 0;
        }

        public void ShowBoss(Transform bossTransform)
        {
            _timer = _showBossTime;
            _isShowBoss = true;
            _markerCamera.Follow = bossTransform;

            _zoomFollow.gameObject.SetActive(false);
            _cameraFollow.gameObject.SetActive(false);
            _markerCamera.gameObject.SetActive(true);
        }

        public void ShowMarker(Transform rootCamera)
        {
            if (_isShowMarker)
                return;

            _timer = _showMarkerTime;
            _isShowMarker = true;
            _markerCamera.Follow = rootCamera;

            _zoomFollow.gameObject.SetActive(false);
            _cameraFollow.gameObject.SetActive(false);
            _markerCamera.gameObject.SetActive(true);
        }

        private void StopShowMarker()
        {
            _isShowMarker = false;

            _markerCamera.gameObject.SetActive(false);

            if (_isZoom)
                OnZoom();
            else
                OffZoom();
        }
    }
}