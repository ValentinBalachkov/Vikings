using _Vikings.Refactoring.Character;
using UniRx;
using UnityEngine;

namespace Vikings.Object
{
    public abstract class AbstractObject : MonoBehaviour
    {
        public abstract Transform GetPosition();
        public abstract void CharacterAction(CharacterStateMachine characterStateMachine);
        public abstract void Init();

        protected CompositeDisposable _disposable = new();
    }

    public interface IAcceptArgs<in T1, in T2>
    {
        public void AcceptArg(T1 arg1, T2 arg2);
    }
}