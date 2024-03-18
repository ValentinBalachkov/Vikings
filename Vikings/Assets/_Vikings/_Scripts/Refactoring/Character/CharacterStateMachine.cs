using System;
using System.Collections.Generic;
using System.Linq;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Object;

namespace _Vikings.Refactoring.Character
{
    public class CharacterStateMachine : StateMachine
    {
        public Action<CharacterStateMachine> OnCharacterAction;

        public int ActionCount => _characterManager.ActionCount;

        public Inventory Inventory => _inventory;

        public int Count => 1;
        
        public BaseCharacterState CurrentState => _currentState;
        private BaseCharacterState _currentState;
        
        private List<BaseCharacterState> _states = new();
        
        private DoActionState _doActionState;
        private DoMoveState _doMoveState;
        
        private PlayerController _playerController;

        private Inventory _inventory;

        private CharacterManager _characterManager;
        

        public void Init(Transform position, PlayerController playerController, CharacterManager characterManager, WeaponFactory weaponFactory)
        {
            _inventory = new Inventory(weaponFactory);
            _characterManager = characterManager;
            
            _playerController = Instantiate(playerController, position);
            _playerController.Init(transform, _characterManager);
            
            _doActionState = new DoActionState("DoActionState", this, OnCharacterAction);
            _doMoveState = new DoMoveState("DoMoveState", this, _playerController);
            
            _states.Add(_doActionState);
            _states.Add(_doMoveState);
        }

        public void SetCollectAnimation()
        {
            _playerController.SetCollectAnimation();
        }
        

        public void SetIdleAnimation()
        {
            _playerController.SetIdleAnimation();
        }

        public void ResetDestinationForLook(Transform newDestination)
        {
            _playerController.ResetDestinationForLook(newDestination);
        }
        
        public Transform GetPosition()
        {
            return transform;
        }
        
        
        public void SetState<T>(AbstractObject abstractObject) where T : BaseCharacterState
        {
            _currentState = _states.FirstOrDefault(x => x.GetType() == typeof(T));
            ChangeState(_currentState, abstractObject);
        }
        
        

        public void SetBuildingToAction(AbstractBuilding abstractBuilding, AbstractResource nearestResource, Action<CharacterStateMachine> endAction)
        {
            SetState<DoMoveState>(nearestResource);
            OnCharacterAction = o =>
            {
                SetState<DoMoveState>(abstractBuilding);
                OnCharacterAction = endAction;
            };
        }
    }
}