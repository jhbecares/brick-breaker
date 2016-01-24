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
				BallScript [] bss = FindObjectsOfType(typeof(BallScript)) as BallScript[];
				foreach (BallScript bs in bss) {
					bs.MaximiseBall ();
				}
			} else if (gameObject.tag == "SmallBallPowerup") {
				BallScript [] bss = FindObjectsOfType(typeof(BallScript)) as BallScript[];
				foreach (BallScript bs in bss) {
					bs.MinimiseBall ();
				}
			} else if (gameObject.tag == "PowerupThreeBalls") {
				foreach (Transform child in gameObject.transform) {
					Destroy (child);
				}
				Picker p = FindObjectOfType (typeof(Picker)) as Picker;
				//p.AddLife ();
				p.InstantiateBalls ();
			} else if (gameObject.tag == "PowerupBlocker" && coll.collider.tag == "Paddle") {
				// Creamos una barrera para que la bola no se caiga
				GameObject GO = Instantiate (blockerGO) as GameObject;
			} else if (gameObject.tag == "PowerupPaddle") {
				// Accedemos al picker y llamamos a la función que cambia el tamaño de los paddle
				Picker p = FindObjectOfType (typeof(Picker)) as Picker;
				p.ChangePaddleSize ();
			} else if (gameObject.tag == "PowerupSpeedFast") {
				// Accedemos a todas las bolas para 
				BallScript [] bss = FindObjectsOfType(typeof(BallScript)) as BallScript[];
				foreach (BallScript bs in bss) {
					bs.IncreaseSpeedBall ();
				}
			} else if (gameObject.tag == "PowerupSpeedSlow") {
				BallScript [] bss = FindObjectsOfType(typeof(BallScript)) as BallScript[];
				foreach (BallScript bs in bss) {
					bs.DecreaseSpeedBall ();
				}
			}

		}
		Destroy (gameObject);
	}

}
