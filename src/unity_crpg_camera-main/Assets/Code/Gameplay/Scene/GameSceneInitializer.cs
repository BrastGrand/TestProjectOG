using Code.Gameplay.Player;
using Code.Gameplay.Spawn;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Gameplay.Scene
{
    public class GameSceneInitializer : MonoBehaviour
    {
        [Header("Scene Components")]
        [SerializeField] private SpawnPointRegistry[] _spawnPointRegistries;
        [SerializeField] private PlayerSpawner _playerSpawner;

        private void Start()
        {
            InitializeSceneAsync().Forget();
        }

        private async UniTaskVoid InitializeSceneAsync()
        {
            Debug.Log("GameSceneInitializer: Starting scene initialization...");

            await InitializeSpawnPoints();
            await InitializePlayer();
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

