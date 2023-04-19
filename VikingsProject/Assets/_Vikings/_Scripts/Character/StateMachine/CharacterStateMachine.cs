using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Building;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class CharacterStateMachine : StateMachine
    {
        public BaseState CurrentState => _currentState;
        [SerializeField] private BuildingsOnMap _buildingsOnMap;
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private WeaponController _weaponController;
        [SerializeField] private BoneFireController _boneFireController;
        [SerializeField] private InventoryController _inventoryController;

        private MovingState _movingState;
        private CollectState _collectState;
        private MoveToStorageState _moveToStorageState;
        private IdleState _idleState;
        private CraftingState _craftingState;

        private List<BaseState> _states = new();
        private BaseState _currentState;


        private void Awake()
        {
            _movingState = new MovingState(this, _buildingsOnMap, _playerPrefab, _inventoryController);
            _collectState = new CollectState(this, _weaponController, _buildingsOnMap, _inventoryController);
            _moveToStorageState = new MoveToStorageState(this, _buildingsOnMap, _playerPrefab);
            _idleState = new IdleState(this, _boneFireController, _playerPrefab);
            _craftingState = new CraftingState(this, _buildingsOnMap, _playerPrefab);
            _states.Add(_movingState);
            _states.Add(_collectState);
            _states.Add(_moveToStorageState);
            _states.Add(_idleState);
            _states.Add(_craftingState);
        }

        public void SetState<T>() where T : BaseState
        {
            _currentState = _states.FirstOrDefault(x => x.GetType() == typeof(T));
            ChangeState(_currentState);
        }
    }
}