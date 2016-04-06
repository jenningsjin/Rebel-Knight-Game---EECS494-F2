using UnityEngine;
using System.Collections;


public class CarpetBossScript : MonoBehaviour {
	[Header("Related Game Objects")]
	public GameObject chaser;
	public GameObject spawnedEnemy;
	public GameObject childHolder;
	public GameObject path; // The BaseTerrain


	[Header("Boss Parameters")]
	public int bossHP = 8;
	public int chaserDistance = 20;
	public int bossPhase = 0;
	public float enemySpawnInterval = 250f;
	public float laneChangeInterval = 24f;
	public int currentLane = 0;
	public float distanceFromEdge;
	public float minDist = 300f; // distance when we instantiate path
	public int pathOffset = 1000; // Where to instantiate new path

	[Header("Testing Flags")]
	public bool spawnEnemies = true;
	public bool bossChangesLanes = true;
	public bool attackDebug = true;

	[Header("Attack Objects/Animation")]
	public GameObject FireBall;
	public GameObject WideBeam;
	public GameObject VerticalBeam;

	[Header("Auxillary")]
	public GameObject explosion;
	float phaseInterval = 1f;

	[Header("Audio")]
	public AudioSource audiosource;
	public AudioClip evilLaugh;
	public AudioClip spawnSound;


	public enum attacks {fireBall = 1, wideBeam = 2, verticalBeam = 3};
	public enum laneNum {left = -4, center = 0, right = 4}
	int[] lanes = new int[3];

	bool coroutineFlag = false;

	void Awake() {
		// TODO: Why would spawn enemy be called before Start()???
		audiosource = gameObject.GetComponent<AudioSource> ();
	}

	// Use this for initialization
	void Start () {
		lanes[0] = -4;
		lanes[1] = 0;
		lanes[2] = 4;
		//StartCoroutine("spawnEnemyCoroutine");
		//StartCoroutine("changeLaneCoroutine");
		audiosource.pitch = 1;
		audiosource.PlayOneShot (evilLaugh);
		path = GameObject.Find ("BaseTerrain");
		float edge = path.transform.position.z + pathOffset/2;
		distanceFromEdge = Mathf.Abs (this.gameObject.transform.position.z - edge);
		//Debug.Log ("Path center: " + path.transform.position.z + ", Edge: " + edge +
		//	", Distance from edge: " + distanceFromEdge);
	}

	public void changePhase() {
		StartCoroutine("spawnEnemyCoroutine");
		StartCoroutine("changeLaneCoroutine");
		bossPhase = 1;
		bossHP-=1;
	}

	
	// Update is called once per frame
	void Update () {
		// Terrain drawing logic
		float edge = path.transform.position.z + pathOffset/2;
		distanceFromEdge = Mathf.Abs (this.gameObject.transform.position.z - edge);
		//Debug.Log ("Path center: " + path.transform.position.z + ", Edge: " + edge +
		//	", Distance from edge: " + distanceFromEdge);
		if (distanceFromEdge < minDist) {
			//Debug.Log ("EXTENDING THE PATH");
			Vector3 newpos = new Vector3 (path.transform.position.x,
				                 path.transform.position.y, path.transform.position.z + pathOffset);
			path = Instantiate<GameObject> (path);
			path.transform.position = newpos;
		}

		phaseInterval -= Time.deltaTime;
		if( (bossHP == 8) && phaseInterval < 0f) {
			bossPhase = 0;
		}
		else if (bossHP == 7 && phaseInterval < 0) {
			bossPhase = 1;
		}
		else if (bossHP == 4 && phaseInterval < 0) {
			bossPhase = 2;
		}
		else if (bossHP == 2 && phaseInterval < 0)  {
			bossPhase = 3;
		}

//		else { // boss is dead
			//Destroy(this.gameObject);
//		}

		int attackChance = Random.Range(1, 400);		
		switch (bossPhase){
			case 0:
				//neutral phase
				break;
			case 1:
			/*
				if (!coroutineFlag) {
					StartCoroutine("spawnEnemyCoroutine");
					StartCoroutine("changeLaneCoroutine");
					coroutineFlag = true;
				}
			*/
				if (attackChance < 2) {
					int attackNum = Random.Range(1, 2);
					//makeAttack(attackNum);
				}
				break;
			case 2:
				if (attackChance < 2) {
					int attackNum = Random.Range(1, 2);
					makeAttack(attackNum);
				}			
				break;
			case 3:
				if (attackChance < 3) {
					int attackNum = Random.Range(1, 3);
					makeAttack(attackNum);
				}
				break;
			default:
				print("We should never be here");
				break;
		}
		
		


		if (attackDebug) {
			if (Input.GetKey(KeyCode.A) ) {
				fireBall();
			}

			if (Input.GetKey(KeyCode.S) ) {
				verticalBeam();
			}			
		
			if (Input.GetKey(KeyCode.D) ) {
				wideBeam();
			}

		}
		



		this.transform.position = new Vector3(currentLane, this.transform.position.y ,chaser.transform.position.z + chaserDistance);
	}

	//Coroutines
	IEnumerator spawnEnemyCoroutine() {
		while(spawnEnemies) {
			spawnEnemy();
			yield return new WaitForSeconds(enemySpawnInterval); 
		}
	}

	IEnumerator changeLaneCoroutine() {
		while(bossChangesLanes) {
			int newPosition = Random.Range(0,3);
			currentLane = lanes[newPosition];
			yield return new WaitForSeconds(laneChangeInterval);
		}
	}

	IEnumerator attackCoroutine() {
		yield return new WaitForSeconds(12);
	}

	//Enemy attack functions
	void spawnEnemy() {
		//this.gameObject.transform.position;
		audiosource.pitch = 1;
		audiosource.PlayOneShot(spawnSound);
		Instantiate(spawnedEnemy, this.transform.position, Quaternion.identity);
		return;
	}

	void fireBall() {
		GameObject attack = Instantiate(FireBall, this.transform.position, Quaternion.identity) as GameObject;
		//attack.GetComponent<Rigidbody>().velocity = Vector3.back * 6;
		attack.GetComponent<Rigidbody>().AddForce(Vector3.back*40);
		return;
	}

	void verticalBeam() {
		GameObject attack = Instantiate(VerticalBeam, this.transform.position, Quaternion.identity)  as GameObject;
		return;
	}

	void wideBeam() {
		Vector3 beamPos = new Vector3(0 , 1, this.transform.position.z  );
		GameObject attack = Instantiate(WideBeam, beamPos, Quaternion.identity)  as GameObject;
		return;
	}


	void makeAttack( int attack) {
		switch (attack){
			case 0:
				spawnEnemy();
				break;			
			case 1:
				fireBall();
				break;
			case 2:
				wideBeam();
				break;
			case 3:
				verticalBeam();
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
			bossChangesLanes = false;
			explosion.transform.position = this.transform.position;
			GameObject.Instantiate(explosion);
			Fungus.Flowchart.BroadcastFungusMessage ("LevelCleared");
			Destroy(this.gameObject);
			// load next scene here.
		}
	}


	void enemyTestSpawn() {
		if (Input.GetKey(KeyCode.Q) ) {
			print("A was pressed");
			spawnEnemy();
		}
		return;
	}
}
