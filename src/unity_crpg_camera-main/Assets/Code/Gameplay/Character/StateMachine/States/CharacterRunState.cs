using Code.Infrastructure.StateMachine;
using UnityEngine;

namespace Code.Gameplay.Character.States
{
    public class CharacterRunState : ICharacterState
    {
        private readonly CharacterMover _characterMover;
        private readonly CharacterStateMachine _characterStateMachine;

        public CharacterState GetState() => CharacterState.Run;

        public CharacterRunState(CharacterMover characterMover, CharacterStateMachine characterStateMachine)
        {
            _characterMover = characterMover;
            _characterStateMachine = characterStateMachine;
        }

        public void Enter()
        {
            Debug.Log("Character entered Run state");
        }

        public void Update()
        {
            if (!_characterMover.IsMoving)
            {
                _characterStateMachine.ChangeState<CharacterIdleState>();
            }
        }

        public void Exit()
        {
            Debug.Log("Character exited Run state");
        }
    }
}
