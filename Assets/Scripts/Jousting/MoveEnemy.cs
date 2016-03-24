using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

    Rigidbody rigid;
    public GameObject explosion;
    public bool start = false;
    bool end = false;
    bool win = false;
    float time = 5f;
    // Use this for initialization
    void Start()
    {
        rigid = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            start = true;
        }
        if (start && !end)
        {
                rigid.AddForce(Vector3.back * 10f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Main Camera")
        {
            rigid.constraints = RigidbodyConstraints.None;
            explosion.transform.position = collision.transform.position;
            Instantiate<GameObject>(explosion);
            end = false;
        }
    }
}
