using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpScript : MonoBehaviour {

	public AudioClip powerupHitAudio;

	public GameObject blockerGO;

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
			} else if (gameObject.tag == "PowerupIman") {
				Picker.magnet = true;
			} else if (gameObject.tag == "PowerupBomb") {
				Debug.Log ("Powerup bomb");
			}

		}
		Destroy (gameObject);
	}

}
