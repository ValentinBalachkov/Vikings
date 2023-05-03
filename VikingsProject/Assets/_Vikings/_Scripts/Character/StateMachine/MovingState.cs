using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class MovingState : BaseState
    {
        private BuildingsOnMap _buildingsOnMap;
        private CharacterStateMachine _stateMachine;
        private InventoryController _inventoryController;

        private IGetItem _currentPoint;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.5f;
        public MovingState(CharacterStateMachine stateMachine, BuildingsOnMap buildingsOnMap, PlayerController playerPrefab, InventoryController inventoryController) : base("Moving",
            stateMachine)
        {
            _buildingsOnMap = buildingsOnMap;
            _playerPrefab = playerPrefab;
            _stateMachine = stateMachine;
            _inventoryController = inventoryController;
        }

        public override void Enter()
        {
            base.Enter();
            _currentPoint = _buildingsOnMap.GetElementPosition();
            _inventoryController.SetItem(_currentPoint);
            _playerPrefab.SetMoveAnimation();
        }
        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            _playerPrefab.MoveToPoint(_currentPoint.GetItemPosition());
            if (!(Vector3.Distance(_playerPrefab.transform.position, _currentPoint.GetItemPosition().position) <=
                  OFFSET_DISTANCE)) return;
            _playerPrefab.SetIdleAnimation();
            _stateMachine.SetState<CollectState>();
        }
    }
}