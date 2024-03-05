using System;
using System.Collections.Generic;

namespace PanelManager.Scripts
{
    public class ReactiveProperty<T> : IDisposable
    {
        #region Action Wrap

        private class ActionWrap
        {
            public Action<T> Action;
            public object    Owner;
        }

        #endregion

        #region Properties

        public T Value { get; private set; }

        private readonly List<ActionWrap> _actions = new List<ActionWrap>(0);

        #endregion

        #region Constructions

        public ReactiveProperty(T value)
        {
            SetValue(value);
        }

        #endregion

        #region Public API

        public void SetValue(T value)
        {
            if (EqualityComparer<T>.Default.Equals(Value, value))
            {
                return;
            }

            Value = value;
            Notify();
        }

        public void Subscribe(object owner, Action<T> action)
        {
            _actions.Add(new ActionWrap {Action = action, Owner = owner});
            action?.Invoke(Value);
        }

        public void Unsubscribe(object owner, Action<T> action)
        {
            _actions.RemoveAll(wrap => wrap.Owner.Equals(owner) && wrap.Action.Equals(action));
        }

        public void Dispose()
        {
            _actions.Clear();
        }

        #endregion

        #region Private API

        private void Notify()
        {
            _actions.RemoveAll(action => action.Owner == null);
            foreach (var action in _actions)
            {
                action.Action?.Invoke(Value);
            }
        }

        #endregion

        #region Operators

        public static implicit operator T(ReactiveProperty<T> property)
        {
            return property.Value;
        }

        public static implicit operator ReactiveProperty<T>(T value)
        {
            return new ReactiveProperty<T>(value);
        }

        #endregion
    }
}