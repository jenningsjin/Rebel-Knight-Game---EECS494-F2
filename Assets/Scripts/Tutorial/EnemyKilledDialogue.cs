using UnityEngine;
using System.Collections;

public class EnemyKilledDialogue : MonoBehaviour {
	float timer;
	bool startTimer = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (startTimer) {
			if (timer <= 0) {
				startTimer = false;
				Fungus.SayDialog.activeSayDialog.Stop();
			}
			timer -= Time.deltaTime;
		}
	}

	// Enemy script calls this method
	public void TellFungusEnemyIsDead() {
		Debug.Log ("Tell fungus enemy is dead");
		Fungus.Flowchart.BroadcastFungusMessage ("EnemyDead");
		timer = 4.0f;
		startTimer = true;
	}
}
