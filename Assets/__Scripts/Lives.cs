﻿using UnityEngine;
using System.Collections;

public class Lives : MonoBehaviour {

	public GUIText livesGT;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (livesGT);
	}

	void Awake() {
		//PlayerPrefs.SetInt ("Lives", lives);
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
