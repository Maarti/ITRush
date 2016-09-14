using UnityEngine;
using System.Collections;

public class RandomDecorPosition : MonoBehaviour {

    public Vector3 rotateVector = new Vector3(0, 0, 180);

    // Use this for initialization
    void Start () {
        if (Random.value >= 0.5f)
        {
            this.transform.position = new Vector3(this.transform.position.x * -1, this.transform.position.y, this.transform.position.z);
            //decor.transform.Translate(14, 0, 0);
            this.transform.Rotate(rotateVector);
        }
    }

}
