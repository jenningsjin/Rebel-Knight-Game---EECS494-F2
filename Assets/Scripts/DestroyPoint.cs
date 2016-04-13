using UnityEngine;
using System.Collections;

public class DestroyPoint : MonoBehaviour {
    public int zVal;
    public GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (player.transform.position.z > zVal) {
            Destroy(this.gameObject);
        }
	}
}
