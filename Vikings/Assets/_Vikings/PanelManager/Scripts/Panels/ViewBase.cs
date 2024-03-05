using System;
using PanelManager.Scripts.Interfaces;
using UniRx;
using UnityEngine;

namespace PanelManager.Scripts.Panels
{
    public abstract class ViewBase : MonoBehaviour, IView, IComparable<IView>
    {
        [SerializeField] private int _order = 0;
        
        
        public virtual bool IsOpen => gameObject.activeSelf;
        
        public abstract PanelType PanelType { get; }

        protected IPanelManager _panelManager;

        public void Initialize(IPanelManager panelManager)
        {
            ViewOpened = new ReactiveCommand();
            ViewClosed = new ReactiveCommand();
            
            _panelManager = panelManager;
            OnInitialize();
        }

        protected virtual void OnInitialize() {}

        protected virtual void OnOpened()
        {
            ViewOpened?.Execute();
        }

        protected virtual void OnClosed()
        {
            ViewClosed?.Execute();
        }

        protected virtual void OpeningProcess()
        {
            gameObject.SetActive(true);
            OnOpened();
        }

        protected virtual void ClosingProcess()
        {
            gameObject.SetActive(false);
            OnClosed();
        }

        #region IView

        public int Order => _order;

        public abstract bool RememberInHistory { get; }

        public ReactiveCommand ViewOpened { get; private set; }

        public ReactiveCommand ViewClosed { get; private set; }

        public void Open()
        {
            OpeningProcess();
        }

        public void Close()
        {
            ClosingProcess();
        }

        #endregion

        #region IComparable

        public int CompareTo(IView other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return _order.CompareTo(other.Order);
        }

        #endregion
    }
}