using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {
	public Transform target;
	public float speed = 1000.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 relativePos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation (relativePos);
		Quaternion current = transform.localRotation;
		// Find a vector with a rotation between the last frame's rotation and our target.
		transform.localRotation = Quaternion.Slerp (current, rotation, Time.deltaTime*3);
		transform.Translate (0, 0, 30 * Time.deltaTime);
		//Spin ();
	}

	void Spin() {
		transform.Rotate (Vector3.up * Time.deltaTime * speed, Space.World);
	}
}
