using UnityEngine;
using System.Collections;
using UnityEngine.UI;   // necessaire pour gerer les element de l'UI

public class GenerateLevelButtons : MonoBehaviour {

    public Button buttonPrefab;

	// Use this for initialization
	void Start () {
	    for(int i = 2; i < 20; i++)
        {
            Button button = Instantiate(buttonPrefab, gameObject.transform) as Button;
            button.transform.SetParent(gameObject.transform, false);
            button.transform.localScale = Vector3.one;
            button.GetComponentInChildren<Text>().text = "Project " + i;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
