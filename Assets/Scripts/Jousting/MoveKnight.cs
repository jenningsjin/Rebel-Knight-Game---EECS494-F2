using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveKnight : MonoBehaviour {
    public static Rigidbody rigid;
    public GameObject explosion;
	public int state;
	public GameObject hp_bar;
	public int jumpSpeed = 100;
	public BoxCollider groundCollider;
    private float moveTimer = 0.1f;
    public static int lane = 1;
    public static float leftLane = -4f;
    public static float rightLane = 4f;
    public static float midLane = 0f;
    private float maxSpeed = 20f;
	bool sentMsg = false;
	bool grounded = true;
    public static bool lanceReady = false;
    float lanceTimer = 1f;
    public GameObject person;
    public ParticleSystem particle;
    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
		state = 0;
		hp_bar = GameObject.Find ("HP_Bar");
        lane = 1;
        switchLanes();
        particle = GetComponent<ParticleSystem>();
        particle.enableEmission = false;
    }

	public void BeginGame() {
		++state;
	}

	// Update is called once per frame
	void Update () {
        moveTimer -= Time.deltaTime;
        if (lanceReady)
        {
            particle.enableEmission = true;
            lanceTimer -= Time.deltaTime;
            if(lanceTimer < 0)
            {
                particle.enableEmission = false;
                lanceTimer = 1f;
                lanceReady = false;
            }
        }
        switch (state) {
		case 0: // Before game start
			break;
		case 1: // Charge
            changeSpeed();
			if( rigid.velocity.z < maxSpeed) {
				rigid.AddForce (Vector3.forward * 40f);
			}
                //Vector3 tmp = rigid.rotation.eulerAngles;
                switchLanes();
			if (Input.GetKeyDown (KeyCode.LeftArrow) && moveTimer < 0 && lane > 0) {
                    lane--;
                    switchLanes();
                    moveTimer = 0.1f;
                    //tmp.y -= 0.5f;
                }
                else if (Input.GetKeyDown (KeyCode.RightArrow) && moveTimer < 0 && lane < 2) {
                    lane++;
                    switchLanes();
                    moveTimer = 0.1f;
                    //tmp.y += 0.5f;
                }
                else if (Input.GetKeyDown (KeyCode.UpArrow) && grounded) {
				rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
				Debug.Log("Jumping");
				grounded = false;
			}
                else if (Input.GetKeyDown(KeyCode.DownArrow) && BoidController.flockSize > 0)
                {
                    Vector3 personPos = this.transform.position;
                    personPos.z += 0.5f;
                    person.transform.position = personPos;
                    GameObject.Instantiate(person);
                    BoidController.flockSize--;
                } else if(Input.GetKeyDown(KeyCode.Space) && lanceTimer > 0 && Weapons.index == 0)
                {
                    lanceReady = true;
                }
			//rigid.rotation = Quaternion.Euler (tmp.x, tmp.y, tmp.z);
			break;
		case 2: // After crossing the finish line
			rigid.constraints = RigidbodyConstraints.FreezeAll;
			if (!sentMsg) {
				Fungus.Flowchart.BroadcastFungusMessage ("LevelCleared");
				sentMsg = true;
			}
			break;
		default:
			Debug.Log ("Unrecognized state");
			break;
		}
    }

    void switchLanes()
    {
        Vector3 pos = this.transform.position;
        switch (lane)
        {
            case 0: pos.x = leftLane; break;
            case 1: pos.x = midLane; break;
            case 2: pos.x = rightLane; break;
            default: pos.x = midLane; lane = 0; break;
        }
        
        //rigid.MovePosition(pos);
        rigid.position = Vector3.MoveTowards(transform.position, pos, 1.5f);
    }

	void OnCollisionEnter(Collision col) {
		//print ("Player body collided with something");
		// Contact points: every contact stores a contact point and the two colliders
		// involved.
        if(col.gameObject.tag == "Enemy" && !lanceReady)
        {
            hp_bar.GetComponent<HealthBar>().decreaseHealth();
            Destroy(col.gameObject);
        }
        /*foreach (ContactPoint c in col.contacts) {
			//print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
			if (c.thisCollider.name == "VulnerableArea" && c.otherCollider.name == "EnemyLance" ||
				c.thisCollider.name == "EnemyLance" && c.otherCollider.name == "VulnerableArea") {
				print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
				hp_bar.GetComponent<HealthBar> ().decreaseHealth ();
			}
		}*/
        //print(col.gameObject.tag);
        if(col.gameObject.tag == "Ground")
        {
            grounded = true;
        }
	}

    void changeSpeed()
    {
        maxSpeed = (BoidController.flockSize * 5f) + 20f;
    }

	//Groundedcheck
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Ground") {
            print("GROUNDED");
			grounded = true;
		}
	}

	// TODO: consider moving this elsewhere
	public void LoadMenu() {
		SceneManager.LoadScene ("Menu");
	}

    public static float lanePosition()
    {
        switch (lane)
        {
            case 0: return leftLane;
            case 1: return midLane;
            case 2: return rightLane;
            default: return midLane;
        }
    }
}
