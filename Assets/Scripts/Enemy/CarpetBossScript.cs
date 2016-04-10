using UnityEngine;
using System.Collections;


public class CarpetBossScript : MonoBehaviour {
	[Header("Related Game Objects")]
	public GameObject chaser;
	public GameObject spawnedEnemy;
	public GameObject childHolder;
	public GameObject path; // The BaseTerrain
	public GameObject telegraphFireball;
	public GameObject telegraphSmoke;

	[Header("Boss Parameters")]
	public int maxHP = 10;
	public int bossHP = 10;
	public int chaserDistance = 20;
	public int bossPhase = 0;
	public float enemySpawnInterval = 6f;
	public float laneChangeTimer = 30f;
	public int currentLane = 0;
	public float distanceFromEdge;
	public float minDist = 300f; // distance when we instantiate path
	public int pathOffset = 1000; // Where to instantiate new path
	public float attackTimer = 20f;

	[Header("Action Flags")]
	//public bool spawnEnemies = true;
	//public bool bossMayChangeLanes = true;
	public bool attackDebug = true;
	public bool fading = false;
	public bool changingLanes = false;
	public bool attacking = false;

	[Header("Attack Objects/Animation")]
	public GameObject FireBall;
	public GameObject WideBeam;
	public GameObject VerticalBeam;

	[Header("Auxillary")]
	public GameObject explosion;
	//float phaseInterval = 1f;
	public Renderer[] renderers; 

	[Header("Audio")]
	public AudioSource audiosource;
	public AudioClip evilLaugh;
	public AudioClip spawnSound;
	public AudioClip charging;

	public enum attacks {fireBall = 1, wideBeam = 2, verticalBeam = 3};
	public enum laneNum {left = -4, center = 0, right = 4}
	int[] lanes = new int[3];

	[Header("UI")]
	public GameObject bossHearts;
	public GameObject bossHearts1;
	//bool coroutineFlag = false;

	// Boss Phases:
	// 0: The game hasn't started yet. Boss should idle.
	// 1: Boss should move, teleport occasionally, and spawn enemies.
	// 2: In addition to the above, the boss should teleport more frequently and shoot fireballs.
	// 3: In addition to the above, the boss should teleport more frequently and create walls of flame.
	// 4: In addition to the above, the boss should teleport more frequently and create pillars of fire.


	void Awake() {
		// TODO: Why would spawn enemy be called before Start()???
		audiosource = gameObject.GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		// Obtain all child renderers.
		renderers = this.gameObject.GetComponentsInChildren<Renderer> ();
		attacking = false;
		changingLanes = false;

		lanes[0] = -4;
		lanes[1] = 0;
		lanes[2] = 4;

		audiosource.pitch = 1;
		audiosource.PlayOneShot (evilLaugh);
		path = GameObject.Find ("BaseTerrain");
		float edge = path.transform.position.z + pathOffset/2;
		distanceFromEdge = Mathf.Abs (this.gameObject.transform.position.z - edge);

		this.transform.position = new Vector3(currentLane, this.transform.position.y, chaser.transform.position.z + chaserDistance);
	
		// UI
		bossHearts = GameObject.Find("BossHearts");
		bossHearts1 = GameObject.Find ("BossHearts1");
	}

	// Fungus calls changePhase as soon as the user clicks through the dialogue.
	public void changePhase() {
		Debug.Log ("Called changePhase()");
		bossPhase = 1;
		laneChangeTimer = 10f;
		attackTimer = 11f;
		StartCoroutine("spawnEnemyCoroutine");
		setLaneChangeTimer();
		setAttackTimer();
	}
		
	void setLaneChangeTimer() {
		Debug.Log ("Setting the lane change timer");
		switch (bossPhase) {
		case 0:
			break;
		case 1:
			laneChangeTimer = 10f;
			break;
		case 2:
			laneChangeTimer = 7f;
			break;
		case 3:
			laneChangeTimer = 5f;
			break;
		case 4:
			laneChangeTimer = 3f;
			break;
		default:
			break;
		}
	}

