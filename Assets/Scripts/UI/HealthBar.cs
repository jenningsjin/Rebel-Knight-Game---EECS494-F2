using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour {

	public float maxHealth = 100f;
	public float currentHealth;
	public GameObject cam;
	public bool sentMsg = false;
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		cam = GameObject.Find ("Main Camera");
		//InvokeRepeating("decreaseHealth", 1f, 1f);
	}

	// Update is called once per frame
	void Update () {
        
	}

	public void decreaseHealth() {
		//Debug.Log ("Decreasing health");
		if (currentHealth > 0) {
			currentHealth -= 10f;
			float scaledHealthVal = currentHealth / maxHealth;
			UpdateHealthBar (scaledHealthVal);
		} else {
			cam.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			cam.GetComponent<MoveKnight> ().state = 0;
			if (!sentMsg) {
				Fungus.Flowchart.BroadcastFungusMessage ("Defeat");
				sentMsg = true;
			}
		}
	}

	public void UpdateHealthBar(float currentHP) {
		// Health 
		this.gameObject.transform.localScale = new Vector3(
			currentHP,
			this.gameObject.transform.localScale.y,
			this.gameObject.transform.localScale.z);
        BoidController.flockSize /= 2;
	}
}
