using System;
using Code.Configs;
using Code.Gameplay.Character;
using Code.Infrastructure.AssetManagement;
using Code.Infrastructure.Services;
using Code.Infrastructure.Services.Spawn;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Gameplay.Player
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private string _playerSpawnPointId = "Player";
        [SerializeField] private string _characterConfigKey = "CharacterConfig";

        private ISpawnService _spawnService;
        private IAssetProvider _assetProvider;
        private GameObject _currentPlayer;

        public async UniTask InitializeAsync()
        {
            InitializeServices();
            await SpawnPlayerAsync();
        }

        private void InitializeServices()
        {
            try
            {
                _spawnService = ServiceLocator.Instance.GetService<ISpawnService>();
                _assetProvider = ServiceLocator.Instance.GetService<IAssetProvider>();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private async UniTask SpawnPlayerAsync()
        {
            if (_spawnService == null)
            {
                Debug.LogError("Cannot spawn player - SpawnService is null!");
                return;
            }

            if (_assetProvider == null)
            {
                Debug.LogError("Cannot spawn player - AssetProvider is null!");
                return;
            }

            var characterConfig = await _assetProvider.Load<CharacterConfig>(_characterConfigKey);

            if (characterConfig == null)
            {
                Debug.LogError($"Fail loading character settings with key {_characterConfigKey}");
                return;
            }

            _currentPlayer = await _spawnService.SpawnAsync(characterConfig.PlayerCharacterKey, _playerSpawnPointId);

            if (_currentPlayer == null)
            {
                Debug.LogError("Failed to spawn player!");
                return;
            }

            var character = _currentPlayer.GetComponent<PlayerCharacter>();

            if (character == null)
            {
                Debug.LogError("Not found PlayerCharacter component");
                return;
            }

            character.Initialize(characterConfig);
        }

        private void OnDestroy()
        {
            if (_currentPlayer != null)
            {
                Destroy(_currentPlayer);
            }
        }
    }
}
