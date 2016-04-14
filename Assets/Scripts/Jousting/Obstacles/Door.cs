using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    public float speed = 1;
    public Vector3 vel;
    Rigidbody rigid;
    public bool spin = false;
    private float rot = 2f;
	// Use this for initialization
	void Start () {
        rigid = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        MoveWall();
	} 

    void MoveWall() {
        rigid.velocity = vel * speed;
        if (spin)
        {
            if (rigid.velocity.x > 0)
            {
                this.transform.Rotate(Vector3.down);
            }
            else
            {
                this.transform.Rotate(Vector3.up);
            }
        }
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Switch") {
            speed *= -1;
        }
        else if (c.gameObject.tag == "Knight") {
            Debug.Log("hit player");
        }
        
    }
}
