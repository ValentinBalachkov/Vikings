using System.Threading.Tasks;
using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class CraftingState : BaseState
    {
        private BuildingsOnMap _buildingsOnMap;
        private PlayerController _playerController;
        private const float OFFSET_DISTANCE = 1f;
        private bool _isCrafting;
        
        public CraftingState(StateMachine stateMachine, BuildingsOnMap buildingsOnMap, PlayerController playerController) : base("Crafting state", stateMachine)
        {
            _buildingsOnMap = buildingsOnMap;
            _playerController = playerController;
        }
        
        public override void Enter()
        {
            _isCrafting = false;
            _playerController.SetMoveAnimation();
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdatePhysics()
        {
            if(_isCrafting) return;
            _playerController.MoveToPoint(_buildingsOnMap.GetCurrentBuildingPosition());
            if (!(Vector3.Distance(_playerController.transform.position, _buildingsOnMap.GetCurrentBuildingPosition().position) <=
                  OFFSET_DISTANCE)) return;

            if (!_isCrafting && _buildingsOnMap.GetCurrentBuilding() is CraftingTableController)
            {
                CraftingTableController craftingTableController =
                    _buildingsOnMap.GetCurrentBuilding() as CraftingTableController;
                CraftingIndicatorView.Instance.Setup(craftingTableController.CraftingTableData.craftingTime,  _buildingsOnMap.GetCurrentBuilding().transform);
                StartTimerCraftingTable(craftingTableController);
            }

            if (!_isCrafting)
            {
                CraftingIndicatorView.Instance.Setup((int)_buildingsOnMap.GetCurrentBuilding().BuildingData.BuildTime, _buildingsOnMap.GetCurrentBuilding().transform);
                StartTimer();
            }
        }

        private async Task StartTimerCraftingTable(CraftingTableController craftingTableController)
        {
            _playerController.SetCraftingAnimation();
            _isCrafting = true;
            int time = craftingTableController.CraftingTableData.craftingTime * 1000;
            await Task.Delay(time);
            craftingTableController.OpenCurrentWeapon();
            _buildingsOnMap.ClearCurrentBuilding();
            _buildingsOnMap.UpdateCurrentBuilding();
        }

        private async Task StartTimer()
        {
            _playerController.SetCraftingAnimation();
            _isCrafting = true;
            int time = (int)_buildingsOnMap.GetCurrentBuilding().BuildingData.BuildTime * 1000;
            await Task.Delay(time);
            _buildingsOnMap.UpgradeBuildingToStorage();
        }

    }
}