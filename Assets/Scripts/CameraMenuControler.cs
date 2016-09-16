using UnityEngine;
using System.Collections;
using UnityEngine.UI;   // necessaire pour gerer les element de l'UI

public class CameraMenuControler : MonoBehaviour {

    public float minFoV = 12f;
    public float maxFoV = 60f;
    public float sensitivity = 10f;
    public CanvasGroup scrollView;

    void Start()
    {
        scrollView.alpha = 0f;
        StartCoroutine(ZoomInCoroutine());
    }

    IEnumerator ZoomInCoroutine()
    {
        while (Camera.main.fieldOfView > minFoV)
        {
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView-1, minFoV, maxFoV);
            yield return new WaitForSeconds(1/sensitivity);
        }
        while (scrollView.alpha < 1f)
        {
            scrollView.alpha += .1f;
            yield return new WaitForSeconds(.1f);
        }            
    }
}
