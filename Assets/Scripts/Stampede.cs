using UnityEngine;
using System.Collections;

public class Stampede : MonoBehaviour {
    public GameObject[] group;
    public int size;
	// Use this for initialization
	void Start () {
        if(size == 3)
        {
            Vector3 pos1 = this.transform.position;
            pos1.z -= 4f;
            group[0].transform.position = this.transform.position;
            group[0] = GameObject.Instantiate(group[0]);
            pos1.x = pos1.x - 1f;
            pos1.z = pos1.z + 1f;
            group[1].transform.position = pos1;
            group[1] = GameObject.Instantiate(group[1]);
            pos1.x = pos1.x + 3f;
            group[2].transform.position = pos1;
            group[2] = GameObject.Instantiate(group[2]);
            transform.parent = group[0].transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
