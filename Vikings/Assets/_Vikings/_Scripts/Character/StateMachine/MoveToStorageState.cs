using Vikings.Building;

namespace Vikings.Chanacter
{
    public class MoveToStorageState : BaseState
    {
        private BuildingsOnMap _buildingsOnMap;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.5f;
        private CharacterStateMachine _stateMachine;

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
            _playerPrefab.SetStoppingDistance(OFFSET_DISTANCE);
            _playerPrefab.SetActionOnGetPosition(OnGetPoint);
            _playerPrefab.MoveToPoint(_buildingsOnMap.GetCurrentBuildingPosition(_stateMachine));
        }

        public override void Exit()
        {
            _playerPrefab.OnEndAnimation -= OnSetItemAnimation;
        }

        private void OnGetPoint()
        {
            _playerPrefab.SetCollectAnimation();
        }

        private void OnSetItemAnimation()
        {
            _buildingsOnMap.SetItemToStorage(_stateMachine);
        }
    }
}