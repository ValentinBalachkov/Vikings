using DG.Tweening;
using UnityEngine;

namespace PanelManager.PanelAnimations
{
    public class ScalingYPanelAnimation : PanelNativeAnimationBase
    {

        private Vector3 _defaultScale;
        private Transform _animationTransform;

        
        
        public override void StartAnimation(Transform animationTransform, AnimationCurve curve)
        {
            _defaultScale = animationTransform.localScale;
            _animationTransform = animationTransform;
            
            const float startAnimationTime = 0f;
            var totalAnimationTime = curve.keys[curve.keys.Length - 1].time;
            
            ScaleChange(animationTransform, curve, startAnimationTime);
            
            _animationTween = DOTween.Sequence();

            _animationTween = DOTween.To(() => startAnimationTime, x => ScaleChange(animationTransform, curve, x),
                totalAnimationTime, totalAnimationTime).OnComplete(KillAnimation);

            _animationTween.Play();
        }

        public override void SetDefault()
        {
            _animationTransform.localScale = _defaultScale;
        }
        
        public override void StopAnimation()
        {
            KillAnimation();
        }

        private void ScaleChange(Transform target, AnimationCurve curve, float x)
        {
            target.localScale = new Vector3(
                curve.Evaluate(x),
                target.localScale.y,
                target.localScale.z);
        }
    }
}