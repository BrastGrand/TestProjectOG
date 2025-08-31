namespace Code.Infrastructure.StateMachine
{
    public interface ICharacterState
    {
        CharacterState GetState();
        void Enter();
        void Update();
        void Exit();
    }
}

