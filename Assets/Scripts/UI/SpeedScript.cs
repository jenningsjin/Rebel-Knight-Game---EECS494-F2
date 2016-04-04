using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedScript : MonoBehaviour {
	public int index;
	public Sprite [] speedDials;

	// Use this for initialization
	void Start () {
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateSpeedDial() {
		Debug.Log ("Updating speed dial");
		if (index < 11) {
			++index;
			gameObject.GetComponent<Image>().sprite = speedDials [index];
		}
	}
}
