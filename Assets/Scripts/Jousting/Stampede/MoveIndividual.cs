using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class MoveIndividual : MonoBehaviour {

    Rigidbody rigid;
    public GameObject explosion;
    public int state;
    bool sentMsg = false;
    //public GameObject hp_bar;

    // Use this for initialization
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }


    void Start()
    {
        state = 0;
        //hp_bar = GameObject.Find("HP_Bar");
    }

    public void BeginGame()
    {
        ++state;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            state = 1;
        }
        switch (state)
        {
            case 0: // Before game start
                break;
            case 1: // Charge
                rigid.AddForce(Vector3.forward * 20f);
                Vector3 vel = rigid.velocity;
                if (Input.GetKey(KeyCode.LeftArrow) && vel.x > -5f)
                {
                    vel.x -= 0.5f;
                }
                else if (Input.GetKey(KeyCode.RightArrow) && vel.x < 5f)
                {
                    vel.x += 0.5f;
                }
                rigid.velocity = vel;
                break;
            case 2: // After crossing the finish line
                rigid.constraints = RigidbodyConstraints.FreezeAll;
                if (!sentMsg)
                {
                    Fungus.Flowchart.BroadcastFungusMessage("LevelCleared");
                    sentMsg = true;
                }
                break;
            default:
                Debug.Log("Unrecognized state");
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        rigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //hp_bar.GetComponent<HealthBar>().decreaseHealth();
        }
    }

    // TODO: consider moving this elsewhere
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
