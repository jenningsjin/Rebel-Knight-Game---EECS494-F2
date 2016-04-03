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
	public bool spawnEnemies = true;

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
		


	}

	void spawnEnemy() {
		//this.gameObject.transform.position;
		Instantiate(spawnedEnemy, this.transform.position, Quaternion.identity);
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
