using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class interstatialad : MonoBehaviour
{
    public TextMeshProUGUI logyazi;

        // These ad units are configured to always serve test ads.
    #if UNITY_ANDROID
        public string _adUnitId = "ca-app-pub-7863366837982797/6329203961";
#elif UNITY_IPHONE
      private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        private string _adUnitId = "ca-app-pub-7863366837982797/6329203961";
#endif

    private gamemanager mangersc;
    private InterstitialAd _interstitialAd;
    public bool adisready, birkere, loaded;

    float zmn, cooldown = 1f;

    public void Start()
    {
        mangersc = gamemanager.managersc;
        
        //ca-app-pub-7863366837982797~7047922094
        //ca-app-pub-7863366837982797/6329203961
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            //if (UnityEngine.Random.Range(0, 100) < 60)
            loaded = true;
            //else
                Debug.Log("not this time");
        });
    }

    private void LateUpdate()
    {
        if (!adisready && !birkere && loaded)
        {
            zmn += Time.deltaTime;

            if(zmn > cooldown)
            {
                LoadInterstitialAd();
                zmn = 0f;
            }
        }
    }

    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        logyazi.text = "Loading the interstitial ad.";
        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);

                    logyazi.text = "interstitial ad failed to load an ad" + error;
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());
                logyazi.text = "Interstitial ad loaded with response : "
                          + ad.GetResponseInfo();

                adisready = true;
                _interstitialAd = ad;
            });

        loaded = false;
        RegisterEventHandlers(_interstitialAd);
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            RegisterEventHandlers(_interstitialAd);
            _interstitialAd.Show();
            Debug.Log("Showing interstitial ad.");
            logyazi.text = "Showing interstitial ad.";
            adisready = false;
            birkere = true;
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            logyazi.text = "Interstitial ad is not ready yet.";
            adisready = false;
        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));

            adisready = false;
            birkere = true;
            mangersc.reslevelfunction();
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
            adisready = false;
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
            adisready = false;
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
            adisready = false;
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            logyazi.text = "Interstitial ad full screen content closed.";

            adisready = false;
            mangersc.reslevelfunction();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            logyazi.text = "Interstitial ad failed to open full screen content " +
                           "with error : " + error;

            adisready = false;
        };
    }
}
