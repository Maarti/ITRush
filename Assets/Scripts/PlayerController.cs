using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	public Boundary boundary;
	public GameObject destroyShot, magnetShot;
	public Transform shotSpawn;
	public float speed, fireRate, fireRateMin, fireRateMax;

	private float nextFire;
	private GameController gameController;


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

	void FixedUpdate ()
	{
		float moveHorizontal = 0f;
		Vector3 touchedScreenPosition;
		if (SystemInfo.deviceType == DeviceType.Desktop)
			touchedScreenPosition = Input.mousePosition;
		else
			touchedScreenPosition = Input.GetTouch (0).position;

		touchedScreenPosition.z = 20;
		Vector3 touchedWorldPosition = Camera.main.ScreenToWorldPoint (touchedScreenPosition);
		float offset = 0.15f;
		if (touchedWorldPosition.x > this.transform.position.x + offset) {
			//Debug.Log("Droite - Player = " + this.transform.position + "    Screen = " + touchedScreenPosition + "   World = " + touchedWorldPosition);
			moveHorizontal = speed;
		} else if (touchedWorldPosition.x < this.transform.position.x - offset) {
			//Debug.Log("Gauche - Player = " + this.transform.position + "    Screen = " + touchedScreenPosition + "   World = " + touchedWorldPosition);
			moveHorizontal = -speed;
		}

		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x + moveHorizontal, boundary.xMin, boundary.xMax),
			0.0f,
			0.0f
		);
	}

	public void fireDestroy ()
	{
		if (Time.time > nextFire) {
			UpdateFirerate ();
			GameObject bolt = Instantiate (destroyShot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			bolt.GetComponent<AudioSource> ().Play ();
			if (gameController.isOverdosed)
				bolt.GetComponent<Mover> ().randomDirection = true;
		}
	}

	public void fireMagnet ()
	{
		if (Time.time > nextFire) {
			UpdateFirerate ();
			GameObject bolt = Instantiate (magnetShot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			bolt.GetComponent<AudioSource> ().Play ();
			if (gameController.isOverdosed)
				bolt.GetComponent<Mover> ().randomDirection = true;
		}
	}

	void UpdateFirerate ()
	{
		float coffeeScore = (float)gameController.coffeeScore / 1000;
		float waitTime = fireRateMin - (coffeeScore * (fireRateMin - fireRateMax));
		nextFire = Time.time + waitTime;
		//Debug.Log("firerateWait = " + fireRateMin + " - ( " + gameController.coffeeScore + " / 1000 * ( " + fireRateMin + " - " + fireRateMax + ")) = " + waitTime);
	}
}
