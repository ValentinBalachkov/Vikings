using UnityEngine;
using UnityEngine.UI;

public class TutorialElement : MonoBehaviour
{
    public int ID => _id;
    public RectTransform RectTransform => _rectTransform;

    public Button ContinueButton => _continueButton;
    
    [SerializeField] private int _id;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Button _continueButton;
}