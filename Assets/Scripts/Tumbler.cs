using UnityEngine;
using System.Collections;

public class Tumbler : MonoBehaviour
{
	public float speed = 30f;
	private bool goingUp = true;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		float y = transform.position.y;
		float newY = y;
		if (y >= 1.5f)
			goingUp = false;
		else if (y <= .5f)
			goingUp = true;
	
		if (goingUp)
			newY = y += 0.05f * speed * Time.deltaTime;
		else
			newY = y -= 0.05f * speed * Time.deltaTime;
		Debug.Log (newY);
		transform.position = new Vector3 (transform.position.x, newY, transform.position.z);
	}
}
