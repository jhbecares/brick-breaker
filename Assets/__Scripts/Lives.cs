using UnityEngine;
using System.Collections;

public class Lives : MonoBehaviour {

	public GUIText livesGT;
	static int times = 0;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (livesGT);
	}

	void Awake() {
		// Assign the high score to ApplePickerHighScore
		PlayerPrefs.SetInt ("Lives", lives);
	}
	
	// Update is called once per frame
	void Update () {
		GameObject scoreGO = GameObject.Find ("Lives");
		livesGT = scoreGO.GetComponent<GUIText> ();
		lives = PlayerPrefs.GetInt("Lives");
		livesGT.text = "Lives: " + lives;
	}

	public static int lives {get;set;}
}
