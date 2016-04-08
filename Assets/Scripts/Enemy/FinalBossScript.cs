using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	int state = 0;
	delegate void Attack();
	Attack attackChoice;
	Attack [] listOfGroundAttacks;
	Attack [] listOfAirAttacks;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case 0: // Moving on the ground
			break;

		case 1: // Flying overhead
			break;

		case 2: // Select Attack
			break;

		case 3: // Broadcast choice and attack
			break;

		default:
			break;
		}
	}

	// Attack choices
	void ShootFireball() {

	}

	void CreateWallOfFlames() {

	}

	void CreatePillarOfFire() {

	}

	void AirStrike() {

	}

	void MeteorShower() {

	}

}
