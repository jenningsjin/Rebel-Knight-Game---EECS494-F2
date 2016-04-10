using UnityEngine;
using System.Collections;

public class Boss2Script : MonoBehaviour {
    public GameObject knight;
    public GameObject fireball;
    public float chaseDistance = 10f;
    Rigidbody rigid;
    public int stage = 0;
    public float enemyInterval = 5f;
    public float knightTimer = 5f;
    float laneTimer = 5f;
    float stage2Timer = 1f;
    float stage3Timer = 3f;
    float fireInterval = 0.5f;
    float laneInterval = 5f;
    float stage6Timer = 10f;
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
                laneInterval = 5f;
                rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | rigid.constraints;
                knightTimer -= Time.deltaTime;
                laneTimer -= Time.deltaTime;
                GenerateKnight();
                switchLane();
                break;
            case 1: stage2Timer -= Time.deltaTime;
                if (stage2Timer < 0)
                {
                    stage2Timer = 1f;
                    stage = 2;
                }
                break;
            case 2: charge();
                if(this.transform.position.z < MoveKnight.rigid.position.z)
                {
                    stage = 3;
                }
                break;
            case 3:
                stage3Timer -= Time.deltaTime;
                if(stage3Timer < 0)
                {
                    stage3Timer = 3f;
                    stage = 4;
                }
                break;
            case 4: //Return boss to original position
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                Vector3 pos = MoveKnight.rigid.position;
                pos.y = 2f;
                pos.z -= 40f;
                pos.x += 1f;
                this.transform.position = pos;
                rigid.velocity = Vector3.zero;
                rigid.constraints = RigidbodyConstraints.FreezeRotation;
                stage = 5;
                this.gameObject.layer = LayerMask.NameToLayer("Boss");
                break;
            case 5:  //Return boss to original position
                chase();
                rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | rigid.constraints;
                if(this.transform.position.z - MoveKnight.rigid.position.z >= chaseDistance)
                {
                    laneTimer = -1f;
                    switchLane();
                    stage = 6;
                }
                break;
            case 6: //Bullet Hell Attack
                stage6Timer -= Time.deltaTime;
                fireInterval -= Time.deltaTime;
                Fire();
                laneInterval = 1f;
                laneTimer -= Time.deltaTime;
                switchLane();
                if(stage6Timer < 0)
                {
                    stage6Timer = 10f;
                    stage = 1;
                }
                break;
            case 7:
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
        else if(this.transform.position.z - MoveKnight.rigid.position.z > chaseDistance + 1 )
        {
            Vector3 vel = MoveKnight.rigid.velocity;
            vel.z -= 2f;
            rigid.velocity = vel;
        } else
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
            laneTimer = laneInterval;
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
                stage = 3;
                rigid.constraints = RigidbodyConstraints.None;
                Vector3 vel = rigid.velocity;
                vel.z = MoveKnight.rigid.velocity.z - 1f;
                vel.y += 8f;
                rigid.velocity = vel;
                float dir = (Random.Range(-2f, 2f));
                Vector3 angle = new Vector3(-1f, dir, 0f);
                rigid.angularVelocity = angle;
            } else
            {
                stage = 3;
            }
        }
    }

    void Fire()
    {
        if(fireInterval < 0)
        {
            Vector3 pos = this.transform.position;
            pos.y = 1f;
            fireball.transform.position = pos;
            Instantiate(fireball);
            fireInterval = 0.7f;
        }
    }
}
