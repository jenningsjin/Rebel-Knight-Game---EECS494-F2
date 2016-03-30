using UnityEngine;
using System.Collections;

public class Weapons : MonoBehaviour {

    public GameObject[] weapons;
    public int index = 0;

	// Use this for initialization
	void Start () {
        weapons[0].SetActive(true);
	    for (int i = 1; i < weapons.Length; i++) {
            weapons[i].SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.S)) {
            weapons[index].SetActive(false);
            if (index + 1 == weapons.Length)
                index = 0;
            else 
                index++;
            weapons[index].SetActive(true);

        }
    }


}
