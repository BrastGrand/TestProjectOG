using Code.Infrastructure.StateMachine;
using UnityEngine;

namespace Code.Gameplay.Character.States
{
    public class CharacterIdleState : ICharacterState
    {
        private readonly CharacterStateMachine _characterStateMachine;
        private readonly CharacterMover _characterMover;

        public CharacterState GetState() => CharacterState.Idle;

        public CharacterIdleState(CharacterMover characterMover, CharacterStateMachine characterStateMachine)
        {
            _characterMover = characterMover;
            _characterStateMachine = characterStateMachine;
        }

        public void Enter()
        {
            Debug.Log("Character entered Idle state");
        }

        public void Update()
        {
            if (_characterMover.IsMoving)
            {
                _characterStateMachine.ChangeState<CharacterRunState>();
            }
        }

        public void Exit()
        {
            Debug.Log("Character exited Idle state");
        }
    }
}

