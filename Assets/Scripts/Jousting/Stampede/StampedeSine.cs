using UnityEngine;
using System.Collections;

public class StampedeSine : MonoBehaviour {
	public float flutterRange = .001f;
	public float flutterSpeed = 1f;
	float time = 0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime*flutterSpeed;
		//time+=flutterSpeed;
		float drift = Mathf.Sin(time) * flutterRange;
		transform.Translate( drift , 0, 0 );	
	}
}
