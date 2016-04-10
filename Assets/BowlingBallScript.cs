using UnityEngine;
using System.Collections;

public class BowlingBallScript : MonoBehaviour {
	public Transform target;
	public float speed;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Knight").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, target.position, Time.deltaTime * speed);
		transform.Rotate (30f, 2f, 2f, Space.World);
	}
}
