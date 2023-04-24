using UnityEngine;
using Vikings.Building;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class CollectState : BaseState
    {
        private CharacterStateMachine _stateMachine;
        private BuildingsOnMap _buildingsOnMap;
        private InventoryController _inventoryController;

        private IGetItem _currentItem;

        public CollectState(CharacterStateMachine stateMachine,
            BuildingsOnMap buildingsOnMap, InventoryController inventoryController) : base("Collect", stateMachine)
        {
            _stateMachine = stateMachine;
            _buildingsOnMap = buildingsOnMap;
            _inventoryController = inventoryController;
        }

        public override void Enter()
        {
            base.Enter();
         
            _currentItem = _buildingsOnMap.GetElementPosition();
          
            _inventoryController.OnCollect += ChangeState;
           
            _inventoryController.CollectItem();
        }

        private void ChangeState()
        {
          
            _inventoryController.OnCollect -= ChangeState;
            _stateMachine.SetState<MoveToStorageState>();
        }
    }
}