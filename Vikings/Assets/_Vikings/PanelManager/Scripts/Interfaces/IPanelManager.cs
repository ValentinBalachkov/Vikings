using PanelManager.Scripts.Panels;
using UniRx;
using UnityEngine;

namespace PanelManager.Scripts.Interfaces
{
    public interface IPanelManager
    {
        public CompositeDisposable Disposable { get;}
        public Canvas Canvas { get; }
        
        void OpenPanel<TM>() where TM : ViewBase;
        
        public void OpenPanel(IView view);

        public void ActiveOverlay(bool isActive);

        void OpenPanel<TM, TY>(TY arg) where TM : ViewBase, IAcceptArg<TY>;

        void ClosePanel<TM>() where TM : ViewBase;

        T SudoGetPanel<T>() where T : ViewBase;
        
        void OpenPreviousPanel();

        void ClearHistory();
    }
}