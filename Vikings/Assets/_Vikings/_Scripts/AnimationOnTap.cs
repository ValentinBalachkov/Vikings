using System;
using System.Collections;
using _Vikings._Scripts.Refactoring;
using UnityEngine;
using Vikings.Items;

public class AnimationOnTap : MonoBehaviour
{
   
    [SerializeField] private Resource _resource;

    private ItemData _currentItem;

    private void Start()
    {
        _currentItem = _resource.GetItemData();
    }


    private void OnMouseUp()
    {
        
    }

    private IEnumerator AnimationPlayCoroutine()
    {
        _currentItem.EffectOnTap.Play();
        yield return new WaitForSeconds(1f);
        _currentItem.EffectOnTap.Stop();
    }
}
