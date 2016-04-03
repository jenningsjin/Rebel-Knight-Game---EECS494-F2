using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KeepMusic : MonoBehaviour {
	public AudioClip audioclip;

	void Awake() {
		if (GameObject.FindGameObjectsWithTag("MenuAudio").Length > 1) {
			Debug.Log ("Okay, main menu audio is already playing. Exit");
			Destroy (this.gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
	}
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<AudioSource>().PlayOneShot(audioclip);
	}
	
	// Update is called once per frame
	void Update () {
		Scene currentScene = SceneManager.GetActiveScene ();
		if (currentScene.name != "Menu" && currentScene.name != "Menu_LevelSelect") {
			Destroy (this.gameObject);
		}
	}
}
