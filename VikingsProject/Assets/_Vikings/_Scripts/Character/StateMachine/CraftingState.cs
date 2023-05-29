using System.Threading.Tasks;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class CraftingState : BaseState
    {
        private BuildingsOnMap _buildingsOnMap;
        private PlayerController _playerController;
        private const float OFFSET_DISTANCE = 1f;
        private bool _isCrafting;
        private CharactersConfig _charactersConfig;
        
        public CraftingState(StateMachine stateMachine, BuildingsOnMap buildingsOnMap, PlayerController playerController, CharactersConfig charactersConfig) : base("Crafting state", stateMachine)
        {
            _buildingsOnMap = buildingsOnMap;
            _playerController = playerController;
            _charactersConfig = charactersConfig;
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
                
                var defaultTime = (int)craftingTableController.CraftingTableData.TableBuildingTime;
                int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
                
                CraftingIndicatorView.Instance.Setup(time,  _buildingsOnMap.GetCurrentBuilding().transform);
                StartTimerCraftingTable(craftingTableController);
            }
            else if (!_isCrafting && _buildingsOnMap.GetCurrentBuilding().BuildingData.StorageData == null)
            {
                var defaultTime = _buildingsOnMap.GetCurrentBuilding().BuildingData.craftingTableCrateTime;
                int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
                CraftingIndicatorView.Instance.Setup(time, _buildingsOnMap.GetCurrentBuilding().transform);
                StartTimer(_buildingsOnMap.GetCurrentBuilding().BuildingData.craftingTableCrateTime);
            }
            else if (!_isCrafting)
            {
                var defaultTime = (int)_buildingsOnMap.GetCurrentBuilding().BuildingData.StorageData.BuildTime;
                int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
                CraftingIndicatorView.Instance.Setup(time, _buildingsOnMap.GetCurrentBuilding().transform);
                StartTimer((int)_buildingsOnMap.GetCurrentBuilding().BuildingData.StorageData.BuildTime);
            }
        }

        private async Task StartTimerCraftingTable(CraftingTableController craftingTableController)
        {
            _playerController.SetCraftingAnimation();
            _isCrafting = true;
            var defaultTime = (int)craftingTableController.CraftingTableData.TableBuildingTime * 1000;
            int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
            await Task.Delay(time);
            craftingTableController.OpenCurrentWeapon();
            _buildingsOnMap.ClearCurrentBuilding();
            _buildingsOnMap.UpdateCurrentBuilding();
        }

        private async Task StartTimer(int craftingTime)
        {
            _playerController.SetCraftingAnimation();
            _isCrafting = true;
            var defaultTime = craftingTime * 1000;
            int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
            await Task.Delay(time);
            _buildingsOnMap.UpgradeBuildingToStorage();
        }

    }
}