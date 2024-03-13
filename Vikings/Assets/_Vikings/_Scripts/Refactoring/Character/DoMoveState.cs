using Vikings.Chanacter;
using Vikings.Object;

namespace _Vikings.Refactoring.Character
{
    public class DoMoveState : BaseCharacterState
    {
        private PlayerController _playerController;
        public DoMoveState(string stateName, CharacterStateMachine stateMachine, PlayerController playerController) : base(stateName, stateMachine)
        {
            _playerController = playerController;
        }

        public override void Enter(AbstractObject abstractObject)
        {
            base.Enter(abstractObject);
            _playerController.MoveToPoint(abstractObject.GetPosition());
            
            _playerController.SetActionOnGetPosition(() =>
            {
                stateMachine.SetState<DoActionState>(abstractObject);
            });
        }

        public override void Exit()
        {
            _playerController.ClearAction();
            base.Exit();
        }
    }
}