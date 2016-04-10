using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveKnight : MonoBehaviour {
	bool godMode = false;
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
    public GameObject lance;
    float lanceTimer = 0.5f;
    public GameObject person;
    public ParticleSystem particle;
    float healthTimer = 1.5f;
    bool tookDamage = false;
    bool left = false;
    float attackDelay = 0.5f;
    bool lanceHit = false;
    public AudioClip neigh;
    public AudioClip damaged;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip attack;
    public AudioClip move;
    AudioSource audio;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
		state = 0;
		hp_bar = GameObject.Find ("Hearts");
        lane = 1;
        particle = GetComponent<ParticleSystem>();
        particle.enableEmission = false;
        lance.SetActive(false);
        audio = GetComponent<AudioSource>();
		if (SceneManager.GetActiveScene ().name == "JoustTutorial") {
			audio.PlayOneShot (neigh, 0.25f);
		}
        switchLanes();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), false);
    }

	public void BeginGame() {
		state = 1;
	}
    void FixedUpdate()
    {
        if (lanceReady)
        {
            particle.enableEmission = true;
            lanceTimer -= Time.deltaTime;
            if(lanceTimer < 0.2 && lanceTimer > 0 && lanceHit)
            {
                Vector3 tmp = this.transform.eulerAngles;
                tmp.y += 1f;
                tmp.x -= 0.5f;
                this.transform.eulerAngles = tmp;
            }
            if (lanceTimer < 0)
            {
                particle.enableEmission = false;
                lanceTimer = 0.5f;
                Time.timeScale = 1f;
                lanceReady = false;
                lance.SetActive(false);
                this.transform.eulerAngles = Vector3.zero;
                lanceHit = false;
            }
        }
    }
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G) && !godMode) {
			godMode = true;
		} else if (Input.GetKeyDown(KeyCode.G)) {
			godMode = false;
		}
        moveTimer -= Time.deltaTime;

        switch (state) {
		case 0: // Before game start
			break;
		case 1: // Charge
                attackDelay -= Time.deltaTime;
            changeSpeed();
                if (tookDamage)
                {
                    healthTimer -= Time.deltaTime;
                    if (healthTimer > 1.25f)
                    {
                        transform.Rotate(Vector3.right, -10f * Time.deltaTime * 4f);
                        transform.Rotate(Vector3.up, -7.5f * Time.deltaTime * 4f);
                    }
                    else if (healthTimer < 1.25f && healthTimer > 1f)
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
                        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), false);
                        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), false);
                        tookDamage = false;
                        healthTimer = 1.5f;
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
                    audio.PlayOneShot(move);
                    //tmp.y -= 0.5f;
                }
                else if (Input.GetKeyDown (KeyCode.RightArrow) && moveTimer < 0 && lane < 2) {
                    lane++;
                    switchLanes();
                    moveTimer = 0.1f;
                    left = false;
                    audio.PlayOneShot(move);
                    //tmp.y += 0.5f;
                }
                else if (Input.GetKeyDown (KeyCode.UpArrow) && grounded) {
                    Vector3 vel = rigid.velocity;
                    vel.y = jumpSpeed;
                    rigid.velocity = vel;
				    grounded = false;
                    audio.PlayOneShot(jump, 2f);
                    //rigid.transform.eulerAngles = Vector3.zero;
			}
                else if (Input.GetKeyDown(KeyCode.DownArrow) && BoidController.flockSize > 0)
                {
                    Vector3 personPos = this.transform.position;
                    personPos.z += 0.5f;
                    person.transform.position = personPos;
                    GameObject.Instantiate(person);
                    BoidController.flockSize--;
                } else if(Input.GetKeyDown(KeyCode.Space) && lanceTimer > 0 && attackDelay < 0)
                {
                    lanceReady = true;
                    lance.SetActive(true);
                    attackDelay = 0.5f;
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
            MoveEnemy x = col.gameObject.GetComponent<MoveEnemy>();
            if(x.state == 1)
            {
				if (!godMode) {
					hp_bar.GetComponent<HeartsScript> ().decreaseHealth ();
				}
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), true);
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), true);
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
                audio.PlayOneShot(damaged, 0.5f);
                Destroy(col.gameObject);
            }
        } else if (col.gameObject.tag == "Boss" && !lanceReady)
        {
            hp_bar.GetComponent<HeartsScript>().decreaseHealth();
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), true);
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
            audio.PlayOneShot(damaged, 0.5f);
        }
        else if((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Boss") && lanceReady)
        {
            lanceHit = true;
            audio.PlayOneShot(attack);
            lanceTimer = 0.2f;
            Time.timeScale = 0.25f;
        }
		if(col.gameObject.layer == LayerMask.NameToLayer("Obstacle") || col.gameObject.layer == LayerMask.NameToLayer("FireLayer"))
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), true);
            // Handle god mode
            if (!godMode) {
				hp_bar.GetComponent<HeartsScript> ().decreaseHealth ();
			}
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
            if (lanceReady)
            {
                lanceTimer = 0;
            }
            audio.PlayOneShot(damaged, 0.5f);
        }
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
            if (!grounded)
            {
                audio.PlayOneShot(land, 1.5f);
            }
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
        else if (transform.eulerAngles != Vector3.zero && !tookDamage && !lanceReady)
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}
