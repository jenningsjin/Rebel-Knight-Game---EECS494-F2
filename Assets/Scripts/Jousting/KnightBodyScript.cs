using UnityEngine;
using System.Collections;

public class KnightBodyScript : MonoBehaviour {
	public GameObject hp_bar;

	// Use this for initialization
	void Start () {
		hp_bar = GameObject.Find ("HP_Bar");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log ("COLLISION with HORSE");
		if (collision.gameObject.CompareTag ("Enemy")) {
			hp_bar.GetComponent<HealthBar> ().decreaseHealth ();
		}
	}
}
