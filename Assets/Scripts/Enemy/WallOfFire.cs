using UnityEngine;
using System.Collections;

public class WallOfFire : MonoBehaviour {
	Rigidbody rigid;
	public GameObject explosion;
	public float timer = 20f;

	// Use this for initialization
	void Start () {
		//Debug.Log ("Timer == " + timer);
		rigid = GetComponent<Rigidbody> ();
	}

	void Update() {
		timer -= Time.deltaTime;
		//Debug.Log ("Time.deltaTime == " + Time.deltaTime + ", timer == " + timer);
		if (timer <= 0) {
			Destroy (this.gameObject);
		}
	}
}
