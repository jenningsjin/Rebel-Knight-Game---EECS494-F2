using UnityEngine;
using System.Collections;

public class FallingObstacle : MonoBehaviour {
    public GameObject drop;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == "Knight") {
            drop.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
