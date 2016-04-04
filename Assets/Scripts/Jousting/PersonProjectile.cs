using UnityEngine;
using System.Collections;

public class PersonProjectile : MonoBehaviour {
	private Rigidbody rigid;
	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody>();
		Vector3 vel = MoveKnight.rigid.velocity;
		vel.y = 10f;
		vel.z += 20f;
		rigid.velocity = vel;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
