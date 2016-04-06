using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	Rigidbody rigid;
	//public AudioSource audiosource;
	//public AudioClip clip;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody> ();
		//audiosource = GetComponent<AudioSource> ();
		//audiosource.PlayOneShot (clip);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vel = rigid.velocity;
		vel.z = -40f;
		rigid.velocity = vel;
	}
}
