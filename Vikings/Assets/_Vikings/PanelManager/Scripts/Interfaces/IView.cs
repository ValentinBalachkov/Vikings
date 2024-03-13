using PanelManager.Scripts.Panels;
using UniRx;

namespace PanelManager.Scripts.Interfaces
{
    public interface IView
    {
        public PanelType PanelType { get; }
        int  Order             { get; }
        bool RememberInHistory { get; }
        
        ReactiveCommand ViewOpened { get; }
        ReactiveCommand ViewClosed { get; }

        void Open();
        void Close();
    }
}