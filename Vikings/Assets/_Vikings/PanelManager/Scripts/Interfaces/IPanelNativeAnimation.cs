using System;
using UnityEngine;

namespace PanelManager.Scripts.Interfaces
{
    public interface IPanelNativeAnimation
    {
        event Action AnimationEnded;
        void StartAnimation(Transform animationTransform, AnimationCurve curve);
        void SetDefault();
        void StopAnimation();
    }
}