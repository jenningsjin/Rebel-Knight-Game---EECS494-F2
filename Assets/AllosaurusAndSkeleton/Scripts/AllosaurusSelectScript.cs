using UnityEngine;
using System.Collections;

public class AllosaurusSelectScript : MonoBehaviour {

	public GameObject allosaurusCamera;
	public GameObject[] allosaurus;


	public void AllosaurusSelect(int alloNum){
		AllosaurusCameraScript frogcamerascript=allosaurusCamera.GetComponent<AllosaurusCameraScript>();
		frogcamerascript.target = allosaurus [alloNum];
	}
}
