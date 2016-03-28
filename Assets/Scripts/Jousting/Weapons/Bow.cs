using UnityEngine;
using System.Collections;

public class Bow : MonoBehaviour {

    public GameObject arrow;
    public GameObject bow;
    public Vector3 bowPosition;
    public Quaternion bowRotation;
    public bool canShoot = true;

    public float reloadTime = 0.5f;
    public float timerCounter;

    // Use this for initialization
    void Start () {
        timerCounter = reloadTime;
	}
	
	// Update is called once per frame
	void Update () {
        bowPosition = bow.transform.position;
        bowRotation = bow.transform.rotation;
        timerCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && timerCounter < 0) {
            ShootArrow();
        }            

    }

    void ShootArrow() {
        Debug.Log("shooot");
        Instantiate(arrow, bowPosition, bowRotation);
        timerCounter = reloadTime;

    }
}
