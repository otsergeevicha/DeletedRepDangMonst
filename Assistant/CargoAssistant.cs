using BehaviorDesigner.Runtime;
using Canvases;
using ContactZones;
using Plugins.MonoCache;
using SO;
using Triggers;
using Turrets.Children;
using UnityEngine;
using UnityEngine.AI;

namespace Assistant
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AssistantAnimation))]
    [RequireComponent(typeof(AmmoTriggers))]
    public class CargoAssistant : MonoCache
    {
        [HideInInspector] [SerializeField] private AmmoTriggers _ammoTriggers;
        [SerializeField] private BehaviorTree _tree;

        public BasketCargoAssistant Basket;
        
        private SectionPlate[] _sectionPlates;

        public AssistantAnimation AssistantAnimation { get; private set; }
        public AssistantData AssistantData { get; private set; }
        public StorageAmmoPlate StorageAmmoPlate { get; private set; }
        public CartridgeGun[] CartridgeGuns { get; private set; }

        public int GetMaxSizeBasket =>
            AssistantData.SizeBasket;

        public void Construct(AssistantData assistantData, CartridgeGun[] cartridgeGuns,
            StorageAmmoPlate storageAmmoPlate, SectionPlate[] sectionPlates)
        {
            _sectionPlates = sectionPlates;
            AssistantData = assistantData;
            StorageAmmoPlate = storageAmmoPlate;
            CartridgeGuns = cartridgeGuns;

            AssistantAnimation = Get<AssistantAnimation>();
            AssistantAnimation.Construct(assistantData);
        }

        private void OnValidate() =>
            _ammoTriggers ??= Get<AmmoTriggers>();

        public void OnActive(Transform spawnPoint)
        {
            SetPosition(spawnPoint);
            gameObject.SetActive(true);

            _ammoTriggers.StorageEntered += OnStorageEntered;
            _ammoTriggers.StorageExited += OnStorageExited;

            _ammoTriggers.CartridgeGunEntered += OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited += OnCartridgeGunExited;
        }

        public void InActive()
        {
            _ammoTriggers.StorageEntered -= OnStorageEntered;
            _ammoTriggers.StorageExited -= OnStorageExited;

            _ammoTriggers.CartridgeGunEntered -= OnCartridgeGunEntered;
            _ammoTriggers.CartridgeGunExited -= OnCartridgeGunExited;

            gameObject.SetActive(false);
        }

        private void SetPosition(Transform spawnPoint) =>
            transform.position = spawnPoint.position;

        private void OnStorageEntered() =>
            Basket.Replenishment().Forget();

        private void OnStorageExited() =>
            Basket.StopReplenishment();

        private void OnCartridgeGunEntered(CartridgeGun cartridgeGun)
        {
            if (Basket.IsEmpty)
                return;

            cartridgeGun.SetPresenceCourier(false);
            cartridgeGun.ApplyBox(Basket);
        }

        private void OnCartridgeGunExited(CartridgeGun cartridgeGun) =>
            cartridgeGun.SetPresenceCourier(true);
    }
}