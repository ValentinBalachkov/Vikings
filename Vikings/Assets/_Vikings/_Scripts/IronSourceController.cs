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
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

        StartCoroutine(RewardDelayCoroutine());
    }

    private void OnDestroy()
    {
        IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;
    }


    /************* RewardedVideo AdInfo Delegates *************/
// Indicates that there’s an available ad.
// The adInfo object includes information about the ad that was loaded successfully
// This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        
    }

// Indicates that no ads are available to be displayed
// This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    void RewardedVideoOnAdUnavailable()
    {
        
    }

// The Rewarded Video ad view has opened. Your activity will loose focus.
    void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        
    }

// The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        StartCoroutine(RewardDelayCoroutine());
    }

// The user completed to watch the video, and should be rewarded.
// The placement parameter will include the reward data.
// When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        
    }

// The rewarded video ad was failed to show.
    void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        StartCoroutine(RewardDelayCoroutine());
    }

// Invoked when the video ad was clicked.
// This callback is not supported by all networks, and we recommend using it only if
// it’s supported by all networks you included in your build.
    void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        
    }


    private IEnumerator RewardDelayCoroutine()
    {
        // yield return new WaitForSeconds(300f);
        yield return new WaitForSeconds(15f);

        while (!IronSource.Agent.isRewardedVideoAvailable())
        {
            yield return null;
        }

        IronSource.Agent.showRewardedVideo();
    }
}