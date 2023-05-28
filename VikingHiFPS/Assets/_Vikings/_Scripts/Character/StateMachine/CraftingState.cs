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
            _playerController.SetStoppingDistance(OFFSET_DISTANCE);
            _playerController.SetActionOnGetPosition(OnGetPoint);
            _playerController.MoveToPoint(_buildingsOnMap.GetCurrentBuildingPosition());
        }

        private void OnGetPoint()
        {
            if (!_isCrafting && _buildingsOnMap.GetCurrentBuilding() is CraftingTableController)
            {
                CraftingTableController craftingTableController =
                    _buildingsOnMap.GetCurrentBuilding() as CraftingTableController;
                
                CraftingIndicatorView.Instance.Setup((int)craftingTableController.CraftingTableData.TableBuildingTime,  _buildingsOnMap.GetCurrentBuilding().transform);
                StartTimerCraftingTable(craftingTableController);
            }
            else if (!_isCrafting && _buildingsOnMap.GetCurrentBuilding().BuildingData.StorageData == null)
            {
                CraftingIndicatorView.Instance.Setup(_buildingsOnMap.GetCurrentBuilding().BuildingData.craftingTableCrateTime, _buildingsOnMap.GetCurrentBuilding().transform);
                StartTimer(_buildingsOnMap.GetCurrentBuilding().BuildingData.craftingTableCrateTime);
            }
            else if (!_isCrafting)
            {
                CraftingIndicatorView.Instance.Setup((int)_buildingsOnMap.GetCurrentBuilding().BuildingData.StorageData.BuildTime, _buildingsOnMap.GetCurrentBuilding().transform);
                StartTimer((int)_buildingsOnMap.GetCurrentBuilding().BuildingData.StorageData.BuildTime);
            }
        }

        private async Task StartTimerCraftingTable(CraftingTableController craftingTableController)
        {
            _playerController.SetCraftingAnimation();
            _isCrafting = true;
            int time = (int)craftingTableController.CraftingTableData.TableBuildingTime * 1000;
            await Task.Delay(time);
            craftingTableController.OpenCurrentWeapon();
            _buildingsOnMap.ClearCurrentBuilding();
            _buildingsOnMap.UpdateCurrentBuilding();
        }

        private async Task StartTimer(int craftingTime)
        {
            _playerController.SetCraftingAnimation();
            _isCrafting = true;
            int time = craftingTime * 1000;
            await Task.Delay(time);
            _buildingsOnMap.UpgradeBuildingToStorage();
        }

    }
}