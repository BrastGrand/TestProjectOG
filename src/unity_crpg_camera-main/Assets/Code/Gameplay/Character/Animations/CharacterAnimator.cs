using System;
using System.Collections.Generic;
using Code.Infrastructure.StateMachine;
using UnityEngine;

namespace Code.Gameplay.Character.Animations
{
    public class CharacterAnimator : IDisposable
    {
        private readonly Animator _animator;
        private readonly CharacterStateMachine _characterStateMachine;

        private readonly Dictionary<CharacterState, string> _animationTriggers = new Dictionary<CharacterState, string>
        {
            { CharacterState.Idle, "Idle" },
            { CharacterState.Run, "Run" },
        };

        public CharacterAnimator(Animator animator, CharacterStateMachine stateMachine)
        {
            _animator = animator;
            _characterStateMachine = stateMachine;
            _characterStateMachine.OnStateChanged += UpdateAnimation;
            UpdateAnimation(CharacterState.Idle);
        }

        private void UpdateAnimation(CharacterState state)
        {
            if (_animator == null)
            {
                return;
            }

            _animationTriggers.TryGetValue(state, out string triggerName);

            if (string.IsNullOrEmpty(triggerName))
            {
                return;
            }

            _animator.SetTrigger(triggerName);
        }

        public void Dispose()
        {
            _characterStateMachine.OnStateChanged -= UpdateAnimation;
        }
    }
}