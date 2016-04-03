using UnityEngine;
using System.Collections;




public class Pit : MonoBehaviour {

    public GameObject hp_bar;

    // Use this for initialization
    void Start () {
        hp_bar = GameObject.Find("Hearts");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Knight") {
            hp_bar.GetComponent<HeartsScript>().killPlayer();
        }
    }
    
}
