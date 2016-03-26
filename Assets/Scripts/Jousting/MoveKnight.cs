﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveKnight : MonoBehaviour {
    Rigidbody rigid;
    public GameObject explosion;
	public int state;
    public bool start = false;
    public bool end = false;
    //bool win = false;
	bool sentMsg = false;
    //float time = 5f;
    // Use this for initialization
    void Start () {
        rigid = GetComponentInChildren<Rigidbody>();
		state = 0;
    }

	public void BeginGame() {
		++state;
	}

	// Update is called once per frame
	void Update () {
		switch (state) {
		case 0: // Before game start
			break;
		case 1: // Charge
			rigid.AddForce(Vector3.forward * 20f);
			Vector3 vel = rigid.velocity;
			if (Input.GetKey(KeyCode.LeftArrow) && vel.x > -5f) {
				vel.x -= 0.5f;
			} else if (Input.GetKey(KeyCode.RightArrow) && vel.x < 5f) {
				vel.x += 0.5f;
			}
			rigid.velocity = vel;
			break;
		case 2: // After crossing the finish line
			rigid.constraints = RigidbodyConstraints.FreezeAll;
			if (!sentMsg) {
				Fungus.Flowchart.BroadcastFungusMessage ("LevelCleared");
				sentMsg = true;
			}
			break;
		default:
			Debug.Log ("Unrecognized state");
			break;
		}
    }

    void OnCollisionEnter(Collision collision)
    {
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
}
