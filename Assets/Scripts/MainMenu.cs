using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	bool paused = false;
	string currentSceneName;
	GameObject knight;
	GameObject boss;
	int oldBoss1Phase;
	GameObject instructions;

	// Use this for initialization
	void Start () {
		instructions = GameObject.Find ("InstructionsCanvas");
		instructions.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.P) && !paused) {
			paused = true;
			pauseGame ();
		} else if (Input.GetKeyDown (KeyCode.P) && paused) {
			paused = false;
			restartGame ();
		}
	}

	void pauseGame()
	{
		Debug.Log ("Game Paused");
		instructions.SetActive (true);
		Time.timeScale = 0.000000001f;
		currentSceneName = SceneManager.GetActiveScene ().name;
		// All levels that can be paused are guaranteed to have a knight
		knight = GameObject.Find ("Knight"); // returns null if not found
		if (currentSceneName == "JoustTutorial" || currentSceneName == "Level1" ||
		    currentSceneName == "BossLevel1" || currentSceneName == "Level2" ||
		    currentSceneName == "BossLevel2") {
			knight.GetComponent<MoveKnight> ().state = 0;
			knight.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			knight.GetComponent<MoveKnight> ().animator.SetBool ("isRunning", false);
		}
		if (currentSceneName == "BossLevel1") {
			boss = GameObject.Find ("Boss");
			oldBoss1Phase = boss.GetComponent<CarpetBossScript> ().bossPhase;
			boss.GetComponent<CarpetBossScript> ().bossPhase = 0;
		}

	}

	void restartGame()
	{
		Debug.Log ("Game Restarted");
		instructions.SetActive (false);
		Time.timeScale = 1;
		currentSceneName = SceneManager.GetActiveScene ().name;
		// All levels that can be paused are guaranteed to have a knight
		knight = GameObject.Find ("Knight"); // returns null if not found
		if (currentSceneName == "JoustTutorial" || currentSceneName == "Level1" ||
			currentSceneName == "BossLevel1" || currentSceneName == "Level2" ||
			currentSceneName == "BossLevel2") {
			knight.GetComponent<MoveKnight> ().state = 1;
			knight.GetComponent<MoveKnight> ().animator.SetBool ("isRunning", true);
		}
		if (currentSceneName == "BossLevel1") {
			// We wouldn't be calling restart unless the game was already paused!
			// Don't need to re-find the boss.
			boss.GetComponent<CarpetBossScript> ().bossPhase = oldBoss1Phase;
		}
	}

	// Scene loading
	public void ReadPrologue() {
		SceneManager.LoadScene ("Prologue");
	}

	public void StartTutorial() {
		SceneManager.LoadScene ("JoustTutorial");
	}

	public void LoadLevel1() {
		SceneManager.LoadScene ("Level1");
	}

	public void LoadLevel1Cutscene() {
		SceneManager.LoadScene("Level1Cutscene");
	}

	public void SelectLevel() {
		SceneManager.LoadScene ("Menu_LevelSelect");
	}

	public void LoadGameOver() {
		SceneManager.LoadScene ("GameOver");
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene("Menu");
	}
		
	public void LoadBossLevel1() {
		SceneManager.LoadScene ("BossLevel1");
	}

	public void LoadBossLevel1Cutscene() {
		SceneManager.LoadScene ("BossLevel1Cutscene");
	}
}
