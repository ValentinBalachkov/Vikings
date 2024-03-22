using Vikings.Object;

namespace _Vikings.Refactoring.Character
{
    public class DoActionState : BaseCharacterState
    {
        public DoActionState(string stateName, CharacterStateMachine stateMachine) : base(stateName, stateMachine)
        {
            
        }

        public override void Enter(AbstractObject abstractObject)
        {
            base.Enter(abstractObject);
            abstractObject.EndAction += stateMachine.OnNextAction;
            abstractObject.CharacterAction(stateMachine);
        }
        

        public override void Exit()
        {
            _abstractObject.EndAction -= stateMachine.OnNextAction;
            base.Exit();
        }
    }
}