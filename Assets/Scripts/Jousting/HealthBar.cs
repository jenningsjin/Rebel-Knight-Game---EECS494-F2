using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public float maxHealth = 100f;
	public float currentHealth;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		InvokeRepeating("decreaseHealth", 1f, 1f);
	}

	// Update is called once per frame
	void Update () {

	}

	void decreaseHealth() {
		Debug.Log ("Decreasing health");
		if (currentHealth > 0) {
			currentHealth -= 10f;
			float scaledHealthVal = currentHealth / maxHealth;
			UpdateHealthBar (scaledHealthVal);
		}
	}

	public void UpdateHealthBar(float currentHP) {
		// Health 
		this.gameObject.transform.localScale = new Vector3(
			currentHP,
			this.gameObject.transform.localScale.y,
			this.gameObject.transform.localScale.z);
	}
}
