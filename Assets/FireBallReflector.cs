using UnityEngine;
using System.Collections;

public class FireBallReflector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0f, 2.5f, 0f, Space.World);
	}
}
