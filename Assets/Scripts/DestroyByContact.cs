using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{

	public GameObject explosion;
	private GameController gameController;
	public int scoreValue;
	private bool isGrabbed = false;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter (Collider other)
	{
		bool addScore = true;
		if (other.tag == "Boundary") {
			return;
		} else if (other.tag == "Magnet Bolt") {
			addScore = false;
			other.transform.parent = gameObject.transform;
			this.GetComponent<Mover> ().goingToPlayer = true;
			other.GetComponent<Mover> ().goingToPlayer = true;
			other.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			if (this.tag == "Bonus")
				isGrabbed = true;
		} else if (other.tag == "Destroy Bolt") {
			Instantiate (explosion, transform.position, transform.rotation);
			if (this.tag == "Bonus")
				scoreValue *= -1;
			Destroy (other.gameObject);
			Destroy (gameObject);
		} else if (other.tag == "Player") {
			Instantiate (explosion, transform.position, transform.rotation);
			if (this.tag == "Malus")
				scoreValue = scoreValue * -1;
			Destroy (gameObject);
		}
		if (addScore && scoreValue != 0) {
			if (isGrabbed && this.tag == "Bonus")
				scoreValue *= 2;
			gameController.AddScore (scoreValue);
			Debug.Log ("ADDSCORE : " + scoreValue);
		}
	}
}