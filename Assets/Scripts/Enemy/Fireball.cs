using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	Rigidbody rigid;
	public GameObject explosion;
	public float timer = 5f;

	// Use this for initialization
	void Start () {
		//Debug.Log ("Timer == " + timer);
		rigid = GetComponent<Rigidbody> ();
	}

	void Update() {
		timer -= Time.deltaTime;
		//Debug.Log ("Time.deltaTime == " + Time.deltaTime + ", timer == " + timer);
		if (timer <= 0) {
			explosion.transform.position = this.transform.position;
			GameObject.Instantiate(explosion);
			Destroy (this.gameObject);
		}
	}

	void FixedUpdate () {
		/*if (Mathf.Abs (rigid.velocity.z) < 30f) {
			rigid.AddForce (Vector3.back * 5);
		}*/
		rigid.velocity = Vector3.back * 20f; 
	}

	void OnCollisionEnter(Collision c) {
		if (c.gameObject.name == "Knight" || c.gameObject.CompareTag("Obstacle") || c.gameObject.CompareTag("Weapon")) {
			explosion.transform.position = this.transform.position;
			GameObject.Instantiate(explosion);
			Destroy (this.gameObject);
		}
	}
}
