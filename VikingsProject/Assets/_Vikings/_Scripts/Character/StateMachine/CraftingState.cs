using System.Threading.Tasks;
using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class CraftingState : BaseState
    {
        private BuildingsOnMap _buildingsOnMap;
        private PlayerController _playerController;
        private const float OFFSET_DISTANCE = 0.5f;
        private bool _isCrafting;
        
        public CraftingState(StateMachine stateMachine, BuildingsOnMap buildingsOnMap, PlayerController playerController) : base("Crafting state", stateMachine)
        {
            _buildingsOnMap = buildingsOnMap;
            _playerController = playerController;
        }
        
        public override void Enter()
        {
            _isCrafting = false;
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
                StartTimerCraftingTable(_buildingsOnMap.GetCurrentBuilding() as CraftingTableController);
            }

            if (!_isCrafting)
            {
                StartTimer();
            }
        }

        private async Task StartTimerCraftingTable(CraftingTableController craftingTableController)
        {
            _isCrafting = true;
            int time = craftingTableController.CraftingTableData.craftingTime * 1000;
            await Task.Delay(time);
            craftingTableController.OpenCurrentWeapon();
            _buildingsOnMap.ClearCurrentBuilding();
            _buildingsOnMap.UpdateCurrentBuilding();
        }

        private async Task StartTimer()
        {
            _isCrafting = true;
            int time = (int)_buildingsOnMap.GetCurrentBuilding().BuildingData.BuildTime * 1000;
            await Task.Delay(time);
            _buildingsOnMap.UpgradeBuildingToStorage();
        }

    }
}