using UnityEngine;
using System.Collections;

public class StampedeMovement : MonoBehaviour {
    public GameObject knight;
    float speed = 4f;
    // Use this for initialization
    void Start () {
        knight = GameObject.Find("Main Camera");
    }
	
	// Update is called once per frame
	void Update () {
        print(knight.transform.position.x - this.transform.position.x);
        print(this.transform.position);
        print(knight.transform.position);
        if ((Mathf.Abs(knight.transform.position.x - this.transform.position.x) > 16f ||
            Mathf.Abs(knight.transform.position.z - this.transform.position.z) > 0.05f))
        {
            
            Vector3 m = Vector3.MoveTowards(transform.position, knight.transform.position, Time.deltaTime * speed);
            m.y = 0;
            transform.position = m;
            speed += 2f;
        } else
        {
            speed -= 2f;
        }
	}

}
