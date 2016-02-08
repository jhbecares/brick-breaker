﻿using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.S)) {
			Application.LoadLevel("Level0");
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			AudioListener.pause = false;
			AudioListener.volume = 1;
		}
		if (Input.GetKeyDown(KeyCode.M)) {
			AudioListener.pause = true;
			AudioListener.volume = 0;
		}
	}
}