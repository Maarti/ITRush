using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// necessaire pour gerer les element de l'UI

public class CameraMenuControler : MonoBehaviour
{

	public float minFoV = 1f, screenFov = 29f, maxFoV = 60f;
	public float sensitivity = 10f;
	public CanvasGroup uiTouchScreen, uiMainMenu, uiPlayMenu, uiHowToPlayMenu, uiShopMenu;
	public GameObject computer;

	// "float" 			= floating around;
	// "mainmenustart" 		= zooming on screen;
	// "mainmenuend" 		= zoomed on screen+menu;
	// "menustart"	= zooming in blackscreen;
	// "menuend"		= zoomed in blackscreen;
	string state = "float";
	float cameraZInit;
	Coroutine fadeInUiTouchScreen, fadeInUiMainMenu, fadeInUiProjectScroller;

	    

	void Start ()
	{
		cameraZInit = Camera.main.transform.position.z;
		Camera.main.fieldOfView = maxFoV;

		HideUI (uiTouchScreen);
		HideUI (uiMainMenu);
		HideUI (uiPlayMenu);
		HideUI (uiHowToPlayMenu);
		HideUI (uiShopMenu);

		StartCoroutine (FloatingCoroutine ());
		fadeInUiTouchScreen = StartCoroutine (FadeInUICoroutine (uiTouchScreen));
	}

	void Update ()
	{
		if (state == "float" && ((Input.touchCount > 0) || (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButton ("Fire1")))) {
			state = "mainmenustart";
			StopCoroutine (FloatingCoroutine ());
			StopCoroutine (fadeInUiTouchScreen);
			HideUI (uiTouchScreen);
			StartCoroutine (MoveCameraToStartCoroutine ());
			StartCoroutine (ZoomInCoroutine (uiMainMenu));
		}
	}

	IEnumerator FloatingCoroutine ()
	{
		bool toLeft = false;
		while (state == "float") {    // floating
			if (toLeft) {
				Camera.main.transform.LookAt (computer.transform);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 0.005f);
				yield return new WaitForSeconds (.025f);
			} else {
				Camera.main.transform.LookAt (computer.transform);
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - 0.005f);
				yield return new WaitForSeconds (.025f);
			}
			if (Camera.main.transform.position.z < -1)
				toLeft = true;
			else if (Camera.main.transform.position.z > 1)
				toLeft = false;
		}
	}

	IEnumerator ZoomInCoroutine (CanvasGroup uiToDisplay)
	{
		if (state == "mainmenustart") {
			while (Camera.main.fieldOfView > screenFov) {
				Camera.main.transform.LookAt (computer.transform);
				Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView - .5f, screenFov, maxFoV);
				yield return new WaitForSeconds (0.025f);
			}
			fadeInUiMainMenu = StartCoroutine (FadeInUICoroutine (uiToDisplay));
			state = "mainmenuend";

		} else if (state == "menustart") {			
			while (Camera.main.fieldOfView > minFoV) {
				Camera.main.transform.LookAt (computer.transform);
				Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView - 1f, minFoV, screenFov);
				yield return new WaitForSeconds (1 / sensitivity);
			}
			fadeInUiProjectScroller = StartCoroutine (FadeInUICoroutine (uiToDisplay));
			state = "menuend";
		}
	}

	IEnumerator ZoomOutCoroutine ()
	{
		while (Camera.main.fieldOfView < screenFov) {
			Camera.main.transform.LookAt (computer.transform);
			Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView + 1f, minFoV, screenFov);
			yield return new WaitForSeconds (1 / sensitivity);
		}
		fadeInUiMainMenu = StartCoroutine (FadeInUICoroutine (uiMainMenu));
		state = "mainmenuend";
	}

	IEnumerator MoveCameraToStartCoroutine ()
	{		
		// On replace la caméra à l'origine
		int multiplier;
		float min, max;
		if (Camera.main.transform.position.z - cameraZInit > 0) {
			multiplier = -1;
			min = cameraZInit;
			max = Camera.main.transform.position.z;
		} else {
			multiplier = 1;
			min = Camera.main.transform.position.z;
			max = cameraZInit;
		}				
		while (Camera.main.transform.position.z != cameraZInit) {
			Camera.main.transform.LookAt (computer.transform);
			Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Mathf.Clamp (Camera.main.transform.position.z + 0.02f * multiplier, min, max));
			yield return new WaitForSeconds (.025f);
		}
	}

	void HideUI (CanvasGroup ui)
	{
		ui.alpha = 0f;
		ui.blocksRaycasts = false;
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
		state = "mainmenustart";
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
		state = "menustart";
		StopCoroutine (fadeInUiMainMenu);
		HideUI (uiMainMenu);
		StartCoroutine (ZoomInCoroutine (uiPlayMenu));
	}

	public void OnClickHowToPlay ()
	{
		state = "menustart";
		StopCoroutine (fadeInUiMainMenu);
		HideUI (uiMainMenu);
		StartCoroutine (ZoomInCoroutine (uiHowToPlayMenu));
	}

	public void OnClickShop ()
	{
		state = "menustart";
		StopCoroutine (fadeInUiMainMenu);
		HideUI (uiMainMenu);
		StartCoroutine (ZoomInCoroutine (uiShopMenu));
	}

		
}
