using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour {
	public GameObject enemy;
	// Use this for initialization
	void Start () {

	}
	// Update is called once per frame
	void Update () {
		 if (Input.GetKey(KeyCode.A)) {
            print(this.gameObject.transform.position);
            Vector3 vect = this.gameObject.transform.position;
            //Instantiate(enemy , vect, Quaternion.identity);
            Instantiate(enemy);
        }			
	}
}
