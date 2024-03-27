using Vikings.Chanacter;
using Vikings.Object;

namespace _Vikings.Refactoring.Character
{
    public class DoMoveState : BaseCharacterState
    {
        private PlayerController _playerController;
        private BoneFire _boneFire;
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

            if (stateMachine.IsIdle)
            {
                _boneFire = abstractObject as BoneFire;
                var pos = _boneFire.GetPosition();
                
                _boneFire.SetFlagState(pos, stateMachine);
                _playerController.MoveToPoint(pos, abstractObject.GetStoppingDistance());
                return;
            }

            if(_boneFire != null)
            {
                _boneFire.SetFlagState(stateMachine);
            }

            _playerController.MoveToPoint(abstractObject.GetPosition(), abstractObject.GetStoppingDistance());
        }

        public override void Exit()
        {
            _playerController.Clear();
            base.Exit();
        }
    }
}