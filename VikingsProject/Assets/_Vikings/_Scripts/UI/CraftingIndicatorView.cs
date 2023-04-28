using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIndicatorView : MonoBehaviour
{
    [SerializeField] private Image _indicatorImage;
    private int _maxCount;

    public void Setup(int maxCount)
    {
        _indicatorImage.fillAmount = 0;
        _maxCount = maxCount;
        StartCoroutine(UpdateIndicatorCoroutine());
    }

    private IEnumerator UpdateIndicatorCoroutine()
    {
        int currentTime = 0;

        while (currentTime < _maxCount)
        {
            yield return new WaitForSeconds(1f);
            _indicatorImage.fillAmount = currentTime / _maxCount;
            currentTime++;
        }
    }
    
}
