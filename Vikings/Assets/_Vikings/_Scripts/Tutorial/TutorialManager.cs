using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialConfig _tutorialConfig;
    [SerializeField] private GameObject _canvas;

    private List<TutorialElement> _tutorialElements = new();
    private TextureChanger _textureChanger;
    private Queue<TutorialSteps> _currentStepsQueue = new();
    private CompositeDisposable _disposable = new();

    [Inject]
    private void Init(TextureChanger textureChanger)
    {
        _textureChanger = textureChanger;
    }

    private void OnDestroy()
    {
        _disposable.Dispose();
    }

    public void LoadElements()
    {
        _tutorialElements = _canvas.GetComponentsInChildren<TutorialElement>().ToList();
        
        for (int i = _tutorialConfig._tutorialStepsList.Count - 1; i >= 0; i--)
        {
            if (_tutorialConfig._tutorialStepsList[i].IsDone)
            {
                continue;
            }
            
            _currentStepsQueue.Enqueue(_tutorialConfig._tutorialStepsList[i]);
        }
    }

    public void NextStep()
    {
        if (_currentStepsQueue.Count == 0)
        {
            DebugLogger.SendMessage("Tutorial is Done", Color.green);
            _textureChanger.DisableBackground();
            return;
        }
        
        var step = _currentStepsQueue.Dequeue();
        
        var element = _tutorialElements.FirstOrDefault(x => x.ID == step.ElementId);
        
        if (element == null)
        {
            DebugLogger.SendMessage("Tutorial element not found!", Color.red);
            return;
        }

        element.ContinueButton.OnClickAsObservable().Subscribe(_ =>
        {
            NextStep();
            _disposable.Dispose();
        }).AddTo(_disposable);
        
        _textureChanger.ChangeTexture(element.RectTransform);
    }
    
    
   

  
}