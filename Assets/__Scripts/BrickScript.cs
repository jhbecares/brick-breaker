using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	public GUIText scoreGT;

	public static int score {get;set;}

	// Use this for initialization
	void Start () {
		GameObject scoreGO = GameObject.Find ("ScoreCounter");
		scoreGT = scoreGO.GetComponent<GUIText> ();
		scoreGT.text = "Score: 0";
		PlayerPrefs.SetInt ("Score", 0);

		if (this.tag != "UnbreakableBrick") {
			World.bricks.Add(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision coll) {
		//Debug.Log ("Destroy on collision");

		if (this.tag == "UnbreakableBrick") {
			// No hacemos nada
		} else {
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
}
