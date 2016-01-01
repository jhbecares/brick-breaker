﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class World : MonoBehaviour {

	public TextAsset bricksText;
	public string levelNumber = "0";
	public List<GameObject> brickPrefabs;

	public bool _________________;

	public PT_XMLReader xmlr;
	public PT_XMLHashList xml;

	void Awake() {
		xmlr = new PT_XMLReader ();
		xmlr.Parse (bricksText.text);

		xml = xmlr.xml ["xml"] [0] ["level"];

		// Construimos el nivel
		BuildLevel (levelNumber);
	}

	public void BuildLevel(string rnumberstr) {
		PT_XMLHashtable levelHT = null;

		for (int i = 0; i < xml.Count; i++) {
			PT_XMLHashtable ht = xml[i];
			if (ht.att("id") == rnumberstr) {
				levelHT = ht;
				break;
			}
		}
		if (levelHT == null) {
			Debug.LogError("Room not found: " + rnumberstr);
		}
		BuildLevel (levelHT);
	}

	// 	  <brick type="simple" colour="blue" x="-11" y="-2.5" z="0" x="2" scaley="1" scalez="1"/>
	public void BuildLevel(PT_XMLHashtable level) {
		// Número de ladrillos en este nivel
		int numBricks = level ["brick"].Count;

		GameObject GO = null;
		string type, colour;
		float x, y, z, scalex, scaley, scalez;

		for (int i = 0; i < numBricks; i++) {
			PT_XMLHashtable l = level["brick"][i];
			type = l.att("type");
			colour = l.att("colour");
			float.TryParse(l.att("x"), out x);
			float.TryParse(l.att("y"), out y);
			float.TryParse(l.att("z"), out z);
			float.TryParse(l.att("scalex"), out scalex);
			float.TryParse(l.att("scaley"), out scaley);
			float.TryParse(l.att("scalez"), out scalez);

			if (colour == "blue") {
				if (type == "simple") {
					GO = Instantiate(brickPrefabs[0]) as GameObject;
				}
				else if (type == "twoshots") {

				}
				else if (type == "unbreakable") {

				}
			}
			else if (colour == "orange") {
				if (type == "simple") {
					GO = Instantiate(brickPrefabs[1]) as GameObject;
				}
				else if (type == "twoshots") {
					
				}
				else if (type == "unbreakable") {
					
				}
			}
			else if (colour == "yellow") {
				if (type == "simple") {
					GO = Instantiate(brickPrefabs[2]) as GameObject;
				}
				else if (type == "twoshots") {
					
				}
				else if (type == "unbreakable") {
					
				}
			}
			else if (colour == "black") {
				if (type == "simple") {
					GO = Instantiate(brickPrefabs[3]) as GameObject;
				}
				else if (type == "twoshots") {
					
				}
				else if (type == "unbreakable") {
					GO = Instantiate(brickPrefabs[3]) as GameObject;
				}
			}

			GO.transform.position = new Vector3(x, y, z);
			GO.transform.localScale = new Vector3(scalex, scaley, scalez);

		}

	}

}