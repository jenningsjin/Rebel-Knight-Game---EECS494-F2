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
            pos1.y = this.transform.position.y - 4f;
            group[0].transform.position = this.transform.position;
            group[0] = GameObject.Instantiate(group[0]);
            transform.parent = group[0].transform;
            pos1.x = pos1.x - 1f;
            pos1.z = pos1.z + 1f;
            pos1.y = this.transform.position.y - 4f;
            group[1].transform.position = pos1;
            group[1] = GameObject.Instantiate(group[1]);
            pos1.x = pos1.x + 3f;
            pos1.y = this.transform.position.y - 4f;
            group[2].transform.position = pos1;
            group[2] = GameObject.Instantiate(group[2]);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.parent = group[1].transform;
        }
	}
}
