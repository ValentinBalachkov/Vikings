using PanelManager.Scripts.Panels;
using UnityEngine;

namespace PanelManager.Scripts.Interfaces
{
    public interface IPanelManager
    {
        public Canvas Canvas { get; }
        
        void OpenPanel<TM>() where TM : ViewBase;
        
        public void OpenPanel(IView view);

        void OpenPanel<TM, TY>(TY arg) where TM : ViewBase, IAcceptArg<TY>;

        void ClosePanel<TM>() where TM : ViewBase;

        T SudoGetPanel<T>() where T : ViewBase;
        
        void OpenPreviousPanel();

        void ClearHistory();
    }
}