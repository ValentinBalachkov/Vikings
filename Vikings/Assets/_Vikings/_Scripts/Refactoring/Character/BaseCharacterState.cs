using UnityEngine;
using Vikings.Chanacter;
using Vikings.Object;

namespace _Vikings.Refactoring.Character
{
    public class BaseCharacterState
    {
        protected AbstractObject _abstractObject;
        protected CharacterStateMachine stateMachine;
        private string _stateName;
        
        public BaseCharacterState(string stateName, CharacterStateMachine stateMachine)
        {
            _stateName = stateName;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter(AbstractObject abstractObject)
        {
            _abstractObject = abstractObject;
            Debug.Log("Enter state: " + _stateName);
        }

        public virtual void Exit()
        {
            _abstractObject = null;
            Debug.Log("Exit state: " + _stateName);
        }
    }
}