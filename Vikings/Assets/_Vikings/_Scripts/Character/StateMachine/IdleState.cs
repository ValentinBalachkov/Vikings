using Vikings.Building;

namespace Vikings.Chanacter
{
    public class IdleState : BaseState
    {
        private BoneFireController _boneFireController;
        private PlayerController _playerPrefab;
        private  float OFFSET_DISTANCE = 1f;
        private CharacterStateMachine _stateMachine;
        private StorageData _storageData;
        private bool _isStopMove;
        private BoneFirePositionData _currentPositionData;
        
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
            _currentPositionData = _boneFireController.GetCurrentPosition();
            _isStopMove = false;
            _playerPrefab.SetMoveAnimation();
            _playerPrefab.SetStoppingDistance(OFFSET_DISTANCE);
            _playerPrefab.SetActionOnGetPosition(OnGetPath);
            
            _playerPrefab.MoveToPoint(_currentPositionData.point);
        }

        public override void Exit()
        {
            _boneFireController.ResetPos(_currentPositionData.id);
            _playerPrefab.ResetIdleFlag();
            base.Exit();
        }
        
        

        private void OnGetPath()
        {
            _playerPrefab.ResetDestinationForLook(_boneFireController.BoneFire.transform);
            _playerPrefab.SetIdleAnimation();
        }
        

    }
}