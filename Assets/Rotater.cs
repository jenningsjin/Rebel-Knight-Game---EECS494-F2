using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour {
	public float rotationSpeed;
	public Vector3 rotVector;

	// Update is called once per frame
	void Update () {
		this.transform.Rotate(rotVector * Time.deltaTime * rotationSpeed);
	}
}
