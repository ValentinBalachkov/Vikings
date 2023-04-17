using Vikings.Building;
using Vikings.Items;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class CollectState : BaseState
    {
        private CharacterStateMachine _stateMachine;
        private WeaponController _weaponController;
        private StorageOnMap _storageOnMap;
        private InventoryController _inventoryController;

        private ItemController _currentItem;

        public CollectState(CharacterStateMachine stateMachine, WeaponController weaponController,
            StorageOnMap storageOnMap, InventoryController inventoryController) : base("Collect", stateMachine)
        {
            _stateMachine = stateMachine;
            _weaponController = weaponController;
            _storageOnMap = storageOnMap;
            _inventoryController = inventoryController;
        }

        public override void Enter()
        {
            base.Enter();
            _currentItem = _storageOnMap.GetElementFromQueue();
            _inventoryController.CollectItem(_currentItem, _weaponController.WeaponData);
            _inventoryController.OnCollect += ChangeState;
        }

        private void ChangeState()
        {
            _inventoryController.OnCollect -= ChangeState;
            _stateMachine.SetState<MoveToStorageState>();
        }
    }
}