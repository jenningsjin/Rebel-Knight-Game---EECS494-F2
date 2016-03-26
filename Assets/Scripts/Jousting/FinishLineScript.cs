using UnityEngine;
using System.Collections;

public class FinishLineScript : MonoBehaviour {
	// The variables below are temporary. Not sure why we have the Knight script attached
	// to the main camera of all places!
	GameObject maincamera;
	// Use this for initialization
	void Start () {
		if (!(maincamera = GameObject.Find ("Main Camera"))) {
			Debug.Log ("FinishLine: Couldn't find main camera");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		Debug.Log ("FinishLine: Collision detected");
		if (col.gameObject.CompareTag ("Knight")) {
			Debug.Log ("FinishLine: Your gallant knight has crossed the finish line.");
			maincamera.GetComponent<MoveKnight> ().state++;
		}
	}
}
