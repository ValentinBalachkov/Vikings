using Vikings.Building;

namespace Vikings.Chanacter
{
    public class CollectState : BaseState
    {
        private CharacterStateMachine _stateMachine;
        private BuildingsOnMap _buildingsOnMap;
        private InventoryController _inventoryController;
        private PlayerController _playerController;
        
        public CollectState(CharacterStateMachine stateMachine, PlayerController playerController,
            BuildingsOnMap buildingsOnMap, InventoryController inventoryController) : base("Collect", stateMachine)
        {
            _stateMachine = stateMachine;
            _buildingsOnMap = buildingsOnMap;
            _inventoryController = inventoryController;
            _playerController = playerController;
        }

        public override void Enter()
        {
            base.Enter();
            
            _playerController.SetCollectAnimation();

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