using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	Rigidbody rigid;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vel = rigid.velocity;
		vel.z = -10f;
		rigid.velocity = vel;
	}
}
