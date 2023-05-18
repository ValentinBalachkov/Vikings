using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIndicatorView : MonoBehaviour
{
    public static CraftingIndicatorView Instance => _instance;
    private static CraftingIndicatorView _instance;
    [SerializeField] private Image _indicatorImage;
    [SerializeField] private RectTransform _rectTransform;
    
    private int _maxCount;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        gameObject.SetActive(false);
    }

    public void Setup(int maxCount, Transform pos)
    {
        _rectTransform.position = Camera.main.WorldToScreenPoint(new Vector3(pos.position.x, pos.position.y, pos.position.z - 1f));
        gameObject.SetActive(true);
        _indicatorImage.fillAmount = 0;
        _maxCount = maxCount;
        StartCoroutine(UpdateIndicatorCoroutine());
    }

    private IEnumerator UpdateIndicatorCoroutine()
    {
        int currentTime = 0;
        while (currentTime < _maxCount)
        {
            _indicatorImage.fillAmount = (float)currentTime / (float)_maxCount;
            currentTime++;
            yield return new WaitForSeconds(1f);
        }
        gameObject.SetActive(false);
    }
    
}
