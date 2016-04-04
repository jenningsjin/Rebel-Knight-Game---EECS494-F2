using UnityEngine;
using System.Collections;

public class AllyProjectile : MonoBehaviour {
	private Rigidbody rigid;
	public GameObject explosion;
	public GameObject player;
	public GameObject speedDial;
    public AudioClip yay;
    AudioSource audio;
	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody>();
		Vector3 vel = MoveKnight.rigid.velocity;
		//vel.y = 10f;
		vel.z += 20f;
		rigid.velocity = vel;
		player = GameObject.Find ("Knight");
		speedDial = GameObject.Find ("Speed");
        audio = GetComponent<AudioSource>();
        if(Random.Range(0f, 2f) > 1f)
        {
            audio.PlayOneShot(yay, 0.5f);
        }
    }

	// Update is called once per frame
	void Update()
	{
		if (Mathf.Abs (this.transform.position.z - player.transform.position.z) > 30f) {
			speedDial.GetComponent<SpeedScript> ().decreaseSpeedDial ();
			explosion.transform.position = this.transform.position;
			GameObject.Instantiate (explosion);
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.CompareTag ("Obstacle") || coll.gameObject.CompareTag ("Enemy")) {
			speedDial.GetComponent<SpeedScript> ().decreaseSpeedDial ();
			explosion.transform.position = this.transform.position;
			GameObject.Instantiate (explosion);
			Destroy (this.gameObject);
		}
	}
}