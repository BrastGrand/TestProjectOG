using Code.Infrastructure.Services;
using Code.Infrastructure.Services.Spawn;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Gameplay.Spawn
{
    public class SpawnPointRegistry : MonoBehaviour
    {
        private ISpawnService _spawnService;
        private SpawnPoint _spawnPoint;

        private void Awake()
        {
            _spawnPoint = GetComponent<SpawnPoint>();
            if (_spawnPoint == null)
            {
                Debug.LogError("SpawnPointRegistry requires SpawnPoint component!");
            }
        }

        public async UniTask InitializeAsync()
        {
            if (_spawnPoint == null)
                return;

            InitializeServices();
            RegisterSpawnPoint();
            await UniTask.CompletedTask;
        }

        private void InitializeServices()
        {
            try
            {
                _spawnService = ServiceLocator.Instance.GetService<ISpawnService>();
            }
            catch (System.InvalidOperationException)
            {
                Debug.LogError("SpawnService is not registered in ServiceLocator!");
            }
        }

        private void RegisterSpawnPoint()
        {
            if (_spawnService != null && _spawnPoint != null)
            {
                _spawnService.RegisterSpawnPoint(_spawnPoint);
                Debug.Log($"SpawnPoint '{_spawnPoint.Id}' registered successfully");
            }
        }

        private void OnDestroy()
        {
            if (_spawnService != null && _spawnPoint != null)
            {
                _spawnService.UnregisterSpawnPoint(_spawnPoint);
            }
        }
    }
}
