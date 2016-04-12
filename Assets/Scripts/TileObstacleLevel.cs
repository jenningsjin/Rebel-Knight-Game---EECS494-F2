using UnityEngine;
using System.Collections;

public class TileObstacleLevel : MonoBehaviour {

	public GameObject [] groundInstances;

	// Use this for initialization
	void Start () {
		groundInstances = GameObject.FindGameObjectsWithTag ("Ground");
		for (int i = 0; i < groundInstances.Length; ++i) {
			groundInstances[i].GetComponent<Renderer> ().material.mainTextureScale =
				new Vector2 (groundInstances[i].transform.localScale.x, groundInstances[i].transform.localScale.z);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
