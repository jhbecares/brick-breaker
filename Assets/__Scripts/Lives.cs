using UnityEngine;
using System.Collections;

public class Lives : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}

	void Awake() {
		// Assign the high score to ApplePickerHighScore
		PlayerPrefs.SetInt ("Lives", lives);
	}
	
	// Update is called once per frame
	void Update () {
		GUIText gt = this.GetComponent<GUIText> ();
		gt.text = "Remaining lives: " + lives;
	}

	public static int lives {get;set;}
}
