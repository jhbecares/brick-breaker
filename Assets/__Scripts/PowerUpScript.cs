using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpScript : MonoBehaviour {

	public AudioClip powerupHitAudio;

	public GameObject ballGO;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision coll) {
		AudioSource.PlayClipAtPoint (powerupHitAudio, transform.position);

		BallScript ballsc = FindObjectOfType(typeof(BallScript)) as BallScript;
		if (gameObject.tag == "PowerupLife") {
			Debug.Log("POweruplife");
			Picker p = FindObjectOfType (typeof(Picker)) as Picker;
			p.AddLife ();
		} else if (gameObject.tag == "BigBallPowerup") {
			ballsc.MaximiseBall ();
		} else if (gameObject.tag == "SmallBallPowerup") {
			ballsc.MinimiseBall ();
		} else if (gameObject.tag == "PowerupThreeBalls") {
			foreach (Transform child in gameObject.transform) {
				Destroy (child);
			}


			Picker p = FindObjectOfType (typeof(Picker)) as Picker;
			p.AddLife();
			/*p.AddLife();
			p.AddLife();*/
			InstantiateNewBalls();
		}
		Destroy (gameObject);
	}


	void InstantiateNewBalls() {
		BallScript [] ballsc = FindObjectsOfType(typeof(BallScript)) as BallScript[];
		foreach (BallScript ball in ballsc) {
			GameObject go = Instantiate(ballGO) as GameObject;
			go.GetComponent<Rigidbody>().isKinematic = false;
			go.transform.position = ball.transform.position;
			go.GetComponent<Rigidbody> ().AddForce(300f*Input.GetAxis("Horizontal"), 300f, 0);
			go.GetComponent<Rigidbody>().velocity = ball.GetComponent<Rigidbody>().velocity;
			/*
			go = Instantiate(ballGO) as GameObject;
			go.GetComponent<Rigidbody>().isKinematic = false;
			go.transform.position = ball.transform.position;
			go.GetComponent<Rigidbody> ().AddForce(300f*Input.GetAxis("Horizontal"), 500f, 0);
			go.GetComponent<Rigidbody>().velocity = ball.GetComponent<Rigidbody>().velocity;

			go = Instantiate(ballGO) as GameObject;
			go.GetComponent<Rigidbody>().isKinematic = false;
			go.transform.position = ball.transform.position;
			go.GetComponent<Rigidbody> ().AddForce(300f*Input.GetAxis("Horizontal"), 700f, 0);
			go.GetComponent<Rigidbody>().velocity = ball.GetComponent<Rigidbody>().velocity;*/
		}
	}
}
