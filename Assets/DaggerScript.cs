using UnityEngine;
using System.Collections;

public class DaggerScript : MonoBehaviour {
	public Transform target;
	public float speed;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Knight").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, target.position, Time.deltaTime * speed);
		//transform.Rotate (0f, 10f, 5f, Space.World);
	}
}
