using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	bool paused = false;
	string currentSceneName;
	GameObject knight;
	GameObject boss;
	int oldBossPhase;
	GameObject instructions;

	// Use this for initialization
	void Start () {
		Debug.Log ("paused == " + paused);
		Debug.Log ("this.gameObject.name == " + this.gameObject.name);
		instructions = GameObject.Find ("InstructionsCanvas");
		if (instructions) {
			Debug.Log ("Found instructions canvas");
			instructions.SetActive (false);
		} else {
			Debug.Log ("COULDN'T FIND INSTRUCTIONS CANVAS");
		}
	}
	
	// Update is called once per frame
	void Update () {
		currentSceneName = SceneManager.GetActiveScene ().name;
		if (Input.GetKeyDown (KeyCode.P) && !paused &&
			(currentSceneName == "JoustTutorial" || currentSceneName == "Level1" ||
			currentSceneName == "BossLevel1" || currentSceneName == "Level2" ||
			currentSceneName == "BossLevel2")) {
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
			oldBossPhase = boss.GetComponent<CarpetBossScript> ().bossPhase;
			boss.GetComponent<CarpetBossScript> ().bossPhase = 0;
		}
		if (currentSceneName == "BossLevel2") {
			boss = GameObject.Find ("Boss");
			oldBossPhase = boss.GetComponent<Boss2Script> ().stage;
			boss.GetComponent<Boss2Script> ().stage = -1;
		}
		instructions.SetActive (true);
		Time.timeScale = 0.000000001f;

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
			boss.GetComponent<CarpetBossScript> ().bossPhase = oldBossPhase;
		}
		if (currentSceneName == "BossLevel2") {
			boss.GetComponent<Boss2Script> ().stage = oldBossPhase;
		}
	}

	// Scene loading
	public void LoadMainMenu() {
		SceneManager.LoadScene("Menu");
	}

	public void SelectLevel() {
		SceneManager.LoadScene ("Menu_LevelSelect");
	}


	public void LoadGameOver() {
		SceneManager.LoadScene ("GameOver");
	}
		
	public void ReadPrologue() {
		SceneManager.LoadScene ("Prologue");
	}

	public void DisplayInstructions() {
		SceneManager.LoadScene ("Instructions");
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
		
	public void LoadBossLevel1() {
		SceneManager.LoadScene ("BossLevel1");
	}

	public void LoadBossLevel1Cutscene() {
		SceneManager.LoadScene ("BossLevel1Cutscene");
	}

    public void LoadLevel2Cutscene()
    {
        SceneManager.LoadScene("Level2Cutscene");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadBossLevel2Cutscene()
    {
        SceneManager.LoadScene("BossLevel2Cutscene");
    }

    public void LoadBossLevel2()
    {
        SceneManager.LoadScene("BossLevel2");
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene("EndCutscene");
    }
}
