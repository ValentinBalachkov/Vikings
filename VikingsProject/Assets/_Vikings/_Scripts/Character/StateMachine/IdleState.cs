using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class IdleState : BaseState
    {
        private BoneFireController _boneFireController;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.8f;
        private CharacterStateMachine _stateMachine;
        private StorageData _storageData;
        private bool _isStopMove;
        
        public IdleState(CharacterStateMachine stateMachine, BoneFireController boneFireController, PlayerController playerPrefab) : 
            base("Idle state", stateMachine)
        {
            _stateMachine = stateMachine;
            _boneFireController = boneFireController;
            _playerPrefab = playerPrefab;
        }
        
        public override void Enter()
        {
            base.Enter();
            _isStopMove = false;
            _playerPrefab.SetMoveAnimation();
            _playerPrefab.SetStoppingDistance(OFFSET_DISTANCE);
            _playerPrefab.SetActionOnGetPosition(OnGetPath);
            _playerPrefab.MoveToPoint(_boneFireController.GetCurrentPosition());
        }
        
        private void OnGetPath()
        {
            _playerPrefab.transform.LookAt(_boneFireController.GetCurrentPosition());
            _playerPrefab.SetIdleAnimation();
        }
        

    }
}