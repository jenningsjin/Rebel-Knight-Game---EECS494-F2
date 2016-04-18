using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class MoveEnemy : MonoBehaviour {

    Rigidbody rigid;
    public GameObject explosion;
	GameObject player;
	Transform target;
	float speed;
	public int state;
    float timer = 0.5f;
    float currentLane = 0f;
    public GameObject eyes;
	//public GameObject speedDial;
    public AudioClip death1;
    public AudioClip death2;
    AudioSource audio;
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
        eyes.SetActive(false);
        currentLane = MoveKnight.lanePosition();
		//speedDial = GameObject.Find ("Speed");
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		switch (state) {
		case 0:
			if (player.GetComponent<MoveKnight>().state == 1) {
				//////Debug.Log ("Chasing player");
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
			if (Mathf.Abs (transform.position.z - MoveKnight.rigid.position.z) > 4f) {
				transform.position = Vector3.MoveTowards (transform.position, pos, 1f);
			}
			if (timer < 0 && currentLane != MoveKnight.lanePosition ()) {
				currentLane = MoveKnight.lanePosition ();
				timer = 1f;
			}

			if (transform.position.z > target.position.z) {
				////////Debug.Log ("CHANGING ENEMY's POSITION");
				//transform.position = Vector3.MoveTowards (transform.position, target.position, Time.deltaTime * 1f);
			} else if (transform.position.z < target.position.z - 20f) {
				Destroy (this.gameObject);
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
			//////Debug.Log("MoveEnemy: Undefined state");
			break;
		}

    }

    void OnCollisionEnter(Collision col) {
        print(col.gameObject.name);
		Match nameContainsAllyProjectile = Regex.Match (col.gameObject.name, "AllyProjectile*");
		if (col.gameObject.name == "Knight" && state == 1) {
			if (MoveKnight.lanceReady) {
				FlamboyantDeathAnimation ();
                if (BoidController.flockSize < 4)
                {
                    BoidController.flockSize += 1;
                }
            }
		} else if (nameContainsAllyProjectile.Success && state == 1) {
			//////Debug.Log ("\nENEMY COLLIDED WITH AN ALLY\n");
			FlamboyantDeathAnimation ();
		}
        if (state == 2 && timer < 2f) {
            /*explosion.transform.position = this.transform.position;
            if(Random.Range(0, 10.0F) > 7f)
            {
                GameObject.Instantiate(explosion);

            }*/
            Destroy(this.gameObject);
        }
		//print ("Player body collided with something");
		// Contact points: every contact stores a contact point and the two colliders
		// involved.
	}

	// TODO: Sporadic bug where object is destroyed, then this is called. (4/4/16)
	void FlamboyantDeathAnimation() {
		//Destroy(this.gameObject);
		eyes.SetActive(true);
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
        float dir = (Random.Range(-2f, 2f));
		Vector3 angle = new Vector3(-1f, dir, 0f);
		rigid.angularVelocity = angle;

		//CarpetBossScript.bossHP -= 1;

		// Update the speed dial to reflect player's success
		//speedDial.GetComponent<SpeedScript>().increaseSpeedDial();
        float decide = Random.Range(0f, 4f);
        if(decide < 2f)
        {

        } else if(decide >= 2f && decide < 2.75f)
        {
            audio.PlayOneShot(death1, 0.4f);
            timer = 2.873f + 0.7f;
        } else if(decide >= 2.75f)
        {
            audio.PlayOneShot(death2, 1f);
            //
        }
		// In the tutorial, we want to inform Fungus that the first enemy is dead, so it
		// can display a dialogue telling the user about his minions. The problem is that
		// this object gets destroyed, so we can't use a timer to clear the dialogue after
		// N seconds. Therefore, I use an empty game object with an attached script.
		Debug.Log("col.gameObject.name == " + this.gameObject.name);
		if (SceneManager.GetActiveScene ().name == "JoustTutorial" &&
			this.gameObject.name == "SpecialEnemy") {
			GameObject enemyDeadObj = GameObject.Find ("EnemyKilledState");
			enemyDeadObj.GetComponent<EnemyKilledDialogue> ().TellFungusEnemyIsDead ();
		}
	}
}
