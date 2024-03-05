using System;
using PanelManager.Scripts.Interfaces;
using UnityEngine;

namespace PanelManager.PanelAnimations
{
    [Serializable]
    public class PanelAnimationParameters
    {
        [SerializeField] private PanelAnimationKey animationKey;
        [SerializeField] private AnimationCurve animationCurve;

        private IPanelNativeAnimation _panelNativeAnimation;

        private Transform _animationTarget;

        private Action _endAnimationCallback;


        public void Initialize(Transform animationTarget)
        {
            _animationTarget = animationTarget;
            CreatePanelAnimation();
        }

        public void StartAnimation(Action endAnimationCallback)
        {
            if (animationKey == PanelAnimationKey.None)
            {
                endAnimationCallback?.Invoke();
                return;
            }

            _endAnimationCallback = endAnimationCallback;
            _panelNativeAnimation.AnimationEnded += NativeAnimationEnded;
            _panelNativeAnimation.StartAnimation(_animationTarget, animationCurve);
        }

        public void StopAnimation()
        {
            _panelNativeAnimation.StopAnimation();
        }

        private void NativeAnimationEnded()
        {
            _panelNativeAnimation.AnimationEnded -= NativeAnimationEnded;
            _panelNativeAnimation.SetDefault();
            _endAnimationCallback?.Invoke();
        }

        private void CreatePanelAnimation()
        {
            _panelNativeAnimation = animationKey switch
            {
                PanelAnimationKey.None => null,
                PanelAnimationKey.Scaling => new ScalingPanelAnimation(),
                PanelAnimationKey.ScalingY => new ScalingYPanelAnimation(),
                _ => _panelNativeAnimation
            };
        }
    }
}