using UnityEngine;
using System.Collections;

public class FadeScript : MonoBehaviour {
	Renderer rend;
	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A)) {
			Debug.Log ("Start fade");
			StartCoroutine("Fade");
		}
	}

	IEnumerator Fade() {
		for (float f = 1f; f >= 0f; f -= 0.1f) {
			Color c = rend.material.color;
			c.a = f;
			rend.material.color = c;
			Debug.Log ("red: " + rend.material.color.a);
			yield return null;
		}
	}
}
