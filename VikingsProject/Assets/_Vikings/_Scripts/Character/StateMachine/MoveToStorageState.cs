using UnityEngine;
using Vikings.Building;

namespace Vikings.Chanacter
{
    public class MoveToStorageState : BaseState
    {
        private StorageOnMap _storageOnMap;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.5f;
        private CharacterStateMachine _stateMachine;
        
        public MoveToStorageState(CharacterStateMachine stateMachine, StorageOnMap storageOnMap, PlayerController playerPrefab) : 
            base("Move to storage", stateMachine)
        {
            _stateMachine = stateMachine;
            _storageOnMap = storageOnMap;
            _playerPrefab = playerPrefab;
        }
        
        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            
            _playerPrefab.MoveToPoint(_storageOnMap.GetCurrentStoragePosition());
            if (!(Vector3.Distance(_playerPrefab.transform.position, _storageOnMap.GetCurrentStoragePosition().position) <=
                  OFFSET_DISTANCE)) return;
            _storageOnMap.SetItemToStorage();
        }
    }
}