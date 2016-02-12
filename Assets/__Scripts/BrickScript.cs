using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrickScript : MonoBehaviour {

	public static GUIText scoreGT;

	public static int score {get;set;}

	public GameObject powerupLife;
	public List<GameObject> powerup;

	public float lifePowerUpFreq = 0.1f;
	public float powerUpFreq = 0.3f;

	float radius = 2.0f;

	float transformValue = 0.51f;

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
		} else if (this.tag == "BrickBomb") {
			if (coll.gameObject.tag == "Ball") {
				// Explotamos los ladrillos que haya en un radio determinado

				Collider[] colliders = Physics.OverlapSphere (coll.gameObject.transform.position, radius);
				Destroy (gameObject);
				score += 200;
				foreach (Collider col in colliders) {
					if (col.tag == "Brick") {
						Destroy (col.gameObject);
						score += 200;
					}
				}
				PlayerPrefs.SetInt ("Score", score);
				// Convert the score back to a string and display it
				scoreGT.text = "Score: " + score.ToString ();

				// Track the high score
				if (score > HighScore.score) {
					HighScore.score = score;
				}
			}
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
		
		if (Random.value < powerUpFreq) {
			int poweruptype = Random.Range(0, powerup.Count);
			GameObject go = Instantiate(powerup[poweruptype]) as GameObject;
			go.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - transformValue);
		}
		else if (Random.value < lifePowerUpFreq) {
			GameObject go = Instantiate (powerupLife) as GameObject;
			go.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - transformValue);
		}  
	}
}
