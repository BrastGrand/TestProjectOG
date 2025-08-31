using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Code.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public async Task InitializeAsync() => await Addressables.InitializeAsync();

        public async Task<TAsset> Load<TAsset>(string key) where TAsset : class
        {
            if (_completedCache.TryGetValue(key, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as TAsset;

            var validateAddress = Addressables.LoadResourceLocationsAsync(key);
            await validateAddress.Task;

            if (validateAddress.Status != AsyncOperationStatus.Succeeded || validateAddress.Result.Count == 0)
            {
                Debug.LogError($"Ресурсы по имени {key} не найдены или загрузка завершилась ошибкой.");
                Addressables.Release(validateAddress);
                return null;
            }

            var handle = Addressables.LoadAssetAsync<TAsset>(key);
            try
            {
                await handle.Task;
                AddHandle(key, handle);
                return handle.Result as TAsset;
            }
            catch (Exception e)
            {
                Debug.LogError($"Ошибка при загрузке ассета {key}: {e}");
                Addressables.Release(handle);
                return null;
            }
        }

        public async Task WarmupAssetsByLabel(string label)
        {
            var assetsList = await GetAssetsListByLabel(label);
            await LoadAll<object>(assetsList);
        }

        public async Task ReleaseAssetsByLabel(string label)
        {
            var assetsList = await GetAssetsListByLabel(label);

            foreach (var assetKey in assetsList)
            {
                if (!_handles.TryGetValue(assetKey, out var handles))
                    continue;

                foreach (var handle in handles.Where(handle => handle.IsValid()))
                {
                    Addressables.Release(handle);
                }

                _handles.Remove(assetKey);
            }
        }

        public async Task<GameObject> Instantiate(string address)
        {
            var handle = Addressables.InstantiateAsync(address);
            AddHandle(address, handle);
            return await handle.Task;
        }

        public async Task<GameObject> Instantiate(string address, Vector3 at)
        {
            var handle = Addressables.InstantiateAsync(address, at, Quaternion.identity);
            AddHandle(address, handle);
            return await handle.Task;
        }

        public async Task<GameObject> Instantiate(string address, Transform under)
        {
            var handle = Addressables.InstantiateAsync(address, under);
            AddHandle(address, handle);
            return await handle.Task;
        }

        public async Task<GameObject> Instantiate(string address, Vector3 at, Transform under)
        {
            var handle = Addressables.InstantiateAsync(address, at, Quaternion.identity, under);
            AddHandle(address, handle);
            return await handle.Task;
        }

        public async Task<SceneInstance> LoadScene(string sceneName)
        {
            var handle = Addressables.LoadSceneAsync(sceneName);
            AddHandle(sceneName, handle);
            return await handle.Task;
        }

        public void Cleanup()
        {
            foreach (var resourceHandles in _handles.Values)
            {
                foreach (var handle in resourceHandles.Where(handle => handle.IsValid()))
                {
                    Addressables.Release(handle);
                }
            }

            _completedCache.Clear();
            _handles.Clear();
        }

        private void AddHandle<T>(string key, AsyncOperationHandle<T> handle)
        {
            if (!_handles.TryGetValue(key, out var handles))
            {
                handles = new List<AsyncOperationHandle>();
                _handles[key] = handles;
            }
            handles.Add(handle);
        }

        private async UniTask<List<string>> GetAssetsListByLabel(string label, Type type = null)
        {
            var operationHandle = Addressables.LoadResourceLocationsAsync(label, type);
            var locations = await operationHandle.ToUniTask();
            List<string> assetKeys = new List<string>(locations.Count);

            foreach (var location in locations)
                assetKeys.Add(location.PrimaryKey);

            Addressables.Release(operationHandle);
            return assetKeys;
        }

        private async Task<TAsset[]> LoadAll<TAsset>(List<string> keys) where TAsset : class
        {
            List<Task<TAsset>> tasks = new List<Task<TAsset>>(keys.Count);

            foreach (var key in keys)
                tasks.Add(Load<TAsset>(key));

            return await Task.WhenAll(tasks);
        }
    }
}