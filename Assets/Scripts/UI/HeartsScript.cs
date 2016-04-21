using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartsScript : MonoBehaviour {
	public int index;
	public GameObject player;
	public bool sentMsg = false;
	public Sprite [] hp_sprites;
    bool dead = false;
	// Use this for initialization
	void Start () {
		index = 0;
        player = GameObject.Find("Knight");
	}
	
	// Update is called once per frame
	void Update () {
        if (dead)
        {
            //MoveKnight.rigid.transform.Rotate(Vector3.left);
            MoveKnight.rigid.AddForce(Vector3.up * 1500f);
            MoveKnight.rigid.angularVelocity = new Vector3(3, 1);
            dead = false;
        }
	}

	public void decreaseHealth() {
		++index;
		if (index >= 5) { // Game Over
                          // Freeze the camera, disable player movement, and send a message
                          // to Fungus, so that Fungus displays the defeat Flowchart.
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            dead = true;
			player.GetComponent<MoveKnight> ().state = 0;
            player.GetComponent<MoveKnight>().animator.SetBool("isRunning", false);
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
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        player.GetComponent<MoveKnight>().state = 0;
        dead = true;
        print("HAHAHAHAHA");
        if (!sentMsg) {
            Fungus.Flowchart.BroadcastFungusMessage("Defeat");
            sentMsg = true;
        }
    }
}
