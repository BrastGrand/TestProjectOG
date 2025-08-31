using System;
using System.Collections.Generic;
using Code.Gameplay.Spawn;
using Code.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Spawn
{
    public class SpawnService : ISpawnService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<string, SpawnPoint> _spawnPoints = new Dictionary<string, SpawnPoint>();

        public SpawnService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
        }

        public void RegisterSpawnPoint(SpawnPoint spawnPoint)
        {
            if (spawnPoint == null)
            {
                Debug.LogError("Cannot register null spawn point!");
                return;
            }

            var id = spawnPoint.Id;
            _spawnPoints.TryAdd(id, spawnPoint);
        }

        public void UnregisterSpawnPoint(SpawnPoint spawnPoint)
        {
            if (spawnPoint == null)
                return;

            var id = spawnPoint.Id;
            if (_spawnPoints.Remove(id))
            {
                Debug.Log($"Spawn point '{id}' unregistered");
            }
        }

        public SpawnPoint GetSpawnPoint(string id)
        {
            _spawnPoints.TryGetValue(id, out var spawnPoint);
            return spawnPoint;
        }

        public async UniTask<GameObject> SpawnAsync(string prefabKey, string spawnPointId)
        {
            var spawnPoint = GetSpawnPoint(spawnPointId);
            if (spawnPoint != null)
                return await SpawnAsync(prefabKey, spawnPoint.Position, spawnPoint.Rotation);

            Debug.LogError($"Spawn point with ID '{spawnPointId}' not found!");
            return null;
        }

        public async UniTask<GameObject> SpawnAsync(string prefabKey, Vector3 position, Quaternion rotation)
        {
            try
            {
                var gameObject = await _assetProvider.Instantiate(prefabKey, position);
                if (gameObject != null)
                {
                    gameObject.transform.rotation = rotation;
                }
                return gameObject;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to spawn '{prefabKey}': {e.Message}");
                return null;
            }
        }

        public async UniTask<T> SpawnAsync<T>(string prefabKey, string spawnPointId) where T : Component
        {
            var gameObject = await SpawnAsync(prefabKey, spawnPointId);
            return gameObject?.GetComponent<T>();
        }

        public async UniTask<T> SpawnAsync<T>(string prefabKey, Vector3 position, Quaternion rotation) where T : Component
        {
            var gameObject = await SpawnAsync(prefabKey, position, rotation);
            return gameObject?.GetComponent<T>();
        }
    }
}

