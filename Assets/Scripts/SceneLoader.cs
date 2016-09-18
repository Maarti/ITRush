using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

	public string sceneName = "Game";
	public int levelId = 0;

	public void LoadScene ()
	{
		ApplicationController.ac.lvlPlaying = levelId;
		SceneManager.LoadScene (sceneName);
	}
}
