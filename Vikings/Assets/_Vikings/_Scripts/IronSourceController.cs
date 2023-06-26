using System;
using UnityEngine;

public class IronSourceController : MonoBehaviour
{
    public string appKey;
    private void Awake()
    {
        IronSource.Agent.init(appKey);
        
    }

    private void Start()
    {
        IronSource.Agent.showRewardedVideo();
    }
}
