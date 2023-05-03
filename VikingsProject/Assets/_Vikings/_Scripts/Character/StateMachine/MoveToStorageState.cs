using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class MoveToStorageState : BaseState
    {
        private BuildingsOnMap _buildingsOnMap;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 1f;
        private CharacterStateMachine _stateMachine;
        private bool _isCollect;
        
        public MoveToStorageState(CharacterStateMachine stateMachine, BuildingsOnMap buildingsOnMap, PlayerController playerPrefab) : 
            base("Move to storage", stateMachine)
        {
            _stateMachine = stateMachine;
            _buildingsOnMap = buildingsOnMap;
            _playerPrefab = playerPrefab;
        }

        public override void Enter()
        {
            base.Enter();
            _playerPrefab.SetMoveAnimation();
            _isCollect = false;
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            
            _playerPrefab.MoveToPoint(_buildingsOnMap.GetCurrentBuildingPosition());
            if ((Vector3.Distance(_playerPrefab.transform.position, _buildingsOnMap.GetCurrentBuildingPosition().position) >
                 OFFSET_DISTANCE) || _isCollect) return;
            _isCollect = true;
            _playerPrefab.SetCraftingAnimation();
            _buildingsOnMap.SetItemToStorage(_stateMachine);
        }
    }
}