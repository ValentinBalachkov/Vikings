﻿using UnityEngine;
using Vikings.Building;
using Vikings.Items;

namespace Vikings.Chanacter
{
    public class IdleState : BaseState
    {
        private BoneFireController _boneFireController;
        private PlayerController _playerPrefab;
        private const float OFFSET_DISTANCE = 0.5f;
        private CharacterStateMachine _stateMachine;
        private StorageData _storageData;
        private bool _isStopMove;
        
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
            _isStopMove = false;
            _playerPrefab.SetMoveAnimation();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
            if(_isStopMove) return;

            if (Vector3.Distance(_playerPrefab.transform.position,
                    _boneFireController.GetCurrentPosition().position) >
                OFFSET_DISTANCE)
            {
                _playerPrefab.MoveToPoint(_boneFireController.GetCurrentPosition());
            }
            else
            {
                _isStopMove = true;
                _playerPrefab.SetIdleAnimation();
            }
            
        }

    }
}