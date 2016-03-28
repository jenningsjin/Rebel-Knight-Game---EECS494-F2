using UnityEngine;
using System.Collections;

public class Arrows : MonoBehaviour {

    public GameObject arrow;
    public GameObject bow;
    public Vector3 bowPosition;
    public Quaternion bowRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        bowPosition = bow.transform.position;
        bowRotation = bow.transform.rotation;
        if (Input.GetKeyDown(KeyCode.A)) {

            Debug.Log("shooot");
            Instantiate(arrow, bowPosition, bowRotation);
            
        }
        

    }
}
