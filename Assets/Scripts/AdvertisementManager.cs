using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;

public class AdvertisementManager : MonoBehaviour
{
	public string zoneId;
	public int rewardQty = 250;

	private bool isReady = false;

	void Start ()
	{
		// On initialise le nombre de Coins
		GameObject.Find ("Coin Text").GetComponentInChildren<Text> ().text = ApplicationController.ac.playerData.coins.ToString ();
	}

	void Update ()
	{
		// Lorsque la pub est prete, on change le libellé du bouton
		if (!isReady && Advertisement.IsReady (zoneId)) {
			isReady = true;
			gameObject.GetComponentInChildren<Text> ().text = "Show Ad";
		}
	}

	public void OnClickAd ()
	{
		// Au clic sur le bouton "Show Ad", on lance la pub puis déclenche le callbak HandleShowResult
		ShowOptions options = new ShowOptions ();
		options.resultCallback = HandleShowResult;
		Advertisement.Show (zoneId, options);
	}

	private void HandleShowResult (ShowResult result)
	{
		switch (result) {
		case ShowResult.Finished:	// Pub visionnee entierement
			Debug.Log ("Video completed. User rewarded " + rewardQty + " credits.");
			ApplicationController.ac.UpdateCoins (rewardQty);
			break;
		case ShowResult.Skipped:	// Pub skipped
			Debug.LogWarning ("Video was skipped.");
			break;
		case ShowResult.Failed:		// Echec de la pub
			Debug.LogError ("Video failed to show.");
			break;
		}
		gameObject.GetComponentInChildren<Text> ().text = "Wait...";
		isReady = false;
	}
}
