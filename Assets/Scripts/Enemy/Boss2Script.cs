using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

public class Boss2Script : MonoBehaviour {
    public GameObject knight;
	public GameObject player;
    public GameObject fireball;
    public GameObject fallingObject;
    public GameObject rollingObject;
	public GameObject maincamera;
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
    float rollingInterval = 0.5f;
    float stage7Timer = 10f;
    float fallingInterval = 0.5f;
    float stage8Timer = 10f;
    float stage9Timer = 15f;
    float attackTimer = 0.5f;
    int attackStage = 6;
    float stage10Timer = 5f;
    float signalTimer = 2f;
    public int hp = 5;
    public GameObject column;
    public GameObject beam;
    public GameObject explosion;
    public GameObject signal;
    public GameObject signal2;

	[Header("Terrain")]
	public float distanceFromEdge;
	public float minDist = 300f; // distance when we instantiate path
	public int pathOffset = 1000; // Where to instantiate new path
	public GameObject path; // The BaseTerrain
    public GameObject bossHearts;

	[Header("Audio")]
    public AudioClip moo;
    public AudioClip dying;
	public AudioClip godzillaClip;
    AudioSource audioSrc;
	//AudioSource dramaticOpeningAudioSrc;
	//public AudioClip dramaticOpeningClip;
	//AudioSource godzillaAudioSrc;
	public bool doneWithOpening = false;

    // Use this for initialization
    void Start() {
        audioSrc = GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody>();
		path = GameObject.Find ("BossLevelTerrain");
        bossHearts = GameObject.Find("BossHearts");
		//dramaticOpeningAudioSrc = GameObject.Find ("DramaticOpeningAudio").GetComponent<AudioSource> ();
		//dramaticOpeningClip = dramaticOpeningAudioSrc.clip;
		//dramaticOpeningAudioSrc.Play ();
		//godzillaAudioSrc = GameObject.Find ("GodzillaAudio").GetComponent<AudioSource> ();
		//godzillaClip = godzillaAudioSrc.clip;
		player = GameObject.Find ("Knight");
		maincamera = GameObject.Find ("Main Camera");
		audioSrc.PlayOneShot (godzillaClip, 0.25f);
    }

