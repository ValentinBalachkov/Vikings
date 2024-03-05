using System;
using Vikings.Object;

namespace _Vikings.Refactoring.Character
{
    public class DoActionState : BaseCharacterState
    {
        private Action<CharacterStateMachine> _onCharacterAction;
        public DoActionState(string stateName, CharacterStateMachine stateMachine, Action<CharacterStateMachine> OnCharacterAction) : base(stateName, stateMachine)
        {
            _onCharacterAction = OnCharacterAction;
        }

        public override void Enter(AbstractObject abstractObject)
        {
            base.Enter(abstractObject);
            abstractObject.CharacterAction(stateMachine);
            _onCharacterAction?.Invoke(stateMachine);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}