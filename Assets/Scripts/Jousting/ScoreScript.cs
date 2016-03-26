using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour {
	public int score;
	public Text scoreText;

	// Use this for initialization
	void Start () {
		score = 0;
		scoreText = this.gameObject.GetComponent<Text> ();
		scoreText.text = "Score: 0";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateScore() {
		score += 10;
		scoreText.text = "Score: " + score.ToString ();
	}
}
