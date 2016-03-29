using UnityEngine;
using System.Collections;

public class Lance : MonoBehaviour {
	bool positionSwitch = true;
	Vector3 position1 = new Vector3 (-2.729998f, -1.100002f, 1.0f);
	Vector3 position2 = new Vector3 (2.3f, -1.1000020f, 1.0f);

	void Start () {		
	}	
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Space)) {
			
			if (positionSwitch) {
				this.transform.position = position2;
				positionSwitch = false;
			}

			else {
				this.transform.position = position1;
			}
		}

	}
}
