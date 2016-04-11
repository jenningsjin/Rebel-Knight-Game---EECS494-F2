using UnityEngine;
using System.Collections;

public class FallingObstacle : MonoBehaviour {
    public GameObject drop;
    public Vector3 vel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == "Knight") {
            vel = Vector3.zero;
            vel.y = -12f;
            //drop.GetComponent<Rigidbody>().useGravity = true;
            drop.GetComponent<Rigidbody>().velocity = vel;
            drop.GetComponent<Rigidbody>().AddForce(0, -16, 0, ForceMode.Acceleration);
        }
    }
}
