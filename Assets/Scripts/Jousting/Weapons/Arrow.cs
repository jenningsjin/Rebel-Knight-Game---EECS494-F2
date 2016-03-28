using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
    Rigidbody arrow;
	// Use this for initialization
	void Start () {
	    arrow = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        arrow.velocity = MoveKnight.rigid.velocity * 3f;
    }

    void OnCollisionEnter(Collision c) {
        print("arrows");
    }
}
