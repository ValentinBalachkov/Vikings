using _Vikings.Refactoring.Character;
using UnityEngine;
using Vikings.Object;

namespace Vikings.Chanacter
{
    public class StateMachine : MonoBehaviour
    {
        private BaseState _currentStateOld;
        private BaseCharacterState _currentState;

        protected void ChangeStateOld(BaseState newState)
        {
            _currentStateOld?.Exit();
            _currentStateOld = newState;
            _currentStateOld.Enter();
        }
        
        protected void ChangeState(BaseCharacterState newState, AbstractObject abstractObject)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter(abstractObject);
        }
        
       
    }
}