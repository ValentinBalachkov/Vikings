using UnityEngine;
using Vikings.Items;

namespace Vikings.Chanacter
{
    public class MovingState : BaseState
    {
        private ItemsOnMapController _itemsOnMapController;
        private CharacterStateMachine _stateMachine;

        private ItemController _currentPoint;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.3f;
        public MovingState(CharacterStateMachine stateMachine, ItemsOnMapController itemsOnMapController, PlayerController playerPrefab) : base("Moving",
            stateMachine)
        {
            _itemsOnMapController = itemsOnMapController;
            _playerPrefab = playerPrefab;
            _stateMachine = stateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            _currentPoint = _itemsOnMapController.GetElementPosition();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            _playerPrefab.MoveToPoint(_currentPoint.transform);
            if (!(Vector3.Distance(_playerPrefab.transform.position, _currentPoint.transform.position) <=
                  OFFSET_DISTANCE)) return;
            _itemsOnMapController.AddElementToQueue(_currentPoint);
            _stateMachine.SetState<CollectState>();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
    }
}