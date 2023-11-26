using System.Collections;
using UnityEngine;
using Vikings.Chanacter;

public class IronSourceController : MonoBehaviour
{
    public string appKey;

    private const int TIMER_ON_START = 30;
    private const int TIMER = 180;
    private const int EFFECT_TIME = 10;
    private const float EFFECT = 2f;

    [SerializeField] private TrayView _trayView;
    [SerializeField] private CharactersConfig _charactersConfig;

    private bool _isRewardedLoaded;

    private void Awake()
    {
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);
        IronSource.Agent.shouldTrackNetworkState(true);

        //Add AdInfo Rewarded Video Events
        IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
        IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
        IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
        IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
        IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
        IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
        IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;

        _charactersConfig.speed_up = 1;
    }

    private void Start()
    {
        StartCoroutine(RewardDelayCoroutineOnStart(TIMER_ON_START));
    }

    public void ShowRewardVideo()
    {
        IronSource.Agent.showRewardedVideo();
    }


    private IEnumerator RewardDelayCoroutineOnStart(int time)
    {
        yield return new WaitForSeconds(time);
        if (_isRewardedLoaded)
        {
            _trayView.AddAdvertisementOnPanel();
        }
        else
        {
            StartCoroutine(RewardDelayCoroutineOnStart(time));
        }
    }

    private IEnumerator AddEffectCoroutine(int time)
    {
        _charactersConfig.speed_up = EFFECT;
        yield return new WaitForSeconds(time);
        _charactersConfig.speed_up = 1;
    }


/************* RewardedVideo AdInfo Delegates *************/
// Indicates that there’s an available ad.
// The adInfo object includes information about the ad that was loaded successfully
// This replaces the RewardedVideoAvailabilityChangedEvent(true) event
    private void RewardedVideoOnAdAvailable(IronSourceAdInfo adInfo)
    {
        _isRewardedLoaded = true;
    }

// Indicates that no ads are available to be displayed
// This replaces the RewardedVideoAvailabilityChangedEvent(false) event
    private void RewardedVideoOnAdUnavailable()
    {
        _isRewardedLoaded = false;
    }

// The Rewarded Video ad view has opened. Your activity will loose focus.
    private void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
    {
        StopAllCoroutines();
        _trayView.RemoveAdvertisementOnPanel();
        StartCoroutine(RewardDelayCoroutineOnStart(TIMER));
    }

// The Rewarded Video ad view is about to be closed. Your activity will regain its focus.
    private void RewardedVideoOnAdClosedEvent(IronSourceAdInfo adInfo)
    {
        
    }

// The user completed to watch the video, and should be rewarded.
// The placement parameter will include the reward data.
// When using server-to-server callbacks, you may ignore this event and wait for the ironSource server callback.
    private void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        StartCoroutine(AddEffectCoroutine(EFFECT_TIME));
    }

// The rewarded video ad was failed to show.
    private void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
    {
        StopAllCoroutines();
        StartCoroutine(RewardDelayCoroutineOnStart(TIMER));
    }

// Invoked when the video ad was clicked.
// This callback is not supported by all networks, and we recommend using it only if
// it’s supported by all networks you included in your build.
    private void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
    {
        
    }
}