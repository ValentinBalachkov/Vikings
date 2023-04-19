using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.Chanacter
{
    public class MovingState : BaseState
    {
        private BuildingsOnMap _buildingsOnMap;
        private CharacterStateMachine _stateMachine;
        private InventoryController _inventoryController;

        private ItemController _currentPoint;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.3f;
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
        }
        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            _playerPrefab.MoveToPoint(_currentPoint.transform);
            if (!(Vector3.Distance(_playerPrefab.transform.position, _currentPoint.transform.position) <=
                  OFFSET_DISTANCE)) return;
            _stateMachine.SetState<CollectState>();
        }
    }
}