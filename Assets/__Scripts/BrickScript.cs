using UnityEngine;
using System.Collections;

public class BrickScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision coll) {
		Debug.Log ("Destroy on collision");
		Destroy (gameObject);
	}
}
