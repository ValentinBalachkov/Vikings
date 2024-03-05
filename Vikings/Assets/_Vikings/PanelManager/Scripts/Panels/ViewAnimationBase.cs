using System;
using PanelManager.Scripts.Panels;
using UnityEngine;

namespace PanelManager.PanelAnimations
{
    public class ViewAnimationBase : ViewBase
    {
        [SerializeField] private Transform animatedObject;
        [SerializeField] private PanelAnimationParameters openPanelAnimationParameters;
        [SerializeField] private PanelAnimationParameters closePanelAnimationParameters;
        public override PanelType PanelType { get; }
        public override bool RememberInHistory { get; }

        public bool PanelIsOpened => _panelIsOpened;

        private Action<ViewAnimationBase> _animationEnded;
        private bool _panelIsOpened;

        protected override void OnInitialize()
        {
            InitializeAnimation();
        }
        
        private void InitializeAnimation()
        {
            if (animatedObject == null) return;

            openPanelAnimationParameters.Initialize(animatedObject);
            closePanelAnimationParameters.Initialize(animatedObject);
        }

        protected override void OnOpened() {}

        protected override void OnClosed() {}

        protected override void OpeningProcess()
        {
            OnOpened();
            _panelIsOpened = true;
        }

        protected override void ClosingProcess()
        {
            gameObject.SetActive(false);
            OnClosed();
            _panelIsOpened = true;
        }

        #region IPanelAnimation

        public void AnimationOpen(Action<ViewAnimationBase> callback)
        {
            _panelIsOpened = false;
            _animationEnded = callback;
            gameObject.SetActive(true);
            openPanelAnimationParameters.StartAnimation(() => _animationEnded?.Invoke(this));
        }

        public void AnimationClose(Action<ViewAnimationBase> callback)
        {
            _panelIsOpened = false;
            _animationEnded = callback;
            closePanelAnimationParameters.StartAnimation(() => _animationEnded?.Invoke(this));
        }

        #endregion
    }
}