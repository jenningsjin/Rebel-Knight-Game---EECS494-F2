using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ReadPrologue() {
		SceneManager.LoadScene ("Prologue");
	}

	public void StartTutorial() {
		SceneManager.LoadScene ("JoustTutorial");
	}

	public void SelectLevel() {
		SceneManager.LoadScene ("Menu_LevelSelect");
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene("Menu");
	}
}
