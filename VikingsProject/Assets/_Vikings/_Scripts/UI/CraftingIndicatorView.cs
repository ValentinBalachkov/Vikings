using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIndicatorView : MonoBehaviour
{
    [SerializeField] private Image _indicatorImage;
    private int _maxCount;

    public void Setup(int maxCount)
    {
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
