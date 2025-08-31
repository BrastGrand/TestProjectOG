using Code.Gameplay.Character.Settings;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Settings/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [Header("Player Character Prefab")]
        [SerializeField] private string _playerCharacterKey = "Character";

        [Header("Movement Settings")]
        [SerializeField] private MovementSettings _movementSettings;
        public MovementSettings MovementSettings => _movementSettings;
        public string PlayerCharacterKey => _playerCharacterKey;
    }
}

