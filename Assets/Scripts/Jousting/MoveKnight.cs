using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveKnight : MonoBehaviour {
    Rigidbody rigid;
    public GameObject explosion;
    public bool start = false;
    public bool end = false;
    bool win = false;
    float time = 5f;
    // Use this for initialization
    void Start () {
        rigid = GetComponentInChildren<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            start = true;
        }
        if (start)
        {
                rigid.AddForce(Vector3.forward * 20f);
        }
        if (end)
        {
			rigid.constraints = RigidbodyConstraints.FreezeAll;
            time -= Time.deltaTime;
            if(time < 0)
            {
                //End game
                if (win)
                {

                } else
                {
                    //lost

                }

            }
        }
        Vector3 vel = rigid.velocity;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if(vel.x > -5f)
            {
                vel.x -= 0.5f;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if(vel.x < 5f)
            {
                vel.x += 0.5f;
            }
        }
        rigid.velocity = vel;
    }

    void OnCollisionEnter(Collision collision)
    {
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }
}
