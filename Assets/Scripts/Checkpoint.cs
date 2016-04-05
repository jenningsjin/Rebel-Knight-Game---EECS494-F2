using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public GameObject ground;
	public Transform t;
	public Vector3 pos;
	// The offset in BossLevel1 is ground.scale (5) * BaseTerrain.scale (200)
	public int offset;
	public bool visited = false;
	// Use this for initialization
	void Start () {
		ground = GameObject.Find ("BaseTerrain");
		t = ground.transform;
		pos = ground.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider c) {
		if (!visited) {
			Vector3 newpos = new Vector3 (pos.x, pos.y, pos.z + offset);
			Instantiate (ground, newpos, t.rotation);
		}
		Debug.Log ("Reached CHECKPOINT");
		visited = true;
	}
}
