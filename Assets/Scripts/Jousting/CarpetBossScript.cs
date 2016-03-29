using UnityEngine;
using System.Collections;


public class CarpetBossScript : MonoBehaviour {
	
	public GameObject spawnedEnemy;

	// Use this for initialization
	void Start () {
		InvokeRepeating("spawnEnemy", 4, 1f);
	}
	
	// Update is called once per frame
	void Update () {
			
			if (Input.GetKey(KeyCode.A) ) {
				print("A was pressed");
				spawnEnemy();
			}
		
	}

	void spawnEnemy() {
		this.gameObject.transform.position;
		Instantiate(spawnedEnemy);
		return;
	}
}
