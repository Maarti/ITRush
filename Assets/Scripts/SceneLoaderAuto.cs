using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoaderAuto : MonoBehaviour {

    public string sceneName="MainMenu";
    public float waitingTime = 0f;

	// Update is called once per frame
	void Update () {
        StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(waitingTime);
        SceneManager.LoadScene(sceneName);
    }
}
