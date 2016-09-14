using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

    private GameController gameController;

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

    // Détruit les objets qui quittent le boundary
    void OnTriggerExit(Collider other)
    {
        // si on laisse passer un malus, on perd la moitié de sa scoreValue
        if (other.tag == "Malus")
        {
            int scoreValue = other.GetComponent<DestroyByContact>().scoreValue / -2;
            gameController.AddScore(scoreValue);
        }
        Destroy(other.gameObject);
    }
}
