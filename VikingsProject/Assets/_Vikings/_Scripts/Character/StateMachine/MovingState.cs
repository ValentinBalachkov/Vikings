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
            _currentPoint = _buildingsOnMap.GetElementPosition(_playerPrefab.transform);
            _inventoryController.SetItem(_currentPoint);
            _playerPrefab.SetMoveAnimation();
            _playerPrefab.SetStoppingDistance(OFFSET_DISTANCE);
            _playerPrefab.SetActionOnGetPosition(OnGetPoint);
            _playerPrefab.MoveToPoint(_currentPoint.GetItemPosition());
        }

        private void OnGetPoint()
        {
            _stateMachine.SetState<CollectState>();
        }
    }
}