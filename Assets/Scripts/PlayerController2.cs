using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/*
[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}*/

public class PlayerController2 : MonoBehaviour
{

    Animator animator;
    public GameObject floorPlane;//in this demonstration this is set manually, the Retail Ability system has methods for dealing with this automatically via data structures for environments
    public float speed;
    public Boundary boundary;
    int WeaponState = 0;
    bool wasAttacking = false;// we need this so we can take lock the direction we are facing during attacks, mecanim sometimes moves past the target which would flip the character around wildly

    float rotateSpeed = 20.0f; //used to smooth out turning

    public Vector3 movementTargetPosition;
    public Vector3 attackPos;
    public Vector3 lookAtPos;

    RaycastHit hit;
    Ray ray;

    public bool rightButtonDown = false;//we use this to "skip out" of consecutive right mouse down...

    public GameObject destroyShot;
    public GameObject magnetShot;
    public Transform shotSpawn;
    public float fireRate, fireRateMin, fireRateMax;
    private float nextFire;

    public bool isMagnetShoot = false;
    private GameController gameController;

    public Text boltTypeText; // on le recupere pour jouer son AudioSource
    //private AudioSource switchBoltAudio;

    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();//need this...
        movementTargetPosition = transform.position;//initializing our movement target as our current position
        //switchBoltAudio = boltTypeText.GetComponent<AudioSource>();
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

    // Update is called once per frame
    void Update()
    {
        /*if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetButtonDown("Fire2"))
             switchBoltType();
         //Debug.Log("hotctrl = " + GUIUtility.hotControl);
         if (SystemInfo.deviceType == DeviceType.Desktop && GUIUtility.hotControl == 0 && Input.GetButton("Fire1"))        
             fire();*/

        // Vector2 touchedScreenPosition = Input.GetTouch(0).position; 
        Vector2 mouseScreenPosition = Input.mousePosition;
        // Debug.Log("mouse = " + mouseScreenPosition);
        Debug.DrawRay(new Vector3(this.transform.position.x, 1f, this.transform.position.y), Vector3.forward * 10);

    }

    void FixedUpdate()
    {

        float moveHorizontal = 0f;
        Vector3 touchedScreenPosition;
        if (SystemInfo.deviceType == DeviceType.Desktop)
            touchedScreenPosition = Input.mousePosition;
        else
            touchedScreenPosition = Input.GetTouch(0).position;

        touchedScreenPosition.z = 20;
        Vector3 touchedWorldPosition = Camera.main.ScreenToWorldPoint(touchedScreenPosition);
        float offset = 0.15f;
        if (touchedWorldPosition.x > this.transform.position.x + offset)
        {
            //Debug.Log("Droite - Player = " + this.transform.position + "    Screen = " + touchedScreenPosition + "   World = " + touchedWorldPosition);
            moveHorizontal = speed;
        }
        else if (touchedWorldPosition.x < this.transform.position.x - offset)
        {
            //Debug.Log("Gauche - Player = " + this.transform.position + "    Screen = " + touchedScreenPosition + "   World = " + touchedWorldPosition);
            moveHorizontal = -speed;
        }


        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x + moveHorizontal, boundary.xMin, boundary.xMax),
            0.0f,
            0.0f
        );

    }

    public void fireDestroy()
    {
        if (Time.time > nextFire)
        {
            //nextFire = Time.time + fireRate;
            UpdateFirerate();
            GameObject bolt = Instantiate(destroyShot, shotSpawn.position, shotSpawn.rotation) as GameObject;
            bolt.GetComponent<AudioSource>().Play();
            if (gameController.isOverdosed)
                bolt.GetComponent<Mover>().randomDirection = true;
            Debug.Log(destroyShot.tag + " fired");
        }
    }

    public void fireMagnet()
    {
        if (Time.time > nextFire)
        {
            //nextFire = Time.time + fireRate;
            UpdateFirerate();
            GameObject bolt = Instantiate(magnetShot, shotSpawn.position, shotSpawn.rotation) as GameObject;
            bolt.GetComponent<AudioSource>().Play();
            if (gameController.isOverdosed)
                bolt.GetComponent<Mover>().randomDirection = true;
            Debug.Log(magnetShot.tag + " fired");
        }
    }

    void UpdateFirerate()
    {
        float coffeeScore = (float)gameController.coffeeScore / 1000;
        float waitTime = fireRateMin - (coffeeScore * (fireRateMin - fireRateMax));
        nextFire = Time.time + waitTime;
        //Debug.Log("firerateWait = " + fireRateMin + " - ( " + gameController.coffeeScore + " / 1000 * ( " + fireRateMin + " - " + fireRateMax + ")) = " + waitTime);
    }
}
