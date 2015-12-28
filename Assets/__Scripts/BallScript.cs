using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public GameObject ball;
	public static float bottomY = -20f;


	void Start() {
		// Para las pruebas iniciábamos la bola con fuerza, pero ya no lo queremos,
		// ya que la bola comienza quieta
		//this.GetComponent<Rigidbody> ().AddForce (80, 800f, 0);
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
