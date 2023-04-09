using UnityEngine;

namespace Vikings.Chanacter
{
    public class BaseState
    {
        protected StateMachine stateMachine;
        private string _stateName;
        
        public BaseState(string stateName, StateMachine stateMachine)
        {
            _stateName = stateName;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter()
        {
            Debug.Log("Enter state: " + _stateName);
        }
        public virtual void Exit(){}
        public virtual void UpdatePhysics(){}
        public virtual void UpdateLogic(){}
    }  
}

