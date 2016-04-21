using UnityEngine;
using System.Collections;

public class FallingObstacle : MonoBehaviour {
    public GameObject drop;
    public Vector3 vel;
	public AudioSource audioSrc;
	// Use this for initialization
	void Start () {
		audioSrc = this.gameObject.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c) {
        if (c.gameObject.tag == "Knight") {
            vel = Vector3.zero;
            vel.y = -12f;
            //drop.GetComponent<Rigidbody>().useGravity = true;
            drop.GetComponent<Rigidbody>().velocity = vel;
            drop.GetComponent<Rigidbody>().AddForce(0, -16, 0, ForceMode.Acceleration);
			audioSrc.Play ();
        }
    }
}