	void setAttackTimer() {
		switch (bossPhase) {
		case 0:
			break;
		case 1:
			break;
		case 2:
			attackTimer = 11f;
			break;
		case 3:
			attackTimer = 8f;
			break;
		case 4:
			attackTimer = 6f;
			break;
		default:
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		// Terrain drawing logic
		float edge = path.transform.position.z + pathOffset/2;
		distanceFromEdge = Mathf.Abs (this.gameObject.transform.position.z - edge);
		if (distanceFromEdge < minDist) {
			Vector3 newpos = new Vector3 (path.transform.position.x,
				                 path.transform.position.y, path.transform.position.z + pathOffset);
			path = Instantiate<GameObject> (path);
			path.transform.position = newpos;
		}
			
		// Lane changing and attacking are both affected by the boss's current health
		// Lane changing
		if (bossPhase > 0) {
			if (laneChangeTimer >= 0) {
				laneChangeTimer -= Time.deltaTime;
			} else {
				if (!changingLanes && !attacking) {
					StartCoroutine (changeLanes ());
					changingLanes = true;
				}
			}
		}
			
		// Attacking
		// Note: we don't want to teleport while telegraphing an attack. That wouldn't make sense.
		if (bossPhase > 1) {
			if (attackTimer >= 0) {
				attackTimer -= Time.deltaTime;
			} else {
				if (!attacking && !changingLanes) {
					int attackNum = 0;
					switch (bossPhase) {
					case 0:
						break;
					case 1:
						break;
					case 2:
						attackNum = 0;
						makeAttack (attackNum);
						break;
					case 3:
						attackNum = Random.Range ((int)0, (int)2);
						makeAttack (attackNum);
						break;
					case 4:
						attackNum = Random.Range ((int)0, (int)3);
						makeAttack(attackNum);
						break;
					default:
						break;
					}
					attacking = true;
				}
			}
		}

		// Because update is called frequently and I have a 1.5s
		// delay in the coroutine, I expect the position to change before the
		// coroutine begins the fade-in animation.
		this.transform.position =
			new Vector3 (currentLane, this.transform.position.y, chaser.transform.position.z + chaserDistance);

		// Debugging
		if (attackDebug) {
			if (Input.GetKeyDown(KeyCode.A) ) {
				StartCoroutine ("fireBall");
			}

			if (Input.GetKeyDown(KeyCode.S) ) {
				StartCoroutine ("verticalBeam");
			}			
		
			if (Input.GetKeyDown(KeyCode.D) ) {
				StartCoroutine ("wideBeam");
			}
			if (Input.GetKeyDown(KeyCode.F)) {
				Debug.Log ("Calling fade in update");
				currentLane = 1;
				StartCoroutine (changeLanes());
				changingLanes = true;
			}
		}
	}

	//Coroutines
	IEnumerator spawnEnemyCoroutine() {
		while (bossPhase > 0) {
			Debug.Log ("Spawning enemy");
			spawnEnemy();
			yield return new WaitForSeconds(enemySpawnInterval); 
		}
	}

	// Move to a new lane
	IEnumerator changeLanes() {
		// Fade out the boss
		Debug.Log ("Fading out");
		fading = true;
		yield return StartCoroutine (fadeCoroutine (false, 1f));
		Debug.Log ("changeLanesPart1(): disabling renderer");

		// Disable all renderers
		for (int i = 0; i < renderers.Length; ++i) {
			renderers [i].enabled = false;
		}

		// Set the current lane
		int coinToss = Random.Range((int)0, (int)2);	
		Debug.Log ("Chose value: " + coinToss);
		int newPosition = 0;
		if (currentLane == lanes[0]) {
			Debug.Log ("Current lane == 0");
			if (coinToss == 0) {
				newPosition = 1;
			} else {
				newPosition = 2;
			}
		} else if (currentLane == lanes[1]) {
			Debug.Log ("Current lane == 1");
			if (coinToss == 0) {
				newPosition = 0;
			} else {
				newPosition = 2;
			}
		} else if (currentLane == lanes[2]) {
			Debug.Log ("Current lane == 2");
			if (coinToss == 0) {
				newPosition = 0;
			} else {
				newPosition = 1;
			}
		} else {
			Debug.Log ("Current lane == " + currentLane);
		}
		currentLane = lanes [newPosition];
		yield return new WaitForSeconds(1.5f);

		// Enable all renderers
		for (int i = 0; i < renderers.Length; ++i) {
			renderers [i].enabled = true;
		}

		// Fade in the boss
		Debug.Log("Fading in");
		fading = true;
		yield return StartCoroutine (fadeCoroutine (true, 1f)); // fade in

		// Clean up
		changingLanes = false;
		setLaneChangeTimer ();
	}

	// Fade the boss in or out
	IEnumerator fadeCoroutine(bool fadeType, float multiplier) {
		// In one frame, fade in/out all renderers by the same amount
		// I assume that everything starts out opaque (alpha = 1).
		while (fading) {
			//Debug.Log ("Fading");
			for (int i = 0; i < renderers.Length; ++i) {
				if (renderers[i].material.HasProperty("_Color")) {
					Color c = renderers [i].material.color;
					// Stop fading if we're fading out and alpha <= 0, or we're
					// fading in and alpha >= 1.
					if (c.a <= 0 && !fadeType || c.a >= 1 && fadeType) {
						fading = false;
						break;
					}
					if (!fadeType) { // Fading out
						c.a -= 0.1f * multiplier; // For some reason, using Time.deltaTime causes the shadow to stick around. What???
					} else { // Fading in
						c.a += 0.1f * multiplier;
					}
					renderers [i].material.color = c;
				}
			}
			yield return null;
		}
		Debug.Log ("Fading done");
	}

	// Attack coroutines
	//Enemy attack functions
	void spawnEnemy() {
		audiosource.pitch = 1;
		audiosource.PlayOneShot(spawnSound);
		Vector3 spawnLocation = this.transform.position;
		int coinToss = Random.Range ((int)0, (int)2);
		if (currentLane == lanes [0]) {
			if (coinToss == 0) {
				spawnLocation.x = lanes [1];
			} else {
				spawnLocation.x = lanes [2];
			}
		} else if (currentLane == lanes [1]) {
			if (coinToss == 0) {
				spawnLocation.x = lanes [0];
			} else {
				spawnLocation.x = lanes [2];
			}
		} else if (currentLane == lanes [2]) {
			if (coinToss == 0) {
				spawnLocation.x = lanes [0];
			} else {
				spawnLocation.x = lanes [1];
			}
		} else {
			Debug.Log ("Spawning enemy in an unknown lane");
		}
		Instantiate (spawnedEnemy, spawnLocation, Quaternion.identity);
		return;
	}

	IEnumerator fireBall() {
		//audiosource.PlayOneShot (charging);
		telegraphFireball.SetActive (true);
		telegraphSmoke.SetActive (true);
		if (bossPhase == 2) {
			yield return new WaitForSeconds (3.5f);
			telegraphFireball.SetActive (false);
			telegraphSmoke.SetActive (false);
			for (int i = 0; i < 2; ++i) {
				Instantiate (FireBall, this.transform.position, Quaternion.identity);
				yield return StartCoroutine (changeLanes());
			}
		} else if (bossPhase == 3) {
			yield return new WaitForSeconds (2.5f);
			telegraphFireball.SetActive (false);
			telegraphSmoke.SetActive (false);
			for (int i = 0; i < 4; ++i) {
				Instantiate (FireBall, this.transform.position, Quaternion.identity);
				yield return StartCoroutine (changeLanes());
			}
		} else if (bossPhase == 4) {
			yield return new WaitForSeconds (1.5f);
			telegraphFireball.SetActive (false);
			telegraphSmoke.SetActive (false);
			for (int i = 0; i < 6; ++i) {
				Instantiate (FireBall, this.transform.position, Quaternion.identity);
				yield return StartCoroutine (changeLanes());
			}
		}


		attacking = false;
		setAttackTimer ();
	}

	IEnumerator verticalBeam() {
		telegraphFireball.SetActive (true);
		telegraphSmoke.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		telegraphFireball.SetActive (false);
		telegraphSmoke.SetActive (false);
		for (int i = 0; i < 3; ++i) {
			Instantiate (VerticalBeam, this.transform.position, Quaternion.identity);
			yield return StartCoroutine (changeLanes());
		}

		attacking = false;
		setAttackTimer ();
	}

	IEnumerator wideBeam() {
		telegraphFireball.SetActive (true);
		telegraphSmoke.SetActive (true);
		if (bossPhase == 3) {
			yield return new WaitForSeconds (3f);
		} else if (bossPhase == 4) {
			yield return new WaitForSeconds (2f);
		}
		Vector3 beamPos = new Vector3(0 , 1, this.transform.position.z  );
		Instantiate(WideBeam, beamPos, Quaternion.identity);
		telegraphFireball.SetActive (false);
		telegraphSmoke.SetActive (false);
		attacking = false;
		setAttackTimer ();
	}
		
	void makeAttack( int attack) {
		switch (attack){
			case 0:
				StartCoroutine ("fireBall");
				break;
			case 1:
				StartCoroutine ("wideBeam");
				break;
			case 2:
				StartCoroutine ("verticalBeam");
				break;
			default:
				print("You have entered the forbidden Zone");
				break;
		}
		return;
	}

	void OnCollisionEnter(Collision col) {
		print(col.gameObject.tag);
		if(col.gameObject.tag == "Weapon") { // && phaseInterval < 0 ) {
			bossHP-=1;
			//phaseInterval = 0.2f;
			if (bossHP < 10 && bossHP >= 7) {
				bossPhase = 2;
			} else if (bossHP < 7 && bossHP >= 4) {
				bossPhase = 3;
			} else if (bossHP < 4 && bossHP >= 1) {
				bossPhase = 4;
			}

			if (bossHP >= 5) {
				bossHearts1.GetComponent<HeartsScriptBoss> ().decreaseHealth ();
			} else {
				bossHearts.GetComponent<HeartsScriptBoss> ().decreaseHealth ();
			}

			print("Boss has been hit");
		}

		if (bossHP == 0 ) {
			print("HI");
			//spawnEnemies = false;
			//bossMayChangeLanes = false;
			bossPhase = 0;

			explosion.transform.position = this.transform.position;
			GameObject.Instantiate(explosion);
			Fungus.Flowchart.BroadcastFungusMessage ("LevelCleared");
			Destroy(this.gameObject);
			// load next scene here.
		}
	}


	void enemyTestSpawn() {
		if (Input.GetKey(KeyCode.Q) ) {
			Debug.Log("Q was pressed");
			spawnEnemy();
		}
		return;
	}
}
