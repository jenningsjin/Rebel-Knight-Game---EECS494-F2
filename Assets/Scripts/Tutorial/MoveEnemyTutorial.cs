using UnityEngine;
using System.Collections;

public class MoveEnemyTutorial : MonoBehaviour {

    //Rigidbody rigid;
    public GameObject explosion;
	GameObject player;
	Transform target;
	//float speed;
	public int state;
    float timer = 0.8f;
	//float fungustimer;
    float currentLane = 0f;
	//bool doingFungusCountdown = false;
	//public GameObject score;

    // Use this for initialization
    void Start()
    {
        //rigid = GetComponentInChildren<Rigidbody>();
		player = GameObject.Find ("Knight");
		target = player.transform;
		//maincamera = GameObject.Find ("Main Camera");
		state = 0;
		//speed = 7.0f;

        currentLane = MoveKnight.lanePosition();
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
			if (player.GetComponent<MoveKnight> ().grounded) {
				transform.LookAt (player.transform);
			}
			timer -= Time.deltaTime;

			Vector3 pos = transform.position;
			pos.x = currentLane;
			transform.position = Vector3.MoveTowards (transform.position, pos, 1f);
			if (timer < 0 && currentLane != MoveKnight.lanePosition ()) {
				currentLane = MoveKnight.lanePosition ();
				timer = 1f;
			}

			if (transform.position.z > target.position.z) {
				//Debug.Log ("CHANGING ENEMY's POSITION");
				transform.position = Vector3.MoveTowards (transform.position, target.position, Time.deltaTime * 1f);
			}
			// Fungus timer logic
			/*if (doingFungusCountdown) {
				fungustimer -= Time.deltaTime;
				if (fungustimer <= 0) {
					Debug.Log (Fungus.SayDialog.activeSayDialog.storyText.text);
					Fungus.SayDialog.activeSayDialog.Stop ();
				}
			}*/
			break;
		default:
			Debug.Log("MoveEnemy: Undefined state");
			break;
		}

    }
		
	void OnCollisionEnter(Collision col) {
        //print(col.gameObject.name);
		Debug.Log("COLLISION WITH ENEMY");
        if(col.gameObject.name == "Knight")
        {
            if (MoveKnight.lanceReady)
            {
				Debug.Log ("ENEMY HAS BEEN DEFEATED!!!");
				Fungus.Flowchart.BroadcastFungusMessage ("EnemyDead");
                Destroy(this.gameObject);
                //score.GetComponent<ScoreScript>().updateScore ();
                if (BoidController.flockSize < 10)
                {
                    BoidController.flockSize += 1;
                }
                //CarpetBossScript.bossHP -= 1;
				//fungustimer = 4.0f;
				//doingFungusCountdown = true;
            }
        }
		//print ("Player body collided with something");
		// Contact points: every contact stores a contact point and the two colliders
		// involved.
		/*foreach (ContactPoint c in col.contacts) {
			//print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
			if (c.thisCollider.name == "VulnerableArea" && c.otherCollider.name == "KnightLance" ||
				c.thisCollider.name == "KnightLance" && c.otherCollider.name == "VulnerableArea") {
				print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
				rigid.constraints = RigidbodyConstraints.None;
				rigid.AddExplosionForce (7.0f, this.gameObject.transform.position, 5.0f);
				// disable all child colliders
				Collider [] componentsList = this.gameObject.GetComponentsInChildren<Collider>();
				foreach (Collider collider in componentsList) {
					collider.enabled = false;
				}
				Destroy (this.gameObject);
				//score.GetComponent<ScoreScript>().updateScore ();
				if(BoidController.flockSize < 10) {
					BoidController.flockSize+=1;
				}
				CarpetBossScript.bossHP-=1;
			}
		}*/
	}
}
