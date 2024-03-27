using System;
using _Vikings.Refactoring.Character;
using UniRx;
using UnityEngine;

namespace Vikings.Object
{
    public abstract class AbstractObject : MonoBehaviour
    {
        public Action EndAction;
        public abstract float GetStoppingDistance();
        public abstract Transform GetPosition();
        public abstract void CharacterAction(CharacterStateMachine characterStateMachine);
        public abstract void Init(MainPanelManager panelManager);

        protected CompositeDisposable _disposable = new();
    }

    public interface IAcceptArgs<in T1, in T2, in T3>
    {
        public void AcceptArg(T1 arg1, T2 arg2, T3 arg3);
    }
}