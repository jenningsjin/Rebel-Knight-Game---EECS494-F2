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
	public bool grounded = true;
    public static bool lanceReady = false;
    float lanceTimer = 1f;
    public GameObject person;
    public ParticleSystem particle;
    float healthTimer = 1f;
    bool tookDamage = false;

    public Weapons weapon;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
		state = 0;
		hp_bar = GameObject.Find ("HP_Bar");
        lane = 1;
        switchLanes();
        particle = GetComponent<ParticleSystem>();
        particle.enableEmission = false;
        weapon = GameObject.FindObjectOfType<Weapons>();
    }

	public void BeginGame() {
		++state;
	}

	// Update is called once per frame
	void Update () {
        moveTimer -= Time.deltaTime;
        if (tookDamage)
        {
            healthTimer -= Time.deltaTime;
            if(healthTimer < 0)
            {
                
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), false);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), false);
                tookDamage = false;
                healthTimer = 1f;
            }
        }
        if (lanceReady)
        {
            particle.enableEmission = true;
            lanceTimer -= Time.deltaTime;
            weapon.weapons[weapon.index].SetActive(true);
            if(lanceTimer < 0)
            {
                particle.enableEmission = false;
                weapon.weapons[weapon.index].SetActive(false);
                lanceTimer = 1f;
                lanceReady = false;
            }
        }
        switch (state) {
		case 0: // Before game start
			break;
		case 1: // Charge
            changeSpeed();
                if (!grounded)
                {
                    Vector3 vel = rigid.velocity;
                    if(this.transform.position.y > 5.5f)
                    {
                        vel.y -= 1.5f;
                        rigid.velocity = vel;
                    }
                    print(rigid.velocity.y);
                    //Rotation Logic
                    if (rigid.velocity.y < 0f)
                    {
                        transform.Rotate(Vector3.right, 20f * Time.deltaTime * 4f);
                    } else
                    {
                        transform.Rotate(Vector3.right, -10f * Time.deltaTime * 4f);
                    }
                } else if(grounded && transform.eulerAngles.x > 0f && transform.eulerAngles.x < 30f)
                {
                    //Gradually return rotation to 0
                    transform.Rotate(Vector3.right, -10f * Time.deltaTime * 8f);
                } else if(transform.eulerAngles != Vector3.zero)
                {
                    transform.eulerAngles = Vector3.zero;
                }
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
                    Vector3 vel = rigid.velocity;
                    vel.y = jumpSpeed;
                    rigid.velocity = vel;
				    grounded = false;
                    rigid.transform.eulerAngles = Vector3.zero;
			}
                else if (Input.GetKeyDown(KeyCode.DownArrow) && BoidController.flockSize > 0)
                {
                    Vector3 personPos = this.transform.position;
                    personPos.z += 0.5f;
                    person.transform.position = personPos;
                    GameObject.Instantiate(person);
                    BoidController.flockSize--;
                } else if(Input.GetKeyDown(KeyCode.Space) && lanceTimer > 0 && weapon.index == 0)
                {
                    lanceReady = true;
                }
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
            if (!grounded)
            {
                Vector3 vel = rigid.velocity;
                if (vel.y > 0)
                {
                    vel.y = -1f;
                }
                rigid.velocity = vel;
            }
            Destroy(col.gameObject);
        }
        if(col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), true);
            hp_bar.GetComponent<HealthBar>().decreaseHealth();
            if (!grounded)
            {
                Vector3 vel = rigid.velocity;
                if (vel.y > 0)
                {
                    vel.y = -1f;
                }
                rigid.velocity = vel;
            }
            tookDamage = true;
        }
        /*foreach (ContactPoint c in col.contacts) {
			//print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
			if (c.thisCollider.name == "VulnerableArea" && c.otherCollider.name == "EnemyLance" ||
				c.thisCollider.name == "EnemyLance" && c.otherCollider.name == "VulnerableArea") {
				print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
				hp_bar.GetComponent<HealthBar> ().decreaseHealth ();
			}
		}*/
        print(col.gameObject.tag);
        if(col.gameObject.tag == "Ground")
        {
            grounded = true;
            //transform.eulerAngles = Vector3.zero;
        }
	}

    void changeSpeed()
    {
        maxSpeed = (BoidController.flockSize * 5f) + 20f;
    }

	//Groundedcheck
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Ground") {
            //print("GROUNDED");
			grounded = true;
            //transform.eulerAngles = Vector3.zero;
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
