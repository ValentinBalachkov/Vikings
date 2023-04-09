using UnityEngine;

namespace Vikings.Chanacter
{
    public class StateMachine : MonoBehaviour
    {
        private BaseState _currentState;

        protected void ChangeState(BaseState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        private void Update()
        {
            _currentState?.UpdateLogic();
        }

        private void FixedUpdate()
        {
            _currentState?.UpdatePhysics();
        }
    }
}