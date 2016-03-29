using UnityEngine;
using System.Collections;

public class Enemy3 : MonoBehaviour {
	Rigidbody rigid;
	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody>();
		//rigid.velocity.z = -20;
		rigid.AddForce (Vector3.back * 5);
	}
	
	// Update is called once per frame
	void Update () {
		if( rigid.velocity.z < 5) {
				rigid.AddForce (Vector3.back * 5);
		}
	}

	void OnCollisionEnter(Collision col) {        
				Destroy (this.gameObject);
	}
}
