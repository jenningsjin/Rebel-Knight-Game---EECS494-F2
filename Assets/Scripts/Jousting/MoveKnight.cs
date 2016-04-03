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
    bool left = false;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
		state = 0;
		hp_bar = GameObject.Find ("Hearts");
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
                if (tookDamage)
                {
                    healthTimer -= Time.deltaTime;
                    if (healthTimer > 0.75f)
                    {
                        transform.Rotate(Vector3.right, -10f * Time.deltaTime * 4f);
                        transform.Rotate(Vector3.up, -7.5f * Time.deltaTime * 4f);
                    }
                    else if (healthTimer < 0.75f && healthTimer > 0.5f)
                    {
                        if(transform.eulerAngles.y < 0f || transform.eulerAngles.y > 330f)
                        {
                            transform.Rotate(Vector3.up, 7.5f * Time.deltaTime * 8f);
                        }
                        if(transform.eulerAngles.x < 0f || transform.eulerAngles.x > 330f)
                        {
                            transform.Rotate(Vector3.right, 10f * Time.deltaTime * 8f);
                        }
                    } else
                    {
                        transform.eulerAngles = Vector3.zero;
                    }
                    if (healthTimer < 0)
                    {

                        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), false);
                        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), false);
                        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), false);
                        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), false);
                        tookDamage = false;
                        healthTimer = 1f;
                        transform.eulerAngles = Vector3.zero;
                    }
                }
                jumping();
			if( rigid.velocity.z < maxSpeed) {
				rigid.AddForce (Vector3.forward * 40f);
			}
                //Vector3 tmp = rigid.rotation.eulerAngles;
                switchLanes();
			if (Input.GetKeyDown (KeyCode.LeftArrow) && moveTimer < 0 && lane > 0) {
                    lane--;
                    switchLanes();
                    moveTimer = 0.1f;
                    left = true;
                    //tmp.y -= 0.5f;
                }
                else if (Input.GetKeyDown (KeyCode.RightArrow) && moveTimer < 0 && lane < 2) {
                    lane++;
                    switchLanes();
                    moveTimer = 0.1f;
                    left = false;
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
                } else if(Input.GetKeyDown(KeyCode.Space) && lanceTimer > 0)
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
        if(this.transform.position.x > pos.x+1f || this.transform.position.x < pos.x-1f)
        {
            if (left)
            {
                Vector3 angle = transform.eulerAngles;
                angle.y = -7.5f;
                transform.eulerAngles = angle;
            } else
            {
                Vector3 angle = transform.eulerAngles;
                angle.y = 7.5f;
                transform.eulerAngles = angle;
            }
        } else if(this.transform.position.x < pos.x+0.2f && this.transform.position.x > pos.x-0.2f &&
            this.transform.position.x >= pos.x+0.1f && this.transform.position.x <= pos.x - 0.1f)
        {
            Vector3 angle = transform.eulerAngles;
            angle.y = 0;
            transform.eulerAngles = angle;
        }
        //rigid.MovePosition(pos);
        rigid.position = Vector3.MoveTowards(transform.position, pos, 1.5f);
        //Rotation
    }

	void OnCollisionEnter(Collision col) {
		//print ("Player body collided with something");
		// Contact points: every contact stores a contact point and the two colliders
		// involved.
        if(col.gameObject.tag == "Enemy" && !lanceReady)
        {
            hp_bar.GetComponent<HeartsScript>().decreaseHealth();
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), true);
            Vector3 v = rigid.velocity;
            v.z = -5f;
            rigid.velocity = v;
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
            Destroy(col.gameObject);
        }
        if(col.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), true);
            hp_bar.GetComponent<HeartsScript>().decreaseHealth();
            Vector3 v = rigid.velocity;
            v.z = -5f;
            rigid.velocity = v;
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
        print(col.gameObject.tag);
        if(col.gameObject.tag == "Ground")
        {
            grounded = true;
            //transform.eulerAngles = Vector3.zero;
        }
	}

    void changeSpeed()
    {
        maxSpeed = (BoidController.flockSize * 4f) + 20f;
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

    void jumping()
    {
        if (!grounded)
        {
            Vector3 vel = rigid.velocity;
            if (this.transform.position.y > 5.5f)
            {
                vel.y -= 1.5f;
                rigid.velocity = vel;
            }
            print(rigid.velocity.y);
            //Rotation Logic
            if (rigid.velocity.y < 0f)
            {
                transform.Rotate(Vector3.right, 15f * Time.deltaTime * 4f);
            }
            else
            {
                transform.Rotate(Vector3.right, -10f * Time.deltaTime * 4f);
            }
        }
        else if (grounded && transform.eulerAngles.x > 0.5f && transform.eulerAngles.x < 30f)
        {
            //Gradually return rotation to 0
            transform.Rotate(Vector3.right, -10f * Time.deltaTime * 8f);
        }
        else if (transform.eulerAngles != Vector3.zero && !tookDamage)
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}
