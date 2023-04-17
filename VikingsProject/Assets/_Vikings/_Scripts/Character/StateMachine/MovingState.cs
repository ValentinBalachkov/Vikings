using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.Chanacter
{
    public class MovingState : BaseState
    {
        private StorageOnMap _storageOnMap;
        private CharacterStateMachine _stateMachine;

        private ItemController _currentPoint;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.3f;
        public MovingState(CharacterStateMachine stateMachine, StorageOnMap storageOnMap, PlayerController playerPrefab) : base("Moving",
            stateMachine)
        {
            _storageOnMap = storageOnMap;
            _playerPrefab = playerPrefab;
            _stateMachine = stateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            _currentPoint = _storageOnMap.GetElementPosition();
        }
        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            _playerPrefab.MoveToPoint(_currentPoint.transform);
            if (!(Vector3.Distance(_playerPrefab.transform.position, _currentPoint.transform.position) <=
                  OFFSET_DISTANCE)) return;
            _stateMachine.SetState<CollectState>();
        }
    }
}