using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeartsScriptBoss : MonoBehaviour {
	public int index;
	public Sprite [] hp_sprites;

	// Use this for initialization
	void Start () {
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void decreaseHealth() {
		++index;
		if (index <= 5) {
			gameObject.GetComponent<Image> ().sprite = hp_sprites [index];
		}
	}
}
