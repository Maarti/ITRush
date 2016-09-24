using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// necessaire pour gerer les element de l'UI

public class CameraMenuController : MonoBehaviour
{
	public CanvasGroup uiTouchScreen, uiMainMenu, uiPlayMenu, uiHowToPlayMenu, uiShopMenu;
	public AnimationClip zoomAnimation;

	Animator animator;
	bool zoomed = false;
	int zoomInHash = Animator.StringToHash ("ZoomIn");
	int zoomOutHash = Animator.StringToHash ("ZoomOut");
	Coroutine fadeInCoroutine;

	void Start ()
	{
		HideUI (uiTouchScreen);
		HideUI (uiMainMenu);
		HideUI (uiPlayMenu);
		HideUI (uiHowToPlayMenu);
		HideUI (uiShopMenu);
		fadeInCoroutine = StartCoroutine (FadeInUICoroutine (uiTouchScreen));
		animator = GetComponent<Animator> ();
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ().SetBool ("isWalking", true);

		// On n'affiche pas le logo immédiatement pour éviter le "bug graphique" lorsque le personnage passe devant
		GameObject.FindGameObjectWithTag ("Bonus").GetComponent<MeshRenderer> ().enabled = false;
		Invoke ("DisplayBrand", 3);
	}

	void Update ()
	{
		if (!zoomed && ((Input.touchCount > 0) || (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButton ("Fire1")))) {
			zoomed = true;
			StopCoroutine (fadeInCoroutine);
			HideUI (uiTouchScreen);
			StartCoroutine (ZoomInCoroutine (uiMainMenu));
		}
	}

	void HideUI (CanvasGroup ui)
	{
		ui.alpha = 0f;
		ui.blocksRaycasts = false;
	}

	IEnumerator ZoomInCoroutine (CanvasGroup uiToDisplay)
	{
		animator.SetTrigger (zoomInHash);
		yield return new WaitForSeconds (zoomAnimation.length);
		fadeInCoroutine = StartCoroutine (FadeInUICoroutine (uiToDisplay));
	}

	IEnumerator ZoomOutCoroutine ()
	{
		animator.SetTrigger (zoomOutHash);
		yield return new WaitForSeconds (zoomAnimation.length);
		fadeInCoroutine = StartCoroutine (FadeInUICoroutine (uiMainMenu));
	}

	IEnumerator FadeInUICoroutine (CanvasGroup ui)
	{
		// On fait apparaitre le menu
		ui.blocksRaycasts = true;
		while (ui.alpha < 1f) {
			ui.alpha += .05f;
			yield return new WaitForSeconds (.1f);
		}
	}

	public void backToMenu ()
	{
		StopAllCoroutines ();
		HideUI (uiTouchScreen);
		HideUI (uiMainMenu);
		HideUI (uiPlayMenu);
		HideUI (uiHowToPlayMenu);
		HideUI (uiShopMenu);
		StartCoroutine (ZoomOutCoroutine ());
	}

	public void OnClickPlay ()
	{
		StopCoroutine (fadeInCoroutine);
		HideUI (uiMainMenu);
		StartCoroutine (ZoomInCoroutine (uiPlayMenu));
	}

	public void OnClickHowToPlay ()
	{
		StopCoroutine (fadeInCoroutine);
		HideUI (uiMainMenu);
		StartCoroutine (ZoomInCoroutine (uiHowToPlayMenu));
	}

	public void OnClickShop ()
	{
		StopCoroutine (fadeInCoroutine);
		HideUI (uiMainMenu);
		StartCoroutine (ZoomInCoroutine (uiShopMenu));
	}

	void DisplayBrand ()
	{
		GameObject.FindGameObjectWithTag ("Bonus").GetComponent<MeshRenderer> ().enabled = true;
	}

		
}
