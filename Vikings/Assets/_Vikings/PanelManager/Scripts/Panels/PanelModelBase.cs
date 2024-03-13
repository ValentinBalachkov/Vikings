using PanelManager.Scripts.Interfaces;

namespace PanelManager.Scripts.Panels
{
    public abstract class PanelModelBase
    {
        protected IPanelManager _panelManager;

        public void Initialize(IPanelManager panelManager)
        {
            _panelManager = panelManager;
            OnInitialize();
        }

        protected virtual void OnInitialize() {}

        public virtual void OnPanelOpened() {}
        public virtual void OnPanelClosed() {}
    }
}