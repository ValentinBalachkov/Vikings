using System;
using System.Collections.Generic;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Chanacter;
using Vikings.Object;

namespace _Vikings.Refactoring.Character
{
    public class CharacterStateMachine : StateMachine
    {
        public bool IsIdle;

        public Action<CharacterStateMachine> OnCharacterAction;
        public int ActionCount => _characterManager.ActionCount;
        public Inventory Inventory => _inventory;

        private BaseCharacterState _currentState;

        private List<BaseCharacterState> _states = new();

        private DoActionState _doActionState;
        private DoMoveState _doMoveState;

        private PlayerController _playerController;

        private Inventory _inventory;

        private CharacterManager _characterManager;

        private Queue<ActionQueueData> _actionsQueue = new();


        public void Init(Transform position, PlayerController playerController, CharacterManager characterManager,
            WeaponFactory weaponFactory)
        {
            _inventory = new Inventory(weaponFactory);
            _characterManager = characterManager;

            _playerController = Instantiate(playerController, position);
            _playerController.Init(transform, _characterManager);

            _doActionState = new DoActionState("DoActionState", this);
            _doMoveState = new DoMoveState("DoMoveState", this, _playerController);

            _states.Add(_doActionState);
            _states.Add(_doMoveState);
        }

        public void SetCollectAnimation()
        {
            _playerController.SetCollectAnimation();
        }

        public void SetWorkAnimation()
        {
            _playerController.SetCraftingAnimation();
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


        public void SetObject(AbstractObject abstractObject, Action<CharacterStateMachine> endAction = null)
        {
            OnCharacterAction = endAction;
            _actionsQueue.Clear();
            AddActionToQueue(abstractObject, _doMoveState);
            AddActionToQueue(abstractObject, _doActionState);
            OnNextAction();
        }

        private void SetState(BaseCharacterState characterState, AbstractObject abstractObject)
        {
            _currentState = characterState;
            ChangeState(_currentState, abstractObject);
        }

        public void OnNextAction()
        {
            ActionQueueData actionData = new();

            if (_actionsQueue.TryDequeue(out actionData))
            {
                IsIdle = actionData.characterState is DoMoveState && actionData.abstractObject is BoneFire;

                if (!IsIdle)
                {
                    _playerController.ResetIdleFlag();
                }

                SetState(actionData.characterState, actionData.abstractObject);
                return;
            }

            OnCharacterAction?.Invoke(this);
        }

        public void SetBuildingToAction(AbstractObject setObject, AbstractObject getObject,
            Action<CharacterStateMachine> endAction, bool clearQueue = true)
        {
            if (clearQueue)
            {
                OnCharacterAction = endAction;
                _actionsQueue.Clear();
                Inventory.Clear();
            }

            AddActionToQueue(getObject, _doMoveState);
            AddActionToQueue(getObject, _doActionState);
            AddActionToQueue(setObject, _doMoveState);
            AddActionToQueue(setObject, _doActionState);

            if (clearQueue)
            {
                OnNextAction();
            }
        }

        private void AddActionToQueue(AbstractObject abstractObject, BaseCharacterState characterState)
        {
            ActionQueueData actionMoveFirst = new()
            {
                characterState = characterState,
                abstractObject = abstractObject
            };
            _actionsQueue.Enqueue(actionMoveFirst);
        }
    }

    [Serializable]
    public class ActionQueueData
    {
        public BaseCharacterState characterState;
        public AbstractObject abstractObject;
    }
}