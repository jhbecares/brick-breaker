﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpScript : MonoBehaviour {

	public AudioClip powerupHitAudio;

	public GameObject blockerGO;

	//public GameObject ballGO;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision coll) {
		if (coll.collider.tag == "Paddle") {
			AudioSource.PlayClipAtPoint (powerupHitAudio, transform.position);
		

			BallScript ballsc = FindObjectOfType (typeof(BallScript)) as BallScript;
			PaddleScript paddlesc = FindObjectOfType (typeof(PaddleScript)) as PaddleScript;
			if (gameObject.tag == "PowerupLife") {
				Picker p = FindObjectOfType (typeof(Picker)) as Picker;
				p.AddLife ();
			} else if (gameObject.tag == "BigBallPowerup") {
				ballsc.MaximiseBall ();
			} else if (gameObject.tag == "SmallBallPowerup") {
				ballsc.MinimiseBall ();
			} else if (gameObject.tag == "PowerupThreeBalls") {
				foreach (Transform child in gameObject.transform) {
					Destroy (child);
				}

				Picker p = FindObjectOfType (typeof(Picker)) as Picker;
				p.AddLife ();
				p.InstantiateBalls ();
			} else if (gameObject.tag == "PowerupBlocker" && coll.collider.tag == "Paddle") {
				// Creamos una barrera para que la bola no se caiga
				GameObject GO = Instantiate (blockerGO) as GameObject;
			} else if (gameObject.tag == "PowerupPaddle") {

				Picker p = FindObjectOfType (typeof(Picker)) as Picker;
				p.ChangePaddleSize ();
			}

		}
		Destroy (gameObject);
	}

}
