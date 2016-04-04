using UnityEngine;
using System.Collections;


public class CarpetBossScript : MonoBehaviour {
	[Header("Related Game Objects")]
	public GameObject chaser;
	public GameObject spawnedEnemy;

	[Header("Boss Parameters")]
	static public int bossHP = 15;
	public int chaserDistance = 30;
	public int bossPhase = 0;
	public float enemySpawnInterval = 7f;
	public float laneChangeInterval = 5f;
	public int currentLane = 0;

	[Header("Testing Flags")]
	public bool spawnEnemies = true;
	public bool bossChangesLanes = true;
	public bool attackDebug = true;

	[Header("Attack Objects/Animation")]
	public GameObject FireBall;
	public GameObject WideBeam;
	public GameObject VerticalBeam;


	public enum attacks {fireBall = 1, wideBeam = 2, verticalBeam = 3};
	public enum laneNum {left = -4, center = 0, right = 4}
	int[] lanes = new int[3];

	// Use this for initialization
	void Start () {
		lanes[0] = -4;
		lanes[1] = 0;
		lanes[2] = 4;
		StartCoroutine("spawnEnemyCoroutine");
		StartCoroutine("changeLaneCoroutine");
	}
	
	// Update is called once per frame
	void Update () {
		if( (bossHP <=15) && (bossHP > 11)) {
			bossPhase = 0;
		}
		else if (bossHP <=11 && bossHP > 7) {
			bossPhase = 1;
		}
		else if (bossHP <= 7 && bossHP > 1) {
			bossPhase = 2;
		}
		else if (bossHP == 1)  {
			bossPhase = 3;
		}

		else { // boss is dead
			Destroy(this.gameObject);
		}

		int attackChance = Random.Range(1, 150);		
		switch (bossPhase){
			case 0:
				//neutral phase

				break;
			case 1:
				if (attackChance < 2) {
					int attackNum = Random.Range(1, 2);
					//makeAttack(attackNum);
				}
				break;
			case 2:
				if (attackChance < 2) {
					int attackNum = Random.Range(1, 3);
					makeAttack(attackNum);
				}			
				break;
			case 3:
				if (attackChance < 2) {
					int attackNum = Random.Range(1, 4);
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
		if(col.gameObject.tag == "Weapon" ) {
			bossHP-=1;
			print("Boss has been hit");
		}
		if (bossHP == 0 ) {
			spawnEnemies = false;
			bossChangesLanes = false;
		}
	}

	void OnTriggerEnter(Collider col) {

	}

	void enemyTestSpawn() {
		if (Input.GetKey(KeyCode.Q) ) {
			print("A was pressed");
			spawnEnemy();
		}
		return;
	}
}
