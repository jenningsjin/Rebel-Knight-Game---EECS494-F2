using UnityEngine;
using System.Collections;

public class Weapons : MonoBehaviour {

    public GameObject[] weapons;
    public int index = 0;
    public int size = 1;

	// Use this for initialization
	void Start () {
        weapons[0].SetActive(true);
	    for (int i = 1; i < size; i++) {
            weapons[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.S)) {
            weapons[index].SetActive(false);
            index = (index + 1) % size;
            weapons[index].SetActive(true);
        }
	}


}
