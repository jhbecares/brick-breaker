using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public GameObject ball;
	public static float bottomY = -20f;
	
	float maxSpeed = 11f;
	float minSpeed = 9f;

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

		// Limitamos la velocidad de la bola para que no haga cosas raras,
		// ya que con un mal golpe se puede acelerar
		Vector3 vel = this.gameObject.GetComponent<Rigidbody> ().velocity;
		if (vel.magnitude > maxSpeed) {
			this.gameObject.GetComponent<Rigidbody> ().velocity = this.gameObject.GetComponent<Rigidbody> ().velocity.normalized * maxSpeed;
		}

		if (vel.magnitude < minSpeed) {
			this.gameObject.GetComponent<Rigidbody> ().velocity = this.gameObject.GetComponent<Rigidbody> ().velocity.normalized * minSpeed;
		}
	}
}
