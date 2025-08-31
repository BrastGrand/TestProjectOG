using System;
using System.Collections.Generic;

namespace Code.Infrastructure.StateMachine
{
    public class CharacterStateMachine
    {
        private readonly Dictionary<Type, ICharacterState> _states = new Dictionary<Type, ICharacterState>();
        private ICharacterState _currentCharacterState;

        public event Action<CharacterState> OnStateChanged;

        public void AddState<T>(T state) where T : class, ICharacterState
        {
            var type = typeof(T);
            if (_states.ContainsKey(type))
            {
                UnityEngine.Debug.LogWarning($"State of type {type.Name} is already added. Overwriting...");
            }
            
            _states[type] = state;
        }

        public void ChangeState<T>() where T : class, ICharacterState
        {
            var type = typeof(T);
            if (!_states.TryGetValue(type, out var newState))
            {
                throw new InvalidOperationException($"State of type {type.Name} is not registered.");
            }

            if (_currentCharacterState == newState)
                return;

            _currentCharacterState?.Exit();
            _currentCharacterState = newState;
            _currentCharacterState.Enter();

            OnStateChanged?.Invoke(_currentCharacterState.GetState());
        }

        public void Update()
        {
            _currentCharacterState?.Update();
        }
    }

    public enum CharacterState
    {
        Idle,
        Run
    }
}

