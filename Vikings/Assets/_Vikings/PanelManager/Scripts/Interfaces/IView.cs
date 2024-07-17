using System.Collections.Generic;
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

        public List<TutorialSteps> GetTutorialSteps();

        void Open();
        void Close();
        
    }
}