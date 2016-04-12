using UnityEngine;
using System.Collections;

public class BowlingBallScript : MonoBehaviour {
	public Transform target;
	public Rigidbody rigid;
	public float zspeed;
	public float xspeed;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Knight").transform;
		rigid = gameObject.GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		float zdist = transform.position.z - target.position.z;
		Vector3 tmp = rigid.velocity;
		if (zdist >= 0.1f) {
			if (target.position.x > transform.position.x){
				tmp.x = xspeed;
			} else if (target.position.x < transform.position.x){
				tmp.x = -xspeed;	
			} else {
				tmp.x = 0f;
			}
		}
		tmp.z = -zspeed;
		rigid.velocity = tmp;

		transform.Rotate (15f, 2f, 2f, Space.World);
		if (MoveKnight.rigid.position.z - 3 > this.rigid.position.z) {
			Destroy(this.gameObject);
		}
	}
}