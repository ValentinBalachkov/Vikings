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
            _playerController.SetActionOnGetPosition(() =>
            {
                stateMachine.OnNextAction();
            });
            _playerController.MoveToPoint( abstractObject.GetPosition(), abstractObject.GetStoppingDistance());
            _playerController.SetMoveAnimation();
        }

        public override void Exit()
        {
            _playerController.Clear();
            base.Exit();
        }
    }
}