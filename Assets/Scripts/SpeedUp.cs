using UnityEngine;
using System.Collections;

public class SpeedUp : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(Vector3.up);
    }

    void OnTriggerEnter(Collider c)
    {
        print(c.gameObject.tag);
        if(c.gameObject.tag == "Knight")
        {
            MoveKnight.powerUp = true;
        }
    }
}
