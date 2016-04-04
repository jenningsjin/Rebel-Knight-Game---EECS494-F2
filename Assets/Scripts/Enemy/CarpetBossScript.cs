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

	[Header("Testing Flags")]
	public bool spawnEnemies = false;
	public bool attackDebug = true;

	[Header("Attack Objects/Animation")]
	public GameObject FireBall;
	public GameObject WideBeam;
	public GameObject VerticalBeam;


	public enum attacks {fireBall = 1, wideBeam = 2, verticalBeam = 3};
	// Use this for initialization
	void Start () {
		if (spawnEnemies) {
			InvokeRepeating("spawnEnemy", 9, 2.5f);
		}
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


		switch (bossPhase){
			case 0:
				//chaserDistance = 30;
				break;
			case 1:
				chaserDistance = 22;
				//CancelInvoke("spawnEnemy");
				break;
			case 2:
				chaserDistance = 17;
				break;
			case 3:
				chaserDistance = 6;
				break;
			default:
				print("We should never be here");
				break;
		}
		
		this.transform.position = new Vector3(0, this.transform.position.y ,chaser.transform.position.z + chaserDistance);
		


		if (attackDebug) {
			if (Input.GetKeyDown(KeyCode.A) ) {
				fireBall();
			}

			if (Input.GetKeyDown(KeyCode.S) ) {
				verticalBeam();
			}			
		
			if (Input.GetKeyDown(KeyCode.D) ) {
				wideBeam();
			}

		}
	}

	void spawnEnemy() {
		//this.gameObject.transform.position;
		Instantiate(spawnedEnemy, this.transform.position, Quaternion.identity);
		return;
	}

	void fireBall() {
		GameObject attack = Instantiate(FireBall, this.transform.position, Quaternion.identity) as GameObject;
		attack.GetComponent<Rigidbody>().velocity = Vector3.back * 6;
		//attack.GetComponent<Rigidbody>().AddForce(Vector3.back*40);
		return;
	}

	void verticalBeam() {
		GameObject attack = Instantiate(VerticalBeam, this.transform.position, Quaternion.identity)  as GameObject;
		return;
	}

	void wideBeam() {
		Vector3 beamPos = new Vector3(this.transform.position.x, 1, this.transform.position.z  );
		GameObject attack = Instantiate(WideBeam, beamPos, Quaternion.identity)  as GameObject;
		return;
	}


	void makeAttack( int attack) {
		switch (attack){
			case 0:
				fireBall();
				break;
			case 1:
				wideBeam();
				break;
			case 2:
				verticalBeam();
				break;
			default:
				print("You have entered the forbidden Zone");
				break;
		}

		return;
	}

	void OnCollisionEnter(Collision col) {
		Destroy(this.gameObject);
	}

	void enemyTestSpawn() {
		if (Input.GetKey(KeyCode.Q) ) {
			print("A was pressed");
			spawnEnemy();
		}
		return;
	}
}
