using Code.Gameplay.Player;
using Code.Gameplay.Spawn;
using Code.Infrastructure.Services;
using Code.Infrastructure.Services.Camera;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Gameplay.Installers
{
    public class GameSceneInstaller : MonoBehaviour
    {
        [Header("Scene Components")]
        [SerializeField] private Camera _sceneCamera;
        [SerializeField] private SpawnPointRegistry[] _spawnPointRegistries;
        [SerializeField] private PlayerSpawner _playerSpawner;

        private void Start()
        {
            InitializeSceneAsync().Forget();
        }

        private async UniTaskVoid InitializeSceneAsync()
        {
            InitializeCameraService();
            
            await InitializeSpawnPoints();
            await InitializePlayer();
        }

        private void InitializeCameraService()
        {
            try
            {
                var cameraService = ServiceLocator.Instance.GetService<ICameraService>();
                
                if (_sceneCamera != null)
                {
                    cameraService.SetMainCamera(_sceneCamera);
                }
                else
                {
                    Debug.LogError("Scene camera is not assigned in GameSceneInstaller!");
                }
            }
            catch (System.InvalidOperationException)
            {
                Debug.LogError("CameraService is not registered in ServiceLocator!");
            }
        }

        private async UniTask InitializeSpawnPoints()
        {
            foreach (var spawnPointRegistry in _spawnPointRegistries)
            {
                if (spawnPointRegistry != null)
                {
                    await spawnPointRegistry.InitializeAsync();
                }
            }
        }

        private async UniTask InitializePlayer()
        {
            if (_playerSpawner != null)
            {
                await _playerSpawner.InitializeAsync();
            }
            else
            {
                Debug.LogError("PlayerSpawner is not assigned!");
            }
        }
    }
}
