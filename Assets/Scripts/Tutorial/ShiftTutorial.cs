using UnityEngine;
using System.Collections;

public class ShiftTutorial : MonoBehaviour {
	bool printingMsg = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c) {
		if (c.gameObject.name == "Knight" && !printingMsg) {
			Debug.Log(c.gameObject.transform.position);
			Debug.Log(c.gameObject.name);
			Debug.Log ("Knight entered tutorial shift area");
			Fungus.Flowchart.BroadcastFungusMessage ("ShiftTutorial");
			printingMsg = true;
		}
	}

	void OnTriggerExit(Collider c) {
		if (printingMsg) {
			Debug.Log ("Shift dialogue done");
			Debug.Log (Fungus.SayDialog.activeSayDialog.storyText.text);
			Fungus.SayDialog.activeSayDialog.Stop();
		}
	}
}
