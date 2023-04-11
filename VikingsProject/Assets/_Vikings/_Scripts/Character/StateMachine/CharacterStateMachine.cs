using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Items;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class CharacterStateMachine : StateMachine
    {
        public BaseState CurrentState => _currentState;
        [SerializeField] private ItemsOnMapController _itemsOnMapController;
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private WeaponController _weaponController;

        private MovingState _movingState;
        private CollectState _collectState;
        private MoveToStorageState _moveToStorageState;
        private IdleState _idleState;

        private List<BaseState> _states = new();
        private BaseState _currentState;


        private void Start()
        {
            _movingState = new MovingState(this, _itemsOnMapController, _playerPrefab);
            _collectState = new CollectState(this, _weaponController, _itemsOnMapController);
            _moveToStorageState = new MoveToStorageState(this, _itemsOnMapController, _playerPrefab);
            _idleState = new IdleState(this, _itemsOnMapController, _playerPrefab);
            _states.Add(_movingState);
            _states.Add(_collectState);
            _states.Add(_moveToStorageState);
            _states.Add(_idleState);
            SetState<MovingState>();
        }

        public void SetState<T>() where T : BaseState
        {
            _currentState = _states.FirstOrDefault(x => x.GetType() == typeof(T));
            ChangeState(_currentState);
        }
    }
}