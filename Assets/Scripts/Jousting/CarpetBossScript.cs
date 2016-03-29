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
			InvokeRepeating("spawnEnemy", 4, 2.5f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if( (bossHP <=15) && (bossHP > 12)) {
			bossPhase = 0;
		}
		else if (bossHP <=12 && bossHP > 7) {
			bossPhase = 1;
		}
		else if (bossHP <= 7 && bossHP > 1) {
			bossPhase = 2;
		}
		else  {
			bossPhase = 3;
		}


		switch (bossPhase){
			case 0:
				//chaserDistance = 30;
				break;
			case 1:
				chaserDistance = 12;
				//CancelInvoke("spawnEnemy");
				break;
			case 2:
				chaserDistance = 8;
				break;
			case 3:
				chaserDistance = 1;
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
