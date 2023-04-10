using UnityEngine;
using Vikings.Items;

namespace Vikings.Chanacter
{
    public class MoveToStorageState : BaseState
    {
        private ItemsOnMapController _itemsOnMapController;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.5f;
        private CharacterStateMachine _stateMachine;
        
        public MoveToStorageState(CharacterStateMachine stateMachine, ItemsOnMapController itemsOnMapController, PlayerController playerPrefab) : base("Move to storage", stateMachine)
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
            _playerPrefab.MoveToPoint(_itemsOnMapController.StorageController.transform);
            if (!(Vector3.Distance(_playerPrefab.transform.position, _itemsOnMapController.StorageController.transform.position) <=
                  OFFSET_DISTANCE)) return;
            _itemsOnMapController.StorageController.SetItemsOnStorage();
            _stateMachine.SetState<MovingState>();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }
    }
}