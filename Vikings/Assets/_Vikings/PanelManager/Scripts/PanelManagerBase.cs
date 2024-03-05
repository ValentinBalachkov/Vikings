using System;
using System.Collections.Generic;
using System.Linq;
using PanelManager.PanelAnimations;
using PanelManager.Scripts.Interfaces;
using PanelManager.Scripts.Panels;
using UnityEngine;

namespace PanelManager.Scripts
{
    public abstract class PanelManagerBase : MonoBehaviour, IPanelManager
    {
        #region Scene references

        [Header("Base")]
        [SerializeField] private PanelManagerSettings _settings;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _panelsParent;
        
        #endregion

        protected List<IView> Panels => _panels.Values.ToList();

        private readonly Dictionary<Type, IView>         _panels = new Dictionary<Type, IView>();
        private readonly Dictionary<Type, PanelModelBase> _models = new Dictionary<Type, PanelModelBase>();

        private readonly Stack<IView> _history = new Stack<IView>();

        private bool      _panelContainersCreated;
        private IView    _currentView;
        private Transform _screensParent;
        private Transform _overlaysParent;
        private bool _panelAnimationPlaying;

        #region IPanelManager

        public Canvas Canvas => _canvas;

        public void OpenPanel<T>() where T : ViewBase
        {
            if(_panelAnimationPlaying) return;
            
            var panel = GetPanel<T>();
            OpenPanel(panel, true);
        }

        public void OpenPanel(IView view)
        {
            var type = view.GetType();
            
            if (_panels.TryGetValue(type, out var panel) == false)
            {
                throw new ArgumentException($"Panel <{type}> not found!");
            }

            OpenPanel(panel, true);
        }

        public void OpenPanel<TP, TY>(TY arg) where TP : ViewBase, IAcceptArg<TY>
        {
            if(_panelAnimationPlaying) return;
            
            var panel = GetPanel<TP>();
            panel.AcceptArg(arg);
            OpenPanel(panel, true);
        }

        public void ClosePanel<T>() where T : ViewBase
        {
            if(_panelAnimationPlaying) return;
            
            var panel = GetPanel<T>();

            if (panel is ViewAnimationBase panelAnimationBase)
            {
                panelAnimationBase.AnimationClose(ClosePanelAnimationEnd);
                return;
            }
            
            panel.Close();
        }

        public T SudoGetPanel<T>() where T : ViewBase => GetPanel<T>();

        public void OpenPreviousPanel()
        {
            if (_history.Count == 0)
            {
                return;
            }

            var panel = _history.Pop();
            OpenPanel(panel, false);
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        #endregion

        #region Protected API

        protected virtual void DefineModels(Dictionary<Type, PanelModelBase> map) {}

        protected void CreatePanelsFromSettings()
        {
            DefineModels(_models);

            var orderedPanels = _settings.Panels.OrderBy(p => p.Order);
            
            foreach (var panelPrefab in orderedPanels)
            {
                if (panelPrefab is ViewWithModelBase panelWithModel)
                {
                    var model = _models[panelPrefab.GetType()];
                    CreatePanel(panelWithModel).Initialize(this, model);
                }
                else
                {
                    CreatePanel(panelPrefab).Initialize(this);
                }
            }
        }

        #endregion
  
        #region Private API

        private T CreatePanel<T>(T panelPrefab) where T : ViewBase
        {
            var type = panelPrefab.GetType();

            if (_panels.ContainsKey(type))
            {
                throw new ArgumentOutOfRangeException($"Panel with type {type} already exists");
            }

            var parent = GetPanelParent(panelPrefab);

            var panel = Instantiate(panelPrefab, parent);

            panel.gameObject.SetActive(false);

            _panels.Add(type, panel);

            return panel;
        }

        private void OpenPanel(IView view, bool canRemember)
        {
            if (view.PanelType == PanelType.Screen)
            {
                if (_currentView != null)
                {
                    if (canRemember && _currentView.RememberInHistory)
                    {
                        _history.Push(_currentView);
                    }

                    ClosePanel(_currentView, view);
                    return;
                }

                _currentView = view;
            }
            OpenNextPanel(view);
        }

        private void OpenNextPanel(IView view)
        {
            if (view is ViewAnimationBase panelAnimationBase)
            {
                _panelAnimationPlaying = true;
                panelAnimationBase.AnimationOpen(OpenPanelAnimationEnd);
                return;
            }
                
            view.Open();
        }

        private void ClosePanel(IView closeView, IView nextView)
        {
            _currentView = nextView;
            if (closeView is ViewAnimationBase panelAnimationBase)
            {
                _panelAnimationPlaying = true;
                panelAnimationBase.AnimationClose(ClosePanelAnimationEnd);
                return;
            }
                
            closeView.Close();
            OpenNextPanel(nextView);
        }
        
        private void ClosePanelAnimationEnd(ViewAnimationBase view)
        {
            view.Close();
            _panelAnimationPlaying = false;
            if(view.PanelType == PanelType.Overlay) return;
            OpenNextPanel(_currentView);
        }
        
        private void OpenPanelAnimationEnd(ViewAnimationBase view)
        {
            view.Open();
            _panelAnimationPlaying = false;
        }
        
        private T GetPanel<T>() where T : ViewBase
        {
            var type = typeof(T);

            if (_panels.TryGetValue(type, out var panel) == false)
            {
                throw new ArgumentException($"Panel <{type}> not found!");
            }

            return (T) panel;
        }

        private Transform GetPanelParent(ViewBase prefab)
        {
            if (_panelContainersCreated == false)
            {
                CreateContainers();

                _panelContainersCreated = true;
            }

            var parent = prefab.PanelType switch
            {
                PanelType.Screen  => _screensParent,
                PanelType.Overlay => _overlaysParent,
                _                 => throw new ArgumentOutOfRangeException()
            };

            return parent;
        }

        private void CreateContainers()
        {
            _screensParent = SetupContainer("Screens");
            _overlaysParent = SetupContainer("Overlays");

            Transform SetupContainer(string containerName)
            {
                var containerParent = _panelsParent.transform;
                var tr              = new GameObject(containerName, typeof(RectTransform)).transform;

                tr.SetParent(containerParent, false);
                tr.SetAsLastSibling();

                var rt = tr.GetComponent<RectTransform>();

                rt.anchorMin        = Vector2.zero;
                rt.anchorMax        = Vector2.one;
                rt.sizeDelta        = Vector2.zero;
                rt.anchoredPosition = Vector2.zero;

                return tr;
            }
        }

        #endregion
    }
}