using System.Threading.Tasks;
using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class CraftingState : BaseState
    {
        public bool isAlreadyCrafting;
        public bool isCrafting;

        
        private BuildingsOnMap _buildingsOnMap;
        private PlayerController _playerController;
        private const float OFFSET_DISTANCE = 1f;
        private CharactersConfig _charactersConfig;
        private CharacterStateMachine _characterStateMachine;
        private Transform _currentPos;
        
       public CraftingState(CharacterStateMachine stateMachine, BuildingsOnMap buildingsOnMap, PlayerController playerController, CharactersConfig charactersConfig) : base("Crafting state", stateMachine)
        {
            _buildingsOnMap = buildingsOnMap;
            _playerController = playerController;
            _charactersConfig = charactersConfig;
            _characterStateMachine = stateMachine;
             
        }
        
        public override void Enter()
        {
            isCrafting = false;
            _playerController.SetMoveAnimation();
            _playerController.SetStoppingDistance(OFFSET_DISTANCE);
            _playerController.SetActionOnGetPosition(OnGetPoint);
            _currentPos = _buildingsOnMap.GetCurrentBuildingPosition(_characterStateMachine);
            _playerController.MoveToPoint(_currentPos);
        }
        

        private void OnGetPoint()
        {
            if (isAlreadyCrafting)
            {
                _playerController.SetCraftingAnimation();
                return;
            }
            if (!isCrafting && _buildingsOnMap.GetCurrentBuilding(_characterStateMachine) is CraftingTableController)
            {
                CraftingTableController craftingTableController =
                    _buildingsOnMap.GetCurrentBuilding(_characterStateMachine) as CraftingTableController;
                
                var defaultTime = (int)craftingTableController.CraftingTableData.TableBuildingTime;
                int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
                CraftingIndicatorView.Instance.Setup(time,  _buildingsOnMap.GetCurrentBuilding(_characterStateMachine).transform);
                StartTimerCraftingTable(craftingTableController);
            }
            else if (!isCrafting && _buildingsOnMap.GetCurrentBuilding(_characterStateMachine).BuildingData.StorageData == null)
            {
                var defaultTime = _buildingsOnMap.GetCurrentBuilding(_characterStateMachine).BuildingData.craftingTableCrateTime;
                int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
                CraftingIndicatorView.Instance.Setup(time, _buildingsOnMap.GetCurrentBuilding(_characterStateMachine).transform);
                StartTimer(_buildingsOnMap.GetCurrentBuilding(_characterStateMachine).BuildingData.craftingTableCrateTime);
            }
            else if (!isCrafting)
            {
                var defaultTime = (int)_buildingsOnMap.GetCurrentBuilding(_characterStateMachine).BuildingData.StorageData.BuildTime;
                int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
                CraftingIndicatorView.Instance.Setup(time, _buildingsOnMap.GetCurrentBuilding(_characterStateMachine).transform);
                StartTimer((int)_buildingsOnMap.GetCurrentBuilding(_characterStateMachine).BuildingData.StorageData.BuildTime);
            }
        }

        private async Task StartTimerCraftingTable(CraftingTableController craftingTableController)
        {
            _buildingsOnMap.PlayBuildingParticle(_buildingsOnMap.GetCurrentBuilding(_characterStateMachine).BuildingData);
            _buildingsOnMap.PlayCraftingAudio();
            _playerController.SetCraftingAnimation();
            _buildingsOnMap.OffCraftingStateAllCharacters(true);
            var defaultTime = (int)craftingTableController.CraftingTableData.TableBuildingTime * 1000;
            int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
            await Task.Delay(time);
            craftingTableController.OpenCurrentWeapon();
            _buildingsOnMap.ClearCurrentBuilding();
            _buildingsOnMap.UpdateCurrentBuilding(_characterStateMachine);
            _buildingsOnMap.OffCraftingStateAllCharacters(false);
            _buildingsOnMap.StopAllParticles();
        }

        private async Task StartTimer(int craftingTime)
        {
            _buildingsOnMap.PlayBuildingParticle(_buildingsOnMap.GetCurrentBuilding(_characterStateMachine).BuildingData);
            _buildingsOnMap.PlayCraftingAudio();
            _playerController.SetCraftingAnimation();
            _buildingsOnMap.OffCraftingStateAllCharacters(true);
            var defaultTime = craftingTime * 1000;
            int time = (int)(defaultTime + (defaultTime * (_charactersConfig.SpeedWork / 100)));
            await Task.Delay(time);
            _buildingsOnMap.UpgradeBuildingToStorage(_characterStateMachine);
            _buildingsOnMap.OffCraftingStateAllCharacters(false);
            _buildingsOnMap.StopAllParticles();
        }

    }
}