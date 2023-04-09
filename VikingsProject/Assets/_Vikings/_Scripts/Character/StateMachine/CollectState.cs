using System.Threading.Tasks;
using Vikings.Inventory;
using Vikings.Items;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class CollectState : BaseState
    {
        private CharacterStateMachine _stateMachine;
        private WeaponController _weaponController;
        private ItemsOnMapController _itemsOnMapController;
        
        private ItemController _currentItem;

        public CollectState(CharacterStateMachine stateMachine, WeaponController weaponController, ItemsOnMapController itemsOnMapController) : base("Collect", stateMachine)
        {
            _stateMachine = stateMachine;
            _weaponController = weaponController;
            _itemsOnMapController = itemsOnMapController;
        }
        
        public override void Enter()
        {
            base.Enter();
            _currentItem = _itemsOnMapController.GetElementFromQueue();
            _currentItem.OnCollect += ChangeState;
            _currentItem.ActivateItem(_weaponController.GetCurrentCollectTime());
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();
        }

        private void ChangeState(ItemData itemData, int count)
        {
            _stateMachine.SetState<MovingState>();
            _currentItem.OnCollect -= ChangeState;
        }
    }
}