using UnityEngine;
using System.Collections;

public class ButtonExtras : MonoBehaviour {
	public AudioClip buttonSound;
	public AudioSource src;

	// Use this for initialization
	void Start () {
		src = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void MakeHoverSound() {
		Debug.Log ("HELLO");
		src.PlayOneShot (buttonSound);
	}
}
