using UnityEngine;
using System.Collections;




public class Pit : MonoBehaviour {

    public GameObject hp_bar;
    public CheckpointPit checkpoint;
    public GameObject player;
    public Animator horse;

    // Use this for initialization
    void Start () {
        hp_bar = GameObject.Find("Hearts");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Knight") {
            hp_bar.GetComponent<HeartsScript>().decreaseHealth();
            StartCoroutine(Respawn());

        }
    }

    IEnumerator Respawn() {

        //yield return new WaitForSeconds(2);
        Vector3 temp = Vector3.zero;
        temp.y = 1.5f;
        temp.z = checkpoint.lastZ;
        player.transform.position = temp;
        player.gameObject.GetComponent<MoveKnight>().state = 0;
        player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        MoveKnight.lane = 1;
        horse.enabled = false;
        yield return new WaitForSeconds(2);
        horse.enabled = true;
        player.gameObject.GetComponent<MoveKnight>().BeginGame();
        
    }

}
