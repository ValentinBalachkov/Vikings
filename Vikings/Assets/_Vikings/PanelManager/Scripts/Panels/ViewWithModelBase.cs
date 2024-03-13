using System;
using PanelManager.Scripts.Interfaces;

namespace PanelManager.Scripts.Panels
{
    public abstract class ViewWithModelBase : ViewBase
    {
        protected PanelModelBase Model;

        public virtual void Initialize(IPanelManager panelManager, PanelModelBase model)
        {
            Model = model;
            Model.Initialize(panelManager);
            Initialize(panelManager);
        }

        protected override void OpeningProcess()
        {
            gameObject.SetActive(true);
            Model.OnPanelOpened();
            OnOpened();
        }

        protected override void ClosingProcess()
        {
            gameObject.SetActive(false);
            Model.OnPanelClosed();
            OnClosed();
        }
    }

    public abstract class ViewWithModelBase<T> : ViewWithModelBase where T : PanelModelBase
    {
        protected new T Model => (T) base.Model;

        public override void Initialize(IPanelManager panelManager, PanelModelBase model)
        {
            if (model is T == false)
            {
                throw new ArgumentException($"Panel {GetType()} got model {model.GetType()}, expected {typeof(T)}");
            }

            base.Initialize(panelManager, model);
        }
    }
}