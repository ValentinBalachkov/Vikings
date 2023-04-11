using UnityEngine;
using Vikings.Building;
using Vikings.Inventory;
using Vikings.Items;

namespace Vikings.Chanacter
{
    public class IdleState : BaseState
    {
        private ItemsOnMapController _itemsOnMapController;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.5f;
        private CharacterStateMachine _stateMachine;
        private InventoryData _inventoryData;
        private StorageData _storageData;
        
        public IdleState(CharacterStateMachine stateMachine, ItemsOnMapController itemsOnMapController, PlayerController playerPrefab) : base("Idle state", stateMachine)
        {
            _stateMachine = stateMachine;
            _itemsOnMapController = itemsOnMapController;
            _playerPrefab = playerPrefab;
        }
        
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            _playerPrefab.MoveToPoint(_itemsOnMapController.BoneFire.transform);
            if (!(Vector3.Distance(_playerPrefab.transform.position, _itemsOnMapController.BoneFire.transform.position) <=
                  OFFSET_DISTANCE)) return;
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
    }
}