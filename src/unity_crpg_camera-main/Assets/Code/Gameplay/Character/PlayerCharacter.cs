using Code.Configs;
using Code.Gameplay.Character.Animations;
using Code.Gameplay.Character.States;
using Code.Gameplay.Player;
using Code.Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Gameplay.Character
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Animator _animator;

        private PlayerInputHandler _inputHandler;
        private CameraInputHandler _cameraInputHandler;
        private CharacterMover _characterMover;
        private CharacterStateMachine _characterStateMachine;
        private CharacterAnimator _characterAnimator;
        private CharacterConfig _characterConfig;

        private bool _isInitialized;

        public void Initialize(CharacterConfig characterConfig)
        {
            _characterConfig = characterConfig;
            _characterMover = new CharacterMover(_navMeshAgent, _characterConfig.MovementSettings);
            InitializeStateMachine();

            _characterAnimator = new CharacterAnimator(_animator, _characterStateMachine);
            _inputHandler = new PlayerInputHandler();
            _inputHandler.OnDestinationSelected += _characterMover.SetDestination;

            _cameraInputHandler = new CameraInputHandler(transform);

            _characterStateMachine.ChangeState<CharacterIdleState>();
            _isInitialized = true;
        }

        private void Update()
        {
            if (_isInitialized == false) return;

            _inputHandler?.HandleInput();
            _cameraInputHandler?.HandleInput();
            _cameraInputHandler?.UpdateCamera();
            _characterStateMachine?.Update();
        }

        private void InitializeStateMachine()
        {
            _characterStateMachine = new CharacterStateMachine();
            
            var idleState = new CharacterIdleState(_characterMover, _characterStateMachine);
            var runState = new CharacterRunState(_characterMover, _characterStateMachine);
            
            _characterStateMachine.AddState(idleState);
            _characterStateMachine.AddState(runState);
        }

        private void OnDestroy()
        {
            _characterAnimator.Dispose();
            _inputHandler.OnDestinationSelected -= _characterMover.SetDestination;
        }
    }
}