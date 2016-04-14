using UnityEngine;
using System.Collections;

public class CheckpointPit : MonoBehaviour {
    public GameObject[] checkpoints;
    public float lastZ = 5;
    public GameObject player;
	// Use this for initialization
	void Start () {
        //lastZ = 5;


    }
	
	// Update is called once per frame
	void Update () {
	    if (player.transform.position.z > checkpoints[0].transform.position.z) {
            lastZ = checkpoints[0].transform.position.z;
        }
        if (player.transform.position.z > checkpoints[1].transform.position.z) {
            lastZ = checkpoints[1].transform.position.z;
        }
	}



}