    // Update is called once per frame
    void Update() {
		// Terrain drawing logic
		float edge = path.transform.position.z + pathOffset/2;
		distanceFromEdge = Mathf.Abs (this.gameObject.transform.position.z - edge);
		if (distanceFromEdge < minDist) {
			Vector3 newpos = new Vector3 (path.transform.position.x,
				path.transform.position.y, path.transform.position.z + pathOffset);
			path = Instantiate<GameObject> (path);
			path.transform.position = newpos;
		}

        //Chasing Logic
        switch (stage)
        {
			/*case -1:
				//Debug.Log (dramaticOpeningAudioSrc.timeSamples / dramaticOpeningClip.frequency);
				// seconds = (samples) * (seconds/sample)
				if (dramaticOpeningAudioSrc.timeSamples / dramaticOpeningClip.frequency > 12 && !godzillaAudioSrc.isPlaying) {
					godzillaAudioSrc.Play ();
					Fungus.Flowchart.BroadcastFungusMessage ("ShakeCamera");
				}
				if (godzillaAudioSrc.timeSamples / godzillaClip.frequency > 4) {
					doneWithOpening = true;
					Fungus.SayDialog.activeSayDialog.Stop ();
					player.GetComponent<MoveKnight> ().BeginGame ();
					stage = 0;
				}
				break;
				*/
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
                if (this.transform.position.z < MoveKnight.rigid.position.z)
                {
                    stage = 3;
                }
                break;
            case 3:
                stage3Timer -= Time.deltaTime;
                if (stage3Timer < 0)
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
                if (transform.position.x < MoveKnight.rigid.position.x + 1.5f && !(this.transform.position.z - MoveKnight.rigid.position.z >= chaseDistance))
                {
                    Vector3 pos1 = transform.position;
                    pos1.x = MoveKnight.rigid.position.x + 1.5f;
                    transform.position = pos1;
                }
                if (this.transform.position.z - MoveKnight.rigid.position.z >= chaseDistance)
                {
                    laneTimer = -1f;
                    signalTimer -= Time.deltaTime;
                    switchLane();
                    if (signalTimer < 0)
                    {
                        signal.SetActive(false);
                        signal2.SetActive(false);
                        signalTimer = 2f;
                        stage = attackStage;
                    } else
                    {
                        signal.SetActive(true);
                        signal2.SetActive(true);
                    }
                }
                break;
            case 6: //Bullet Hell Attack
                stage6Timer -= Time.deltaTime;
                fireInterval -= Time.deltaTime;
                Fire();
                laneInterval = 1f;
                laneTimer -= Time.deltaTime;
                switchLane();
                if (stage6Timer < 0)
                {
                    stage6Timer = 10f;
                    attackStage = 7;
                    laneTimer = 0.5f;
                    stage = 0;
                }
                break;
            case 7: //Falling Obstacle Attack
                stage7Timer -= Time.deltaTime;
                fallingInterval -= Time.deltaTime;
                fallingObstacle();
                laneInterval = 1f;
                laneTimer -= Time.deltaTime;
                switchLane();
                if (stage7Timer < 0)
                {
                    stage7Timer = 10f;
                    attackStage = 8;
                    laneTimer = 0.5f;
                    stage = 0;
                }
                break;
            case 8: //Rolling ball attack
                stage8Timer -= Time.deltaTime;
                rollingInterval -= Time.deltaTime;
                rollingBall();
                laneInterval = 1f;
                laneTimer -= Time.deltaTime;
                switchLane();
                if (stage8Timer < 0)
                {
                    stage8Timer = 10f;
                    attackStage = 6;
                    laneTimer = 0.5f;
                    stage = 0;
                }
                break;
            case 9: //Final hell stage and death
                stage9Timer -= Time.deltaTime;
                laneInterval = 0.3f;
                laneTimer -= Time.deltaTime;
                switchLane();
                attackTimer -= Time.deltaTime;
                if(attackTimer < 0)
                {
                    float decide = Random.Range(0, 3);
                    if(decide >= 0 && decide < 1)//Generate Knight
                    {
                        Vector3 pos1 = this.transform.position;
                        pos1.z -= 2f;
                        pos1.y = 2f;
                        knight.transform.position = pos1;
                        Instantiate(knight);
                        attackTimer = 0.8f;
                    }
                    else if(decide >= 1 && decide < 2)//Generate Column
                    {
                        Vector3 pos1 = this.transform.position;
                        pos1.z += 30f;
                        column.transform.position = pos1;
                        print(column.transform.position);
                        Instantiate(column);
                        attackTimer = 1.2f;
                    } else
                    {
                        Vector3 pos1 = this.transform.position;
                        pos1.z += 30f;
                        pos1.y = 0.8f;
                        pos1.x = 0;
                        beam.transform.position = pos1;
                        Instantiate(beam);
                        attackTimer = 1.2f;
                    }
                }
                if (stage9Timer < 0)
                {
                    stage9Timer = 10f;
                    stage = 10;
                    audioSrc.PlayOneShot(dying);

                }
                break;
            case 10: //Death
                chase();
                this.transform.Rotate(new Vector3(20f, 20f, 20f));
                laneInterval = 0.7f;
                laneTimer -= Time.deltaTime;
                switchLane();
                stage10Timer -= Time.deltaTime;
                if(stage10Timer < 0)
                {
                    stage = 11;
                }
                break;
            case 11: //Instantiate Explosions and Die
                explosion.transform.position = this.transform.position;
                Instantiate(explosion);
                Vector3 p = explosion.transform.position;
                p.x -= 1f;
                Instantiate(explosion);
                p.x += 2f;
                Instantiate(explosion);
                Fungus.Flowchart.BroadcastFungusMessage("LevelCleared");
                Destroy(this.gameObject);

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
            vel.z -= 20f;
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
            pos.z += 10f;
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
                audioSrc.PlayOneShot(moo);
                stage = 3;
                rigid.constraints = RigidbodyConstraints.None;
                Vector3 vel = rigid.velocity;
                vel.z = MoveKnight.rigid.velocity.z - 1f;
                vel.y += 8f;
                rigid.velocity = vel;
                float dir = (Random.Range(-2f, 2f));
                Vector3 angle = new Vector3(-1f, dir, 0f);
                rigid.angularVelocity = angle;
                hp--;
                if(hp >= 0)
                {
                    bossHearts.GetComponent<HeartsScriptBoss>().decreaseHealth();
                }
                if (hp <= 0)
                {
                    attackStage = 9;
                }
            } /*else
            {
                stage = 3;
            }*/
        }
    }

    void Fire()
    {
        if(fireInterval < 0)
        {
            Vector3 pos = this.transform.position;
            pos.y = 2f;
            fireball.transform.position = pos;
            Instantiate(fireball);
            fireInterval = 0.7f;
        }
    }

    void rollingBall()
    {
        if (rollingInterval < 0)
        {
            Vector3 pos = this.transform.position;
            pos.z += 15f;
            pos.y = 1.5f;
            rollingObject.transform.position = pos;
            Instantiate(rollingObject);
            rollingInterval = 0.9f;
        }
    }

    void fallingObstacle()
    {
        if (fallingInterval < 0)
        {
            Vector3 pos = this.transform.position;
            pos.z += 10f;
            pos.y = 6f;
            fallingObject.transform.position = pos;
            Instantiate(fallingObject);
            fallingInterval = 0.7f;
        }
    }
}
