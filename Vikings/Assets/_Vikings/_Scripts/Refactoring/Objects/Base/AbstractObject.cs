using SecondChanceSystem.SaveSystem;
using UniRx;
using UnityEngine;

namespace Vikings.Object
{
    public abstract class AbstractObject : MonoBehaviour
    {
        public abstract Transform GetPosition();
        public abstract void CharacterAction();
        public abstract void Init();

        protected CompositeDisposable _disposable;
    }
}