using UnityEngine;
using System.Collections;


public class CarpetBossScript : MonoBehaviour {
	[Header("Related Game Objects")]
	public GameObject chaser;
	public GameObject spawnedEnemy;
	public GameObject childHolder;
	public GameObject path; // The BaseTerrain


	[Header("Boss Parameters")]
	public int maxHP = 10;
	public int bossHP = 10;
	public int chaserDistance = 20;
	public int bossPhase = 0;
	public float enemySpawnInterval = 250f;
	public float laneChangeInterval = 30f;
	public int currentLane = 0;
	public float distanceFromEdge;
	public float minDist = 300f; // distance when we instantiate path
	public int pathOffset = 1000; // Where to instantiate new path
	public float attackInterval = 20f;

	[Header("Action Flags")]
	public bool spawnEnemies = true;
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
	float phaseInterval = 1f;
	public Renderer[] renderers; 

	[Header("Audio")]
	public AudioSource audiosource;
	public AudioClip evilLaugh;
	public AudioClip spawnSound;


	public enum attacks {fireBall = 1, wideBeam = 2, verticalBeam = 3};
	public enum laneNum {left = -4, center = 0, right = 4}
	int[] lanes = new int[3];

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

		lanes[0] = -4;
		lanes[1] = 0;
		lanes[2] = 4;

		audiosource.pitch = 1;
		audiosource.PlayOneShot (evilLaugh);
		path = GameObject.Find ("BaseTerrain");
		float edge = path.transform.position.z + pathOffset/2;
		distanceFromEdge = Mathf.Abs (this.gameObject.transform.position.z - edge);

		this.transform.position = new Vector3(currentLane, this.transform.position.y, chaser.transform.position.z + chaserDistance);
	}

	// Fungus calls changePhase as soon as the user clicks through the dialogue.
	public void changePhase() {
		Debug.Log ("Called changePhase()");
		StartCoroutine("spawnEnemyCoroutine");
		bossPhase = 1;
		// Set timer intervals
		setLaneChangeInterval();
		setAttackInterval ();
	}
		
	void setLaneChangeInterval() {
		switch (bossPhase) {
		case 0:
			break;
		case 1:
			laneChangeInterval = 30f;
			break;
		case 2:
			laneChangeInterval = 20f;
			break;
		case 3:
			laneChangeInterval = 10f;
			break;
		case 4:
			laneChangeInterval = 5f;
			break;
		default:
			break;
		}
	}

	void setAttackInterval() {
		switch (bossPhase) {
		case 0:
			break;
		case 1:
			break;
		case 2:
			attackInterval = 20f;
			break;
		case 3:
			attackInterval = 15f;
			break;
		case 4:
			attackInterval = 10f;
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
			if (laneChangeInterval >= 0) {
				laneChangeInterval -= Time.deltaTime;
				this.transform.position =
					new Vector3 (currentLane, this.transform.position.y, chaser.transform.position.z + chaserDistance);
			} else {
				int newPosition = Random.Range (0, 3);
				if (currentLane != lanes [newPosition] && !changingLanes) {
					StartCoroutine (changeLanes ());
					changingLanes = true;
					currentLane = lanes [newPosition];
				}
			}
		}
			
		// Attacking
		// Note: we don't want to teleport while telegraphing an attack. That wouldn't make sense.
		if (bossPhase > 1) {
			if (attackInterval >= 0) {
				attackInterval -= Time.deltaTime;
			} else {
				if (!attacking && !changingLanes) {
					int attackNum = 0;
					switch (bossPhase) {
					case 1:
						attackNum = Random.Range (1, 2);
						makeAttack (attackNum);
						break;
					case 2:
						attackNum = Random.Range (1, 3);
						makeAttack (attackNum);
						break;
					case 3:
						attackNum = Random.Range(1, 4);
						makeAttack(attackNum);
						break;
					default:
						break;
					}
					attacking = true;
				}
			}
		}

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
				StartCoroutine (changeLanes ());
				changingLanes = true;
			}
		}
	}

	//Coroutines
	IEnumerator spawnEnemyCoroutine() {
		while(spawnEnemies) {
			spawnEnemy();
			yield return new WaitForSeconds(enemySpawnInterval); 
		}
	}

	// Move to a new lane
	IEnumerator changeLanes() {
		// Fade out the boss
		Debug.Log ("Fading out");
		fading = true;
		yield return StartCoroutine (fadeCoroutine(false, 1f));
		Debug.Log ("changeLanes(): disabling renderer");

		// Disable all renderers
		for (int i = 0; i < renderers.Length; ++i) {
			renderers [i].enabled = false;
		}

		// Teleport
		yield return new WaitForSeconds(1.0f);
		this.transform.position = new Vector3(currentLane, this.transform.position.y, chaser.transform.position.z + chaserDistance);

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
		setLaneChangeInterval ();
	}

	// Fade the boss in or out
	IEnumerator fadeCoroutine(bool fadeType, float multiplier) {
		// In one frame, fade in/out all renderers by the same amount
		// I assume that everything starts out opaque (alpha = 1).
		while (fading) {
			//Debug.Log ("Fading");
			for (int i = 0; i < renderers.Length; ++i) {
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
			yield return null;
		}
		Debug.Log ("Fading done");
	}

	// Attack coroutines

	//Enemy attack functions
	void spawnEnemy() {
		audiosource.pitch = 1;
		audiosource.PlayOneShot(spawnSound);
		Instantiate(spawnedEnemy, this.transform.position, Quaternion.identity);
		return;
	}

	IEnumerator fireBall() {
		GameObject attack = Instantiate(FireBall, this.transform.position, Quaternion.identity)  as GameObject;
		yield return null;
	}

	IEnumerator verticalBeam() {
		GameObject attack = Instantiate(VerticalBeam, this.transform.position, Quaternion.identity)  as GameObject;
		yield return null;
	}

	IEnumerator wideBeam() {
		Vector3 beamPos = new Vector3(0 , 1, this.transform.position.z  );
		GameObject attack = Instantiate(WideBeam, beamPos, Quaternion.identity)  as GameObject;
		yield return null;
	}
		
	void makeAttack( int attack) {
		switch (attack){
			case 0:
				spawnEnemy();
				break;			
			case 1:
				StartCoroutine ("fireBall");
				break;
			case 2:
				StartCoroutine ("wideBeam");
				break;
			case 3:
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
		if(col.gameObject.tag == "Weapon" && phaseInterval < 0 ) {
			bossHP-=1;
			phaseInterval = 0.2f;
			print("Boss has been hit");
		}

		if (bossHP == 0 ) {
			print("HI");
			spawnEnemies = false;
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
