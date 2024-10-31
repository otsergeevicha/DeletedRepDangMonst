using ContactZones;
using Plugins.MonoCache;
using SO;
using UnityEngine;
using UnityEngine.AI;

namespace Workers
{
    [RequireComponent(typeof(WorkerAnimation))]
    public class Worker : MonoCache
    {
        [HideInInspector] [SerializeField] private NavMeshAgent _agent;

        [HideInInspector] [SerializeField] private CapsuleCollider _collider;
        [HideInInspector] [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Transform _hummer;
        [SerializeField] private Transform _gem;
        
        private Transform[] _gemMiners;
        private StorageGem _storageGem;

        public void Construct(WorkerData workerData, Transform[] gemMiners, Vector3 storageGemPoint,
            StorageGem storageGem)
        {
            _storageGem = storageGem;
            StorageGemPoint = storageGemPoint;
            _gemMiners = gemMiners;
            WorkerData = workerData;
            WorkerAnimation = Get<WorkerAnimation>();
            WorkerAnimation.Construct(workerData);
        }

        private void OnValidate()
        {
            _agent ??= Get<NavMeshAgent>();
            _collider ??= Get<CapsuleCollider>();
            _rigidbody ??= Get<Rigidbody>();
        }

        public Vector3 Workplace { get; private set; }
        public Vector3 StorageGemPoint { get; private set; }
        public Vector3 CurrentGemPoint { get; private set; }
        public WorkerData WorkerData { get; private set; }
        public WorkerAnimation WorkerAnimation { get; private set; }
        public bool IsReadyWork { get; private set; }
        public bool IsProcessMining { get; set; }
        public bool AtWork { get; private set; }

        public bool IsStorageFulled =>
            _storageGem.IsFulled;

        public bool IsHandEmpty { get; set; } = true;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent(out StorageGem storage))
            {
                if (!IsHandEmpty)
                {
                    storage.ApplyGem();
                    IsHandEmpty = true;
                    _gem.gameObject.SetActive(false);
                }
            }
        }

        public void OnActive() =>
            gameObject.SetActive(true);

        public void InActive() =>
            gameObject.SetActive(false);

        public void SendWorkplace(Vector3 workplace)
        {
            Workplace = workplace;
            _agent.enabled = true;
            IsReadyWork = true;
            GetFreeGemPosition();
        }

        public void Reset()
        {
            _agent.enabled = false;

            IsReadyWork = false;
            AtWork = false;
        }

        private void GetFreeGemPosition() =>
            CurrentGemPoint = _gemMiners[Random.Range(0, _gemMiners.Length)].position;

        public void OnAtWork()
        {
            AtWork = true;
            _hummer.gameObject.SetActive(true);
        }

        public void SetMannequin(bool flag)
        {
            _collider.enabled = flag;
            _rigidbody.useGravity = flag;
        }

        private void EndMining()
        {
            IsProcessMining = false;
            IsHandEmpty = false;
            _gem.gameObject.SetActive(true);
        }
    }
}