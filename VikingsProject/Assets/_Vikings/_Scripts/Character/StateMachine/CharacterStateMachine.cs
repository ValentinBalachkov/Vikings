using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vikings.Items;
using Vikings.Weapon;

namespace Vikings.Chanacter
{
    public class CharacterStateMachine : StateMachine
    {
        [SerializeField] private ItemsOnMapController _itemsOnMapController;
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private WeaponController _weaponController;

        private MovingState _movingState;
        private CollectState _collectState;
        private MoveToStorageState _moveToStorageState;

        private List<BaseState> _states = new();

        private void Start()
        {
            _movingState = new MovingState(this, _itemsOnMapController, _playerPrefab);
            _collectState = new CollectState(this, _weaponController, _itemsOnMapController);
            _moveToStorageState = new MoveToStorageState(this, _itemsOnMapController, _playerPrefab);
            _states.Add(_movingState);
            _states.Add(_collectState);
            _states.Add(_moveToStorageState);
            SetState<MovingState>();
        }

        public void SetState<T>() where T : BaseState
        {
            var state = _states.FirstOrDefault(x => x.GetType() == typeof(T));
            ChangeState(state);
        }
    }
}