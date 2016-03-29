using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    Rigidbody rigid;
    public GameObject explosion;
	GameObject player;
	Transform target;
	float speed;
	//GameObject maincamera; // for player move script -- WHY???
	public int state;
	public GameObject score;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponentInChildren<Rigidbody>();
		player = GameObject.Find ("Knight");
		target = player.transform;
		//maincamera = GameObject.Find ("Main Camera");
		state = 0;
		speed = 15.0f;
		score = GameObject.Find ("Score");
    }

    // Update is called once per frame
    void Update()
    {
		switch (state) {
		case 0:
			if (player.GetComponent<MoveKnight>().state == 1) {
				Debug.Log ("Chasing player");
				++state;
			}
			break;
		case 1:
			// MoveTowards: Each frame, a line is drawn from the enemy's current position to the player's current position,
			// and the enemy moves a certain distance along this line, given by the third argument.
			// If the frame rate is very fast, then Time.deltaTime is small, and the enemy doesn't travel far
			// in a given frame.
			// On the other hand, if the frame rate is very slow, then Time.deltaTime is huge, and the enemy travels far
			// in a given frame.
			if (transform.position.z > target.position.z) {
				Debug.Log ("CHANGING ENEMY's POSITION");
				transform.position = Vector3.MoveTowards (transform.position, target.position, Time.deltaTime * speed);
				transform.LookAt (player.transform);
			}
			break;
		default:
			Debug.Log("MoveEnemy: Undefined state");
			break;
		}

    }
		
	void OnCollisionEnter(Collision col) {
		//print ("Player body collided with something");
		// Contact points: every contact stores a contact point and the two colliders
		// involved.
		foreach (ContactPoint c in col.contacts) {
			//print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
			if (c.thisCollider.name == "VulnerableArea" && c.otherCollider.name == "KnightLance" ||
				c.thisCollider.name == "KnightLance" && c.otherCollider.name == "VulnerableArea") {
				print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
				Destroy (this.gameObject);
				score.GetComponent<ScoreScript>().updateScore ();
				BoidController.flockSize+=1;
			}
		}
	}
}
