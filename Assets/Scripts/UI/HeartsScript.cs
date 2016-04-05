using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartsScript : MonoBehaviour {
	public int index;
	public GameObject player;
	public bool sentMsg = false;
	public Sprite [] hp_sprites;

	// Use this for initialization
	void Start () {
		index = 0;
        player = GameObject.Find("Knight");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void decreaseHealth() {
		++index;
		if (index >= 5) { // Game Over
			// Freeze the camera, disable player movement, and send a message
			// to Fungus, so that Fungus displays the defeat Flowchart.
			player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			player.GetComponent<MoveKnight> ().state = 0;
			if (!sentMsg) {
				Fungus.Flowchart.BroadcastFungusMessage ("Defeat");
				sentMsg = true;
			}
		}
		if (index <= 5) {
			gameObject.GetComponent<Image> ().sprite = hp_sprites [index];
		}
	}

    public void killPlayer() {
        index = 5;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        player.GetComponent<MoveKnight>().state = 0;
        if (!sentMsg) {
            Fungus.Flowchart.BroadcastFungusMessage("Defeat");
            sentMsg = true;
        }
    }
}
