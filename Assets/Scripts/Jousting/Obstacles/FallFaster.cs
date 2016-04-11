using UnityEngine;
using System.Collections;

public class FallFaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Ground") {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
