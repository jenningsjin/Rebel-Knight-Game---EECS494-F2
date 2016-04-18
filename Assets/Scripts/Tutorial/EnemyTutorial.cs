using UnityEngine;
using System.Collections;

public class EnemyTutorial : MonoBehaviour {
	bool printingMsg = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c) {
		if (c.gameObject.name == "Knight" && !printingMsg) {
			Debug.Log ("Knight entered tutorial enemy area");
			Fungus.Flowchart.BroadcastFungusMessage ("EnemyTutorial");
			printingMsg = true;
		}
	}

	void OnTriggerExit(Collider c) {
		if (printingMsg) {
			// This kills the active say dialogue. Must guarantee that it is the enemy dialogue
			// it kills.
			Debug.Log ("Enemy dialogue done");
			Debug.Log (Fungus.SayDialog.activeSayDialog.storyText.text);
			Fungus.SayDialog.activeSayDialog.Stop();
		}
	}
}
