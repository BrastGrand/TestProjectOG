using Code.Gameplay.Character.Settings;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Services;
using Code.Infrastructure.Services.Camera;
using Code.Infrastructure.Services.Spawn;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure.Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private string _gameSceneName = "Scene_00";
        [SerializeField] private CameraSettings _cameraSettings;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            InitializeServices();
            LoadGameScene();
        }

        private void InitializeServices()
        {
            var serviceLocator = ServiceLocator.Instance;

            var assetProvider = new AssetProvider();
            serviceLocator.RegisterService<IAssetProvider>(assetProvider);

            var cameraService = new CameraService(_cameraSettings);
            serviceLocator.RegisterService<ICameraService>(cameraService);

            var spawnService = new SpawnService(assetProvider);
            serviceLocator.RegisterService<ISpawnService>(spawnService);
        }

        private void LoadGameScene()
        {
            Debug.Log($"Loading game scene: {_gameSceneName}");
            SceneManager.LoadScene(_gameSceneName);
        }
    }
}
