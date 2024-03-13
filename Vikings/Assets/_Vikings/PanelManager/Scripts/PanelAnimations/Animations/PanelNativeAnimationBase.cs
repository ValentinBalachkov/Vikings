using System;
using DG.Tweening;
using PanelManager.Scripts.Interfaces;
using UnityEngine;

namespace PanelManager.PanelAnimations
{
    public class PanelNativeAnimationBase : IPanelNativeAnimation
    {
        public event Action AnimationEnded;
        protected Tween _animationTween;
        
        public virtual void StartAnimation(Transform animationTransform, AnimationCurve curve) {}
        public virtual void StopAnimation() {}
        public virtual void SetDefault() {}
        
        protected void KillAnimation()
        {
            if(_animationTween == null) return;
            _animationTween.Kill();
            _animationTween = null;
            AnimationEnded?.Invoke();
        }
    }
}