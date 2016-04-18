using UnityEngine;
using System.Collections;

public class LaneMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = MoveKnight.rigid.position;
        pos.x = 0;
        pos.y = 0.5f;
        this.transform.position = pos;
	}
}
