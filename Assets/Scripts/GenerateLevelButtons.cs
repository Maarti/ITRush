using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// necessaire pour gerer les element de l'UI

public class GenerateLevelButtons : MonoBehaviour
{

	public Button buttonPrefab;

	// Use this for initialization
	void Start ()
	{
		float alpha;
		foreach (Level lvl in ApplicationController.ac.levels) {
			Button button = Instantiate (buttonPrefab, gameObject.transform) as Button;
			button.transform.SetParent (gameObject.transform, false);
			button.transform.localScale = Vector3.one;
			button.GetComponent<SceneLoader> ().levelId = lvl.id;

			// Init des text
			foreach (Text text in button.GetComponentsInChildren<Text>()) {
				switch (text.name) {
				case "Level Text":
					text.text = "Project " + (lvl.id + 1);
					break;
				case "Bronze Text":
					text.text = lvl.bronzeTime + " sec";
					break;
				case "Silver Text":
					text.text = lvl.silverTime + " sec";
					break;
				case "Gold Text":
					text.text = lvl.goldTime + " sec";
					break;
				}
			}

			// Init des images de medailles
			foreach (Image img in button.GetComponentsInChildren<Image>()) {
				Debug.Log (img.name);
				if (ApplicationController.ac.IsMedalObtained (lvl, img.name))
					alpha = 1f;
				else
					alpha = .1f;
				CanvasGroup cg = img.GetComponent<CanvasGroup> ();
				if (cg != null)
					cg.alpha = alpha;
			}
		}
	    

		/*for(int i = 2; i < 20; i++)
        {
            Button button = Instantiate(buttonPrefab, gameObject.transform) as Button;
            button.transform.SetParent(gameObject.transform, false);
            button.transform.localScale = Vector3.one;
            button.GetComponentInChildren<Text>().text = "Project " + i;
        }*/
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
