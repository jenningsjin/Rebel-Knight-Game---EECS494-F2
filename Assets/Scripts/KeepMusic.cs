// Assumptions: no more than one track can play in a scene
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KeepMusic : MonoBehaviour {
	public AudioClip [] audioClips;
	public enum Track {FragmentsOfTime, BattleCry, ThroughTheGates, OldEnglishMarch};
	bool [] isCurrentlyPlaying;
	public AudioSource audioSource;
	public float fadeSpeed = 1.5f;
	public int state = 0;

	void Awake() {
		if (GameObject.FindGameObjectsWithTag("MenuAudio").Length > 1) {
			Debug.Log ("Okay, main menu audio is already playing. Exit");
			Destroy (this.gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
		audioSource = this.gameObject.GetComponent<AudioSource> ();
		isCurrentlyPlaying = new bool[4];
		setAllTracksToNotPlaying ();
	}
		
	// Use this for initialization
	void Start () {
		audioSource.PlayOneShot(audioClips[(int) Track.FragmentsOfTime]);
		isCurrentlyPlaying [(int)Track.FragmentsOfTime] = true;
	}
	
	// Update is called once per frame
	void Update () {
		Scene currentScene = SceneManager.GetActiveScene ();
		switch (state) {
		case 0: // Playing a track
			// If the right thing isn't playing, do something!
			if (currentScene.name == "Prologue" && !isCurrentlyPlaying [(int)Track.BattleCry]) {
				Debug.Log ("In the prologue, but not playing prologue theme!");
				++state;
			} else if ((currentScene.name == "Menu" || currentScene.name == "Menu_LevelSelect") &&
			           !isCurrentlyPlaying [(int)Track.FragmentsOfTime]) {
				++state;
			}
			break;
		case 1: // Fading out a track
			if (audioSource.volume > 0) {
				Debug.Log ("Decreasing volume...");
				audioSource.volume -= fadeSpeed * Time.deltaTime;
			} else {
				++state;
			}
			break;
		case 2: // Selecting a new track
			audioSource.Stop();
			if (currentScene.name == "Prologue") {
				Debug.Log ("Choosing prologue track");
				setAllTracksToNotPlaying();
				isCurrentlyPlaying [(int)Track.BattleCry] = true;
				audioSource.PlayOneShot (audioClips [1]);
			} else if (currentScene.name == "Menu" || currentScene.name == "Menu_LevelSelect") {
				setAllTracksToNotPlaying ();
				isCurrentlyPlaying [(int)Track.FragmentsOfTime] = true;
				audioSource.PlayOneShot (audioClips [(int)Track.FragmentsOfTime]);
			}
			++state;
			break;
		case 3: // Fading in the new track
			if (audioSource.volume < 1) {
				audioSource.volume += fadeSpeed * Time.deltaTime;
			} else {
				state = 0;
			}
			break;
		default:
			Debug.Log ("Audio in an unknown state");
			break;
		}
	}

	void setAllTracksToNotPlaying() {
		for (int i = 0; i < isCurrentlyPlaying.Length; ++i) {
			isCurrentlyPlaying [i] = false;
		}
	}
}
