using UnityEngine;
using System.Collections;

public class orbit : MonoBehaviour {
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
		transform.localRotation = Quaternion.Slerp (current, rotation, Time.deltaTime);
		transform.Translate (0, 0, 3 * Time.deltaTime);
		//Spin ();
	}

	void Spin() {
		transform.Rotate (Vector3.up * Time.deltaTime * speed, Space.World);
	}
}
