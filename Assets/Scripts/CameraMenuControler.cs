using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// necessaire pour gerer les element de l'UI

public class CameraMenuControler : MonoBehaviour
{

	public float minFoV = 1f, screenFov = 29f, maxFoV = 60f;
	public float sensitivity = 10f;
	public CanvasGroup uiTouchScreen, uiMainMenu, uiProjectScroller;
	public GameObject computer;

	// "float" 			= floating around;
	// "menustart" 		= zooming on screen;
	// "menuend" 		= zoomed on screen+menu;
	// "projectstart"	= zooming in blackscreen;
	// "projectend"		= zoomed in blackscreen;
	string state = "float";
	float cameraZInit;
	Coroutine fadeInUiTouchScreen, fadeInUiMainMenu, fadeInUiProjectScroller;

	    

	void Start ()
	{
		cameraZInit = Camera.main.transform.position.z;
		Camera.main.fieldOfView = maxFoV;

		HideUI (uiTouchScreen);
		HideUI (uiMainMenu);
		HideUI (uiProjectScroller);

		StartCoroutine (FloatingCoroutine ());
		fadeInUiTouchScreen = StartCoroutine (FadeInUICoroutine (uiTouchScreen));
	}

	void Update ()
	{
		if (state == "float" && ((Input.touchCount > 0) || (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButton ("Fire1")))) {
			state = "menustart";
			StopCoroutine (FloatingCoroutine ());
			StopCoroutine (fadeInUiTouchScreen);
			HideUI (uiTouchScreen);
			StartCoroutine (MoveCameraToStartCoroutine ());
			StartCoroutine (ZoomInCoroutine ());
		} else if (state == "menuend" && ((Input.touchCount > 0) || (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButton ("Fire1")))) {
			state = "projectstart";
			StopCoroutine (fadeInUiMainMenu);
			HideUI (uiMainMenu);
			StartCoroutine (ZoomInCoroutine ());
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

	IEnumerator ZoomInCoroutine ()
	{
		if (state == "menustart") {
			while (Camera.main.fieldOfView > screenFov) {
				Camera.main.transform.LookAt (computer.transform);
				Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView - 1, screenFov, maxFoV);
				yield return new WaitForSeconds (1 / sensitivity);
			}
			fadeInUiMainMenu = StartCoroutine (FadeInUICoroutine (uiMainMenu));
			state = "menuend";

		} else if (state == "projectstart") {			
			while (Camera.main.fieldOfView > minFoV) {
				Camera.main.transform.LookAt (computer.transform);
				Camera.main.fieldOfView = Mathf.Clamp (Camera.main.fieldOfView - 1, minFoV, maxFoV);
				yield return new WaitForSeconds (1 / sensitivity);
			}
			fadeInUiProjectScroller = StartCoroutine (FadeInUICoroutine (uiProjectScroller));
			state = "projectend";
		}
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
			Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Mathf.Clamp (Camera.main.transform.position.z + 0.1f * multiplier, min, max));
			yield return new WaitForSeconds (.05f);
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
		while (ui.alpha < 1f) {
			ui.alpha += .05f;
			yield return new WaitForSeconds (.1f);
		}
		ui.blocksRaycasts = true;
	}

		
}
