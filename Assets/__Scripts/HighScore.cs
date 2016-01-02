using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	static public int score = 0;

	// Use this for initialization
	void Start () {
	}

	void Awake() {
		// If the ApplePickerHighScore already exists, read it
		if (PlayerPrefs.HasKey ("BrikBreakerHighScore")) {
			score = PlayerPrefs.GetInt("BrikBreakerHighScore");
		}

		// Assign the high score to ApplePickerHighScore
		PlayerPrefs.SetInt ("BrikBreakerHighScore", score);
	}
	
	// Update is called once per frame
	void Update () {
		GUIText gt = this.GetComponent<GUIText> ();
		gt.text = "High Score: " + score;

		// Update ApplePickerHighScore in PlayerPrefs if necessary
		if (score > PlayerPrefs.GetInt ("BrikBreakerHighScore")) {
			PlayerPrefs.SetInt("BrikBreakerHighScore", score);
		}
	}
}
