// Assumptions: no more than one track can play in a scene
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KeepMusic : MonoBehaviour {
	public AudioClip [] audioClips;
	public enum Track {MenuMusic, BattleCry, ThroughTheGates, OldEnglishMarch, ExtraCredit, Maelstrom, Warhammer};
	bool [] isCurrentlyPlaying;
	public AudioSource audioSource;
	//public float fadeSpeed = 50f;
	public float fadeOutSpeed = 50f;
	public float fadeInSpeed = 10f;
	public int state = 0;
	//public float timer = 3.0f;

	void Awake() {
		if (GameObject.FindGameObjectsWithTag("MenuAudio").Length > 1) {
			Debug.Log ("Okay, main menu audio is already playing. Exit");
			Destroy (this.gameObject);
		}
		DontDestroyOnLoad (this.gameObject);
		audioSource = this.gameObject.GetComponent<AudioSource> ();
		isCurrentlyPlaying = new bool[audioClips.Length];
		setAllTracksToNotPlaying ();
	}
		
	// Use this for initialization
	void Start () {
		audioSource.PlayOneShot(audioClips[(int) Track.MenuMusic]);
		isCurrentlyPlaying [(int)Track.MenuMusic] = true;
	}
	
	// Update is called once per frame
	void Update () {
		Scene currentScene = SceneManager.GetActiveScene ();
		switch (state) {
		case 0: // Playing a track
			// If the right thing isn't playing, do something!
			if ((currentScene.name == "Prologue" || currentScene.name == "JoustTutorial") &&
			    !isCurrentlyPlaying [(int)Track.BattleCry]) {
				Debug.Log ("In the prologue, but not playing prologue theme!");
				++state;
			} else if ((currentScene.name == "BossLevel1" || currentScene.name == "BossLevel1Cutscene") &&
			           !isCurrentlyPlaying [(int)Track.OldEnglishMarch]) {
				++state;
			} else if ((currentScene.name == "Menu" || currentScene.name == "Menu_LevelSelect") &&
				!isCurrentlyPlaying [(int)Track.MenuMusic]) {
				++state;
			} else if (currentScene.name == "BossLevel2" && !isCurrentlyPlaying [(int)Track.Maelstrom]) {
				++state;
			} else if (currentScene.name == "Level2" && !isCurrentlyPlaying [(int)Track.Warhammer]) {
				++state;
			} else if (currentScene.name == "Level2Cutscene" && !isCurrentlyPlaying [(int)Track.ExtraCredit]) {
				++state;
			}
			break;
		case 1: // Fading out a track (3s)
			if (audioSource.volume > 0) {
				//Debug.Log ("Decreasing volume...");
				audioSource.volume -= fadeOutSpeed * Time.deltaTime;
			} else {
				++state;
			}
			break;
		case 2: // Selecting a new track
			audioSource.Stop ();
			setAllTracksToNotPlaying ();
			if (currentScene.name == "Prologue" || currentScene.name == "JoustTutorial") {
				Debug.Log ("Choosing prologue track");
				isCurrentlyPlaying [(int)Track.BattleCry] = true;
				audioSource.PlayOneShot (audioClips [(int)Track.BattleCry]);
			} else if (currentScene.name == "BossLevel1Cutscene" || currentScene.name == "BossLevel1") {
				isCurrentlyPlaying [(int)Track.OldEnglishMarch] = true;
				audioSource.PlayOneShot (audioClips [(int)Track.OldEnglishMarch]);
			} else if (currentScene.name == "Menu" || currentScene.name == "Menu_LevelSelect") {
				isCurrentlyPlaying [(int)Track.MenuMusic] = true;
				audioSource.PlayOneShot (audioClips [(int)Track.MenuMusic]);
			} else if (currentScene.name == "BossLevel2") {
				isCurrentlyPlaying [(int)Track.Maelstrom] = true;
				audioSource.PlayOneShot (audioClips [(int)Track.Maelstrom]);
				/*
				GameObject boss = GameObject.Find ("Boss");
				if (boss.GetComponent<Boss2Script> ().doneWithOpening) {
					isCurrentlyPlaying [(int)Track.Maelstrom] = true;
					audioSource.PlayOneShot (audioClips [(int)Track.Maelstrom]);
				}
				*/
			} else if (currentScene.name == "Level2") {
				isCurrentlyPlaying [(int)Track.Warhammer] = true;
				audioSource.PlayOneShot (audioClips [(int)Track.Warhammer]);
			} else if (currentScene.name == "Level2Cutscene") {
				isCurrentlyPlaying [(int)Track.ExtraCredit] = true;
				audioSource.PlayOneShot (audioClips [(int)Track.ExtraCredit]);
			}
			++state;
			break;
		case 3: // Fading in the new track (3s)
			if (audioSource.volume < 1) {
				audioSource.volume += fadeInSpeed * Time.deltaTime;
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
