using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrickScript : MonoBehaviour {

	public GUIText scoreGT;

	public static int score {get;set;}

	public GameObject powerupLife;
	public List<GameObject> powerup;

	// Use this for initialization
	void Start () {
		GameObject scoreGO = GameObject.Find ("ScoreCounter");
		scoreGT = scoreGO.GetComponent<GUIText> ();

		int sc = PlayerPrefs.GetInt ("Score");
		if (sc == 0) {
			scoreGT.text = "Score: 0";
			PlayerPrefs.SetInt ("Score", 0);
		} else {
			scoreGT.text = "Score: " + sc;
		}

		if (this.tag != "UnbreakableBrick") {
			World.bricks.Add(this);
		}

		DontDestroyOnLoad (GameObject.Find ("score"));
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision coll) {
		//Debug.Log ("Destroy on collision");

		if (this.tag == "UnbreakableBrick") {
			// No hacemos nada
		} else {
			// Creamos un powerup de forma aleatoria

			CreatePowerUp();
			Destroy (gameObject);

			// Parse the text of the scoreGT into an int
			//int score = int.Parse (scoreGT.text);
			
			// Add points for breaking the brick
			score += 100;
			PlayerPrefs.SetInt ("Score", score);
			
			// Convert the score back to a string and display it
			scoreGT.text = "Score: " + score.ToString ();
			
			// Track the high score
			if (score > HighScore.score) {
				HighScore.score = score;
			}

			// Find out what hit this basket
			GameObject collidedWith = coll.gameObject;
			if (collidedWith.tag == "Ball") {
				World.bricks.Remove(this);
				Destroy (this);
			}
		}

	}

	void CreatePowerUp() {
		int num = Random.Range (0, 2000);
		// hacemos que solo se creen vidas cuando el random es módulo 5, para
		// que no haya demasiadas
		if (num % 5 == 0 && Lives.lives < Picker.maxLives) {
			GameObject go = Instantiate (powerupLife) as GameObject;
			go.transform.position = gameObject.transform.position;
		} else if (num % 3 == 0) {
			// creamos cualquier otro powerup si es módulo 2
			int poweruptype = Random.Range(0, powerup.Count);
			GameObject go = Instantiate(powerup[poweruptype]) as GameObject;
			go.transform.position = gameObject.transform.position;
		} else {
			// en caso contrario no hacemos nada
		}
	}
}
