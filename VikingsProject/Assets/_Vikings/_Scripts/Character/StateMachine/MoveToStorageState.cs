using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class MoveToStorageState : BaseState
    {
        private BuildingsOnMap _buildingsOnMap;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.5f;
        private CharacterStateMachine _stateMachine;
        private bool _isSetToStorage;
        
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
            _playerPrefab.OnEndAnimation += OnSetItemAnimation;
            _isSetToStorage = false;
        }

        public override void Exit()
        {
            _playerPrefab.OnEndAnimation -= OnSetItemAnimation;
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if(_isSetToStorage) return;
           
            _playerPrefab.MoveToPoint(_buildingsOnMap.GetCurrentBuildingPosition());
            if (Vector3.Distance(_playerPrefab.transform.position, _buildingsOnMap.GetCurrentBuildingPosition().position) >
                OFFSET_DISTANCE) return;
            _playerPrefab.SetCollectAnimation();
            _isSetToStorage = true;
        }

        private void OnSetItemAnimation()
        {
            _buildingsOnMap.SetItemToStorage(_stateMachine);
        }
    }
}