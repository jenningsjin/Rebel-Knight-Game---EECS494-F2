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

	public void increaseSpeedDial() {
		Debug.Log ("Increasing speed dial");
		if (index < 11) {
			++index;
			gameObject.GetComponent<Image>().sprite = speedDials [index];
		}
	}

	public void decreaseSpeedDial() {
		Debug.Log ("Decreasing speed dial");
		if (index > 0) {
			--index;
			gameObject.GetComponent<Image> ().sprite = speedDials [index];
		}
	}
}
