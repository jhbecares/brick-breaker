﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpScript : MonoBehaviour {

	public AudioClip powerupHitAudio;

	public GameObject blockerGO;

	public GameObject messagePoints;
	public GameObject messageLife;
	public GameObject messageMagnet;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision coll) {
		if (coll.collider.tag == "Paddle") {
			AudioSource.PlayClipAtPoint (powerupHitAudio, transform.position);
			if (gameObject.tag == "PowerupLife") {
				Picker p = FindObjectOfType (typeof(Picker)) as Picker;
				p.AddLife ();

				GameObject msg = Instantiate (messageLife) as GameObject;
				msg.GetComponent<MessageScript> ().messageLifeBool = true;
			} else if (gameObject.tag == "BigBallPowerup") {
				GameObject[] bss = GameObject.FindGameObjectsWithTag ("Ball");
				foreach (GameObject bs in bss) {
					bs.GetComponent<BallScript>().MaximiseBall ();
				}
			} else if (gameObject.tag == "SmallBallPowerup") {
				GameObject [] bss = GameObject.FindGameObjectsWithTag ("Ball");
				foreach (GameObject bs in bss) {
					bs.GetComponent<BallScript>().MinimiseBall ();
				}
			} else if (gameObject.tag == "PowerupThreeBalls") {
				foreach (Transform child in gameObject.transform) {
					Destroy (child);
				}
				Picker p = FindObjectOfType (typeof(Picker)) as Picker;
				p.InstantiateBalls ();
			} else if (gameObject.tag == "PowerupBlocker" && coll.collider.tag == "Paddle") {
				// Creamos una barrera para que la bola no se caiga
				Instantiate (blockerGO);
			} else if (gameObject.tag == "PowerupPaddle") {
				// Accedemos al picker y llamamos a la función que cambia el tamaño de los paddle
				Picker p = FindObjectOfType (typeof(Picker)) as Picker;
				p.ChangePaddleSize ();
			} else if (gameObject.tag == "PowerupSpeedFast") {
				// Accedemos a todas las bolas aumentar su velocidad 
				GameObject [] bss = GameObject.FindGameObjectsWithTag ("Ball");
				foreach (GameObject bs in bss) {
					bs.GetComponent<BallScript>().IncreaseSpeedBall ();
				}
			} else if (gameObject.tag == "PowerupSpeedSlow") {
				GameObject [] bss = GameObject.FindGameObjectsWithTag ("Ball");
				foreach (GameObject bs in bss) {
					bs.GetComponent<BallScript>().DecreaseSpeedBall ();
				}
			} else if (gameObject.tag == "PowerupPoints") {
				// Sumamos una cantidad de puntos múltiplo de 100 que está entre 100 y 900
				int aux = 100 * Random.Range (1, 9);
				int score = BrickScript.score + aux;
				PlayerPrefs.SetInt ("Score", score);
				BrickScript.score = score;

				// Convert the score back to a string and display it
				BrickScript.scoreGT.text = "Score: " + score.ToString ();

				// Track the high score
				if (score > HighScore.score) {
					HighScore.score = score;
				}
				// Instanciamos el mensaje que nos avisa de que nos han dado puntos
				GameObject msg = Instantiate (messagePoints) as GameObject;
				msg.GetComponent<MessageScript> ().messagePointsBool = true;
				msg.GetComponent<MessageScript> ().ShowMessagePoints (aux);

			} else if (gameObject.tag == "PowerupIman") {
				Picker.magnet = true;
				// Instanciamos el mensaje que nos avisa de que el paddle es un imán
				GameObject msg = Instantiate (messageMagnet) as GameObject;
				msg.GetComponent<MessageScript> ().messageIman = true;
			} 
		}
		Destroy (gameObject);
	}

}
