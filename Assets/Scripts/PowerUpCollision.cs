using UnityEngine;
using System.Collections;

public class PowerUpCollision : MonoBehaviour {

    public GameObject collisionAnimation;
    private GameController gameController;
    public int coffeeValue = 100;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        if (other.tag == "Boundary")
            return;
        else if (other.tag == "Player")
        {
            Instantiate(collisionAnimation, transform.position, transform.rotation);
            if (this.tag == "Coffee")
                gameController.AddCoffee(coffeeValue);
            audio.PlayOneShot(audio.clip);
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }
            Destroy(gameObject, audio.clip.length);
        }
        
    }
}
