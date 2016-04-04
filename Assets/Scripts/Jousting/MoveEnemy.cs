using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveEnemy : MonoBehaviour {

    Rigidbody rigid;
    public GameObject explosion;
	GameObject player;
	Transform target;
	float speed;
	public int state;
    float timer = 0.8f;
    float currentLane = 0f;
	//public GameObject score;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponentInChildren<Rigidbody>();
		player = GameObject.Find ("Knight");
		target = player.transform;
		//maincamera = GameObject.Find ("Main Camera");
		state = 0;
		speed = 7.0f;

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
                transform.position = Vector3.MoveTowards(transform.position, pos, 1f);
                if (timer < 0 && currentLane != MoveKnight.lanePosition())
                {
                    currentLane = MoveKnight.lanePosition();
                    timer = 1f;
                }

                if (transform.position.z > target.position.z) {
                    //Debug.Log ("CHANGING ENEMY's POSITION");
                    transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 1f);
			}
			break;
            case 2:
                //Dead
                timer -= Time.deltaTime;
                if(timer < 0)
                {
                    Destroy(this.gameObject);
                    /*Collider[] componentsList = this.gameObject.GetComponentsInChildren<Collider>();
                    foreach (Collider collider in componentsList)
                    {
                        collider.enabled = false;
                    }*/
                }
                break;
		default:
			Debug.Log("MoveEnemy: Undefined state");
			break;
		}

    }

    void OnCollisionEnter(Collision col) {
        //print(col.gameObject.name);
        if (col.gameObject.name == "Knight" && state == 1)
        {
            if (MoveKnight.lanceReady)
            {
                //Destroy(this.gameObject);
                state = 2;
                timer = 3f;
                rigid.constraints = RigidbodyConstraints.None;
                Vector3 vel = rigid.velocity;
                vel.z = MoveKnight.rigid.velocity.z + 12f;
                if(vel.z < 32)
                {
                    vel.z = 32f;
                }
                vel.y += 8f;
                rigid.velocity = vel;
                Vector3 angle = new Vector3(-1f, 2f, 0f);
                rigid.angularVelocity = angle;

                //score.GetComponent<ScoreScript>().updateScore ();
                if (BoidController.flockSize < 10)
                {
                    BoidController.flockSize += 1;
                }
                CarpetBossScript.bossHP -= 1;
				if (SceneManager.GetActiveScene ().name == "JoustTutorial") {
					Fungus.Flowchart.BroadcastFungusMessage ("EnemyDead");
				}
            }
        } else if (state == 2 && timer < 2f)
        {
            explosion.transform.position = this.transform.position;
            if(Random.Range(0, 10.0F) > 6f)
            {
                GameObject.Instantiate(explosion);

            }
            Destroy(this.gameObject);
        }
		//print ("Player body collided with something");
		// Contact points: every contact stores a contact point and the two colliders
		// involved.
	}
}
