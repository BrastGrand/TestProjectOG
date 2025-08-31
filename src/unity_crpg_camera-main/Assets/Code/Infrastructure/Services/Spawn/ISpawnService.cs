using System.Collections.Generic;
using Code.Gameplay.Spawn;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Infrastructure.Services.Spawn
{
    public interface ISpawnService : IService
    {
        void RegisterSpawnPoint(SpawnPoint spawnPoint);
        void UnregisterSpawnPoint(SpawnPoint spawnPoint);
        SpawnPoint GetSpawnPoint(string id);
        UniTask<GameObject> SpawnAsync(string prefabKey, string spawnPointId);
        UniTask<GameObject> SpawnAsync(string prefabKey, Vector3 position, Quaternion rotation);
        UniTask<T> SpawnAsync<T>(string prefabKey, string spawnPointId) where T : Component;
        UniTask<T> SpawnAsync<T>(string prefabKey, Vector3 position, Quaternion rotation) where T : Component;
    }
}

