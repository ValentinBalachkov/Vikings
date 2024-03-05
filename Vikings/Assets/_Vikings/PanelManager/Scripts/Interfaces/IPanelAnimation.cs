using System;

namespace PanelManager.Scripts.Interfaces
{
    public interface IPanelAnimation
    {
        bool IsPlayingAnimation { get; }
        
        void AnimationOpen(Action callback);

        void AnimationClose(Action callback);
    }
}