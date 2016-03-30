using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveKnight : MonoBehaviour {
    public static Rigidbody rigid;
    public GameObject explosion;
	public int state;
	public GameObject hp_bar;
	public int jumpSpeed = 100;
	public BoxCollider groundCollider;
    private float moveTimer = 0.1f;
    private int lane = 1;

	bool sentMsg = false;
	bool grounded = true;
    // Use this for initialization
    void Start () {
        rigid = GetComponent<Rigidbody>();
		state = 0;
		hp_bar = GameObject.Find ("HP_Bar");
    }

	public void BeginGame() {
		++state;
	}

	// Update is called once per frame
	void Update () {
        moveTimer -= Time.deltaTime;
		switch (state) {
		case 0: // Before game start
			break;
		case 1: // Charge
			if( rigid.velocity.z < 30) {
				rigid.AddForce (Vector3.forward * 40f);
			}
			Vector3 vel = rigid.velocity;
			//Vector3 tmp = rigid.rotation.eulerAngles;
			if (Input.GetKey (KeyCode.LeftArrow) && moveTimer < 0 && lane > 0) {
                    Vector3 pos = this.transform.position;
                    pos.x -= 4f;
                    rigid.MovePosition(pos);
                    moveTimer = 0.1f;
                    lane--;
				//tmp.y -= 0.5f;
			} 
			else if (Input.GetKey (KeyCode.RightArrow) && moveTimer < 0 && lane < 2) {
                    Vector3 pos = this.transform.position;
                    pos.x += 4f;
                    rigid.MovePosition(pos);
                    moveTimer = 0.1f;
                    lane++;
                    //tmp.y += 0.5f;
                }
			else if (Input.GetKey (KeyCode.UpArrow) && grounded) {
				rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
				Debug.Log("Jumping");
				grounded = false;
			}
			//rigid.rotation = Quaternion.Euler (tmp.x, tmp.y, tmp.z);
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

	void OnCollisionEnter(Collision col) {
		//print ("Player body collided with something");
		// Contact points: every contact stores a contact point and the two colliders
		// involved.
		foreach (ContactPoint c in col.contacts) {
			//print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
			if (c.thisCollider.name == "VulnerableArea" && c.otherCollider.name == "EnemyLance" ||
				c.thisCollider.name == "EnemyLance" && c.otherCollider.name == "VulnerableArea") {
				print ("Contact " + c.thisCollider.name + " hit " + c.otherCollider.name);
				hp_bar.GetComponent<HealthBar> ().decreaseHealth ();
			}
		}
	}

	//Groundedcheck
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Ground") {
			grounded = true;
		}
	}

	// TODO: consider moving this elsewhere
	public void LoadMenu() {
		SceneManager.LoadScene ("Menu");
	}
}
