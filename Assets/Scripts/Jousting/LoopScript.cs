using UnityEngine;
using System.Collections;

public class LoopScript : MonoBehaviour {

	public GameObject itemToLoop;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		print(col.gameObject.name);
		itemToLoop.transform.position = new Vector3(itemToLoop.transform.position.x, itemToLoop.transform.position.y , 0f);

	}
}
