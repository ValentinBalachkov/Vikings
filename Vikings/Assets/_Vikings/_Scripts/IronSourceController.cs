using System.Collections;
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
        //Add AdInfo Rewarded Video Events

        IronSource.Agent.shouldTrackNetworkState(true);

        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += OnRewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent += AdClosedEvent;

        StartCoroutine(RewardDelayCoroutine());
    }

    private void OnDestroy()
    {
        IronSourceEvents.onRewardedVideoAvailabilityChangedEvent -= OnRewardedVideoAvailabilityChangedEvent;
        IronSourceEvents.onRewardedVideoAdClosedEvent -= AdClosedEvent;
    }

    private void OnRewardedVideoAvailabilityChangedEvent(bool isAvailable)
    {
        bool available = isAvailable;
    }

    private void AdClosedEvent()
    {
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.shouldTrackNetworkState(true);
    }


    private IEnumerator RewardDelayCoroutine()
    {
         yield return new WaitForSeconds(300f);
         IronSource.Agent.showRewardedVideo();
    }
}