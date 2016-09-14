using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    public bool goingToPlayer = false, randomDirection = false;
    private GameObject player;
    public float magnetSpeed;
    public Vector3 direction = Vector3.forward;
    private float randomAngle = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (randomDirection)
            rb.velocity = new Vector3(Random.Range(-randomAngle, randomAngle), 0, 1) * speed;
        else
            rb.velocity = direction * speed;
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.Log("Cannot find 'Player' tag");
        }
    }

    void Update()
    {
        // Aller en direction du joueur
        if (goingToPlayer)
        {
            //rb.velocity = transform.forward;
            // Vitesse de déplacement
            float step = magnetSpeed * Time.deltaTime;
            // Le joueur est à y=0 donc je ne pointe pas directement vers le joueur (sinon le objets traversent le sol)
            // Je créer au préalable une "target" aux coordonnées du joueur mais avec un y=1
            Vector3 target = new Vector3(player.transform.position.x, 1f, player.transform.position.z);
            // Les objets regardent vers le joueur
            transform.LookAt(player.transform);
            // Les objets vont vers le joueur
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
    }


}