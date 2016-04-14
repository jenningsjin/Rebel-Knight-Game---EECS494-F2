using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallExplode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Explode() {
        List<Rigidbody> explosionList = new List<Rigidbody>();
        foreach (Transform child in transform) {
            if (child.gameObject.GetComponent<Rigidbody>() != null)
                explosionList.Add(child.gameObject.GetComponent<Rigidbody>());
        }
        foreach (Rigidbody child in explosionList) {
            child.transform.SetParent(null);
            Vector3 temp = new Vector3(Random.Range(-2, 2), 1, Random.Range(0.5f, 3));
            //child.gameObject.AddComponent<Rigidbody>().AddForce(temp * Random.Range(10, 20), ForceMode.Impulse);

            child.useGravity = true;
            child.AddForce(temp * Random.Range(5, 10), ForceMode.Impulse);
            child.AddTorque(new Vector3(1, 0, 0) * Random.Range(1,30), ForceMode.Impulse);
        }
    }
}
