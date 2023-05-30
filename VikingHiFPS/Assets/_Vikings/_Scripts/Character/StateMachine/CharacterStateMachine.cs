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
        public InventoryController InventoryController => _inventoryController;

        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private CharactersConfig _charactersConfig;
        private BuildingsOnMap _buildingsOnMap;


        private BoneFireController _boneFireController;

        private PlayerController _playerController;

        private MovingState _movingState;
        private CollectState _collectState;
        private MoveToStorageState _moveToStorageState;
        private IdleState _idleState;
        private CraftingState _craftingState;

        private List<BaseState> _states = new();
        private BaseState _currentState;

        public void SpawnCharacter(Transform position)
        {
            _playerController = Instantiate(_playerPrefab, position);
        }

        public void Init(BuildingsOnMap buildingsOnMap, BoneFireController boneFireController)
        {
            _buildingsOnMap = buildingsOnMap;
            _boneFireController = boneFireController;
            
            _movingState = new MovingState(this, _buildingsOnMap, _playerController, _inventoryController);
            _collectState = new CollectState(this,_playerController, _buildingsOnMap, _inventoryController);
            _moveToStorageState = new MoveToStorageState(this, _buildingsOnMap, _playerController);
            _idleState = new IdleState(this, _boneFireController, _playerController);
            _craftingState = new CraftingState(this, _buildingsOnMap, _playerController, _charactersConfig);
            _states.Add(_movingState);
            _states.Add(_collectState);
            _states.Add(_moveToStorageState);
            _states.Add(_idleState);
            _states.Add(_craftingState);

            SetState<IdleState>();
        }

        public void SetState<T>() where T : BaseState
        {
            _currentState = _states.FirstOrDefault(x => x.GetType() == typeof(T));
            ChangeState(_currentState);
        }
    }
}