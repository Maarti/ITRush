using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

	public string sceneName = "Game";
	public int levelId;

	public void LoadScene ()
	{
		if (levelId != null && levelId != -1)
			ApplicationController.ac.currentLevel = ApplicationController.ac.levels [levelId];
		SceneManager.LoadScene (sceneName);
	}
}
