using UnityEngine;
using System.Collections;

public class Boss2Script : MonoBehaviour {
    public GameObject knight;
    public float chaseDistance = 10f;
    Rigidbody rigid;
    public int stage = 0;
    public float enemyInterval = 5f;
    public float knightTimer = 5f;
    public float laneTimer = 5f;
    float stage2Timer = 2f;
    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        //Chasing Logic
        switch (stage)
        {
            case 0: chase();
                rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | rigid.constraints;
                knightTimer -= Time.deltaTime;
                laneTimer -= Time.deltaTime;
                GenerateKnight();
                switchLane();
                break;
            case 1: stage2Timer -= Time.deltaTime;
                if(stage2Timer < 0)
                {
                    stage2Timer = 2f;
                    stage = 2;
                }
                break;
            case 2: charge();
                break;
        }

    }

    void chase()
    {
        if (this.transform.position.z - MoveKnight.rigid.position.z < chaseDistance)
        {
            Vector3 vel = MoveKnight.rigid.velocity;
            vel.z += 20f;
            rigid.velocity = vel;
        }
        else
        {
            Vector3 vel = MoveKnight.rigid.velocity;
            rigid.velocity = vel;
        }
    }

    void charge()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Enemy");
        Vector3 vel = rigid.velocity;
        vel.z = -20f;
        rigid.velocity = vel;
    }

    void GenerateKnight()
    {
        if(knightTimer < 0)
        {
            Vector3 pos = this.transform.position;
            pos.z -= 2f;
            pos.y = 2f;
            knight.transform.position = pos;
            Instantiate(knight);
            knightTimer = enemyInterval;
        }
    }

    void switchLane()
    {
        if(laneTimer < 0)
        {
            Vector3 pos = this.transform.position;
            pos.x = MoveKnight.lanePosition();
            this.transform.position = pos;
            laneTimer = 5f;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        print(col.gameObject.name);
        if(col.gameObject.tag == "Weapon")
        {
            if(stage == 0)
            {
                stage = 1;
            }
        }
        if(col.gameObject.name == "Knight" && stage == 2)
        {
            if (MoveKnight.lanceReady)
            {
                stage = 2;
                rigid.constraints = RigidbodyConstraints.None;
                Vector3 vel = rigid.velocity;
                vel.z = MoveKnight.rigid.velocity.z + 12f;
                if (vel.z < 32)
                {
                    vel.z = 32f;
                }
                vel.y += 8f;
                rigid.velocity = vel;
                float dir = (Random.Range(-2f, 2f));
                Vector3 angle = new Vector3(-1f, dir, 0f);
                rigid.angularVelocity = angle;
            } else
            {

            }
        }
    }
}
