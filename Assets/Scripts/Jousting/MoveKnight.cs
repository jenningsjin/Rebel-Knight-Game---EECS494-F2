using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveKnight : MonoBehaviour {
	[Header("Debug")]
	public bool godMode = false;

	[Header("General Controls")]
    public static Rigidbody rigid;
    public int state;
    public int jumpSpeed = 100;
    public BoxCollider groundCollider;
    private float moveTimer = 0.1f;
    public static int lane = 1;
    public static float leftLane = -4f;
    public static float rightLane = 4f;
    public static float midLane = 0f;
    static float maxSpeed = 20f;
    public bool grounded = true;
    public static bool lanceReady = false;
    public GameObject lance;
    float lanceTimer = 0.5f;
    public GameObject person;
    float healthTimer = 1.5f;
    public bool tookDamage = false;
    //bool left = false;
    float attackDelay = 0.5f;
    bool lanceHit = false;
    static public bool powerUp = false;
    float powerUpTimer = 3f;
    float groundTimer = 1f;
	[Header("UI")]
	public GameObject hp_bar;
	bool sentMsg = false;

	[Header("Aesthetics")]
	public ParticleSystem particle;
	public Animator animator;
	public GameObject[] knightAndHorse; // for rendering
	public Renderer [] renderers; // for rendering
	public float damageFlashSpeed = 6.0f;
	public int numFlashes = 2;

	[Header("Audio")]
    public AudioClip neigh;
    public AudioClip damaged;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip attack;
    public AudioClip move;
    AudioSource audioSrc;

    [Header("Stampede")]
    public GameObject[] Stampede;
    public static int stampedeSize = 0;
    public Animator[] stampedeAnimators;

    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
		state = 0;
		hp_bar = GameObject.Find ("Hearts");
        lane = 1;
        particle = GetComponent<ParticleSystem>();
		ParticleSystem.EmissionModule em = particle.emission;
		em.enabled = false;
		//animator = GameObject.Find ("RiderController").GetComponent<Animator> ();
		renderers = new Renderer[2];
		for (int i = 0; i < 2; ++i) {
			renderers [i] = knightAndHorse [i].GetComponent<Renderer> ();
		}
        lance.SetActive(false);
        audioSrc = GetComponent<AudioSource>();
		if (SceneManager.GetActiveScene ().name == "JoustTutorial") {
			audioSrc.PlayOneShot (neigh, 0.25f);
		}
        switchLanes();
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), false);
    }

	// Called once user clicks through Fungus dialogue, or whenever
	// you want to start the game.
	public void BeginGame() {
		state = 1;
		animator.SetBool ("isRunning", true);
        stampedeAnimators[0].SetBool ("isRunning", true);
        stampedeAnimators[1].SetBool ("isRunning", true);
        stampedeAnimators[2].SetBool ("isRunning", true);
        stampedeAnimators[3].SetBool ("isRunning", true);
        stampedeAnimators[4].SetBool ("isRunning", true);               
	}

	// Fixed update is called at fixed time intervals. After fixed update, any
	// necessary physics calculations are made, so it's a good idea to use it with
	// rigid bodies.
    void FixedUpdate()
    {
        groundTimer -= Time.deltaTime;
        if (lanceReady) {
			ParticleSystem.EmissionModule em = particle.emission;
			em.enabled = true;
            lanceTimer -= Time.deltaTime;
            if (lanceTimer < 0.2 && lanceTimer > 0 && lanceHit) {
                Vector3 tmp = this.transform.eulerAngles;
                tmp.y += 1f;
                tmp.x -= 0.5f;
                this.transform.eulerAngles = tmp;
            }
            if (lanceTimer < 0) {
                em.enabled = false;
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
		// Toggle invincibility
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
            updateSpeed();
            if (tookDamage) {
                healthTimer -= Time.deltaTime;
                if (healthTimer > 1.25f) {
                	transform.Rotate(Vector3.right, -10f * Time.deltaTime * 4f);
                    transform.Rotate(Vector3.up, -7.5f * Time.deltaTime * 4f);
                } else if (healthTimer < 1.25f && healthTimer > 1f) {
                    if (transform.eulerAngles.y < 0f || transform.eulerAngles.y > 330f) {
                        transform.Rotate(Vector3.up, 7.5f * Time.deltaTime * 8f);
					}
                    if (transform.eulerAngles.x < 0f || transform.eulerAngles.x > 330f) {
                            transform.Rotate(Vector3.right, 10f * Time.deltaTime * 8f);
                    }
                } else {
                    transform.eulerAngles = Vector3.zero;
                }
                if (healthTimer < 0) {
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
			if (rigid.velocity.z < maxSpeed) {
				rigid.AddForce (Vector3.forward * 40f);
			}
            switchLanes();
			if (Input.GetKeyDown (KeyCode.LeftArrow) && moveTimer < 0 && lane > 0) {
                lane--;
                switchLanes();
                moveTimer = 0.1f;
                //left = true;
                audioSrc.PlayOneShot(move);
            } else if (Input.GetKeyDown (KeyCode.RightArrow) && moveTimer < 0 && lane < 2) {
                lane++;
                switchLanes();
                moveTimer = 0.1f;
                //left = false;
                audioSrc.PlayOneShot(move);
            } else if (Input.GetKeyDown (KeyCode.UpArrow) && grounded) {
                Vector3 vel = rigid.velocity;
                vel.y = jumpSpeed;
                rigid.velocity = vel;
			    grounded = false;
                audioSrc.PlayOneShot(jump, 2f);
			} else if (Input.GetKeyDown(KeyCode.DownArrow) && stampedeSize > 0) {
                Vector3 personPos = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
                personPos.z += 0.5f;
                person.transform.position = personPos;
                GameObject.Instantiate(person);

                Stampede[stampedeSize - 1].SetActive(false);
                stampedeSize-=1;


            } else if(Input.GetKeyDown(KeyCode.Space) && lanceTimer > 0 && attackDelay < 0) {
				audioSrc.PlayOneShot(attack);
                //Arm.transform.Rotate(40, 10, 0);
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
        switch (lane) {
            case 0: pos.x = leftLane; break;
            case 1: pos.x = midLane; break;
            case 2: pos.x = rightLane; break;
            default: pos.x = midLane; lane = 0; break;
        }
		this.transform.position = Vector3.MoveTowards(transform.position, pos, 0.35f);
    }

	void OnCollisionEnter(Collision col) {
		//print ("Player body collided with something");
		// Contact points: every contact stores a contact point and the two colliders
		// involved.
        if(col.gameObject.tag == "Enemy" && !lanceReady)
        {
			// Player has been damaged. Play 'flash red' animation.
			StartCoroutine(flashRedCoroutine(damageFlashSpeed, numFlashes));
            MoveEnemy x = col.gameObject.GetComponent<MoveEnemy>();
            if(x.state == 1)
            {
				if (!godMode && !tookDamage) {
					//Debug.Log ("Damaging player");
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
                audioSrc.PlayOneShot(damaged, 0.5f);
                Destroy(col.gameObject);
            }
        } else if (col.gameObject.tag == "Boss" && !lanceReady)
        {
			StartCoroutine(flashRedCoroutine(damageFlashSpeed, numFlashes));
			if (!godMode && !tookDamage) {
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
            audioSrc.PlayOneShot(damaged, 0.5f);
        }
        else if((col.gameObject.tag == "Enemy" || col.gameObject.tag == "Boss") && lanceReady)
        {
            lanceHit = true;

            lanceTimer = 0.2f;
            Time.timeScale = 0.25f;
        }
		if(col.gameObject.layer == LayerMask.NameToLayer("Obstacle") || col.gameObject.layer == LayerMask.NameToLayer("FireLayer"))
        {
			StartCoroutine(flashRedCoroutine(damageFlashSpeed, numFlashes));
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), true);
            // Handle god mode
			if (!godMode && !tookDamage) {
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
            audioSrc.PlayOneShot(damaged, 0.5f);
        }
        if(col.gameObject.tag == "Ground")
        {
            grounded = true;
            //transform.eulerAngles = Vector3.zero;
        }
        if (col.gameObject.tag == "Barrel") {
            col.gameObject.GetComponent<WallExplode>().Explode();
            Debug.Log("CRYYYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
        }
	}

	//Groundedcheck
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Ground") {
			//print("GROUNDED");
			if (!grounded)
			{
				audioSrc.PlayOneShot(land, 1.5f);
			}
			grounded = true;
            groundTimer = 1f;
			//transform.eulerAngles = Vector3.zero;
		}
	}

    void updateSpeed()
    {
        if (powerUp && powerUpTimer > 0)
        {
            powerUpTimer -= Time.deltaTime;
            maxSpeed = 50f;
            Vector3 vel = rigid.velocity;
            vel.z = maxSpeed;
            rigid.velocity = vel;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), true);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), true);
        } else if(powerUp && powerUpTimer < 0)
        {
            powerUpTimer = 3f;
            powerUp = false;
            maxSpeed = 20f;
            Vector3 vel = rigid.velocity;
            vel.z = maxSpeed;
            rigid.velocity = vel;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("Default"), false);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Obstacle"), LayerMask.NameToLayer("MainCamera"), false);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Default"), false);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("MainCamera"), false);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("Default"), false);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("FireLayer"), LayerMask.NameToLayer("MainCamera"), false);
        }
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
            if (this.transform.position.y > 5.5f) // TODO: The ground's height shouldn't be hard-coded like this.
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

        if(grounded && transform.position.y > 2f && groundTimer < 0)
        {
            Vector3 vel = rigid.velocity;
            vel.y = -4f;
            rigid.velocity = vel;
        }
    }

	public IEnumerator flashRedCoroutine(float multiplier, int numFlashes)
	{
		// Save the original colors of the horse and knight.
		Color[] originalColors = new Color[2];
		for (int i = 0; i < 2; ++i) {
			originalColors [i] = renderers [i].material.color;
		}

		for (int counter = 0; counter < numFlashes; ++counter) {
			// Increment the red to max
			//Debug.Log("1: Flashing red");
			bool flashingRed = true;
			while (flashingRed) {
				// Update each renderer's red value to a max of 1.
				for (int i = 0; i < renderers.Length; ++i) {
					Color c = renderers [i].material.color;
					c.r += multiplier * Time.deltaTime;
					c.g -= multiplier * Time.deltaTime;
					c.b -= multiplier * Time.deltaTime;
					if (c.r > 1) {
						c.r = 1;
					}
					if (c.g < 0) {
						c.g = 0;
					}
					if (c.b < 0) {
						c.b = 0;
					}
					renderers [i].material.color = c;
				}
				// Stop looping when all renderers have a red value of 1.
				flashingRed = false;
				for (int i = 0; i < renderers.Length; ++i) {
					if (renderers[i].material.color.r < 1 || 
						renderers[i].material.color.g > 0 ||
						renderers[i].material.color.b > 0) {
						flashingRed = true;
						break;
					}
				}
				yield return null;
			}
			// Return knight and horse to their original colors
			//Debug.Log ("2: Flashing back to original colors");
			while (!flashingRed) {
				// Decrease each renderer's red value
				for (int i = 0; i < renderers.Length; ++i) {
					Color c = renderers [i].material.color;
					c.r -= multiplier * Time.deltaTime;
					c.g += multiplier * Time.deltaTime;
					c.b += multiplier * Time.deltaTime;
					if (c.r < originalColors[i].r) {
						c.r = originalColors[i].r;
					}
					if (c.g > originalColors[i].g) {
						c.g = originalColors[i].g;
					}
					if (c.b > originalColors[i].b) {
						c.b = originalColors[i].b;
					}
					renderers[i].material.color = c;
				}
				flashingRed = true;
				for (int i = 0; i < renderers.Length; ++i) {
					if (renderers [i].material.color.r > originalColors [i].r ||
						renderers[i].material.color.g < originalColors[i].g ||
						renderers[i].material.color.b < originalColors[i].b) {
						flashingRed = false;
						break;
					}
				}
				yield return null;
			}
			//Debug.Log ("Done with flashing red");
		}
	}

	// TODO: consider moving this elsewhere
	public void LoadMenu() {
		SceneManager.LoadScene ("Menu");
	}
}
