using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public GameObject ball;
	public static float bottomY = -20f;


	void Start() {
		//Rigidbody.AddForce (0, 1000f, 0);
		this.GetComponent<Rigidbody> ().AddForce (80, 800f, 0);
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < bottomY) {
			// Eliminamos la pelota si se nos cae
			Destroy (this.gameObject);

			// Además, eliminamos uno de los paddles
			Picker pickerScript = Camera.main.GetComponent<Picker>();
			pickerScript.BallDestroyed();
		}
	}
}
