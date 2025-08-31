using System.Threading.Tasks;
using Code.Infrastructure.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Code.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        Task InitializeAsync();
        Task<TAsset> Load<TAsset>(string key) where TAsset : class;
        Task WarmupAssetsByLabel(string label);
        Task ReleaseAssetsByLabel(string label);
        Task<SceneInstance> LoadScene(string sceneName);
        Task<GameObject> Instantiate(string address);
        Task<GameObject> Instantiate(string address, Vector3 at);
        Task<GameObject> Instantiate(string address, Transform under);
        Task<GameObject> Instantiate(string address, Vector3 at, Transform under);
        void Cleanup();
    }
}
