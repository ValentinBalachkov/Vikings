using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class IdleState : BaseState
    {
        private BoneFireController _boneFireController;
        private PlayerController _playerPrefab;
        private  float OFFSET_DISTANCE = 2f;
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
            OFFSET_DISTANCE = Random.Range(2f, 4f);
            _playerPrefab.SetMoveAnimation();
            _playerPrefab.SetStoppingDistance(OFFSET_DISTANCE);
            _playerPrefab.SetActionOnGetPosition(OnGetPath);
            
            _playerPrefab.MoveToPoint(_boneFireController.GetCurrentPosition());
        }
        
        private void OnGetPath()
        {
            _playerPrefab.SetIdleAnimation();
        }
        

    }
}