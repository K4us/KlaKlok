using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdmobScript : MonoBehaviour
{

		private BannerView bannerView;
		private InterstitialAd interstitial;

		// Use this for initialization
		void Start ()
		{
				Klaklok_earn.Instance.showAds += RequestInterstitial;		
				RequestBanner ();
		}
	
		private void RequestBanner ()
		{
				// Create a 320x50 banner at the top of the screen.
				bannerView = new BannerView ("Place-Banner-Id-Here", AdSize.Banner, AdPosition.Bottom);
				// Register for ad events.
				bannerView.AdLoaded += HandleAdLoaded;
				bannerView.AdFailedToLoad += HandleAdFailedToLoad;
				bannerView.AdOpened += HandleAdOpened;
				bannerView.AdClosing += HandleAdClosing;
				bannerView.AdClosed += HandleAdClosed;
				bannerView.AdLeftApplication += HandleAdLeftApplication;
				// Load a banner ad.
				bannerView.LoadAd (createAdRequest ());
		}
	
		void RequestInterstitial ()
		{
				// Create an interstitial.
				interstitial = new InterstitialAd ("Place-Interstitial-Id-Here");
				// Register for ad events.
				interstitial.AdLoaded += HandleInterstitialLoaded;
				interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
				interstitial.AdOpened += HandleInterstitialOpened;
				interstitial.AdClosing += HandleInterstitialClosing;
				interstitial.AdClosed += HandleInterstitialClosed;
				interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
				// Load an interstitial ad.
				interstitial.LoadAd (createAdRequest ());
		}
	
		// Returns an ad request with custom ad targeting.
		private AdRequest createAdRequest ()
		{
				return new AdRequest.Builder ()
				//                .AddTestDevice (AdRequest.TestDeviceSimulator)
				//                .AddTestDevice ("0123456789ABCDEF0123456789ABCDEF")
				//                .AddKeyword ("game")
				//                .SetGender (Gender.Male)
				//                .SetBirthday (new DateTime (1985, 1, 1))
				//                .TagForChildDirectedTreatment (false)
				//                .AddExtra ("color_bg", "9B30FF")
			.Build ();
		
		}
	
		private void ShowInterstitial ()
		{
				if (interstitial.IsLoaded ()) {
						interstitial.Show ();
				}
		}
	
	#region Banner callback handlers
	
		public void HandleAdLoaded (object sender, EventArgs args)
		{
				bannerView.Show ();
		}
	
		public void HandleAdFailedToLoad (object sender, AdFailedToLoadEventArgs args)
		{
				bannerView.LoadAd (createAdRequest ());
		}
	
		public void HandleAdOpened (object sender, EventArgs args)
		{
				bannerView.LoadAd (createAdRequest ());
		}
	
		void HandleAdClosing (object sender, EventArgs args)
		{
		}
	
		public void HandleAdClosed (object sender, EventArgs args)
		{
		}
	
		public void HandleAdLeftApplication (object sender, EventArgs args)
		{
				bannerView.Destroy ();
		}
	
	#endregion
	
	#region Interstitial callback handlers
	
		public void HandleInterstitialLoaded (object sender, EventArgs args)
		{
				if (interstitial.IsLoaded ()) {
						interstitial.Show ();
				}
		}
	
		public void HandleInterstitialFailedToLoad (object sender, AdFailedToLoadEventArgs args)
		{
				interstitial.LoadAd (createAdRequest ());
		}
	
		public void HandleInterstitialOpened (object sender, EventArgs args)
		{
		}
	
		void HandleInterstitialClosing (object sender, EventArgs args)
		{
		}
	
		public void HandleInterstitialClosed (object sender, EventArgs args)
		{
		}
	
		public void HandleInterstitialLeftApplication (object sender, EventArgs args)
		{
		}
	
	#endregion
}
