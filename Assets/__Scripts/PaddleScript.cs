using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {

	public float paddleSpeed = 20f;
	public Bounds bounds;

	// Update is called once per frame
	void Update () {
		/*// Get the current screen position of the mouse from Input
		Vector3 mousePos2D = Input.mousePosition;

		// The Camera's z position sets the how far to push the mouse into 3D
		mousePos2D.z = -Camera.main.transform.position.z;

		// Convert the point from 2D screen space into 3D game world space
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);

		// Move the x position of this Basket to the x position of the mouse
		Vector3 pos = this.transform.position;
		pos.x = mousePos3D.x;
		this.transform.position = pos;*/

		// Comprobamos si estamos moviendo el paddle a la derecha o a la izquierda
		// Izquierda
		if (Input.GetAxis ("Horizontal") < 0) {
			//Debug.Log("LEFT");
			// Nos movemos a la izquierda
			transform.Translate (-paddleSpeed * Time.deltaTime, 0, 0);
		}
		// Derecha
		if (Input.GetAxis ("Horizontal") > 0) {
			//Debug.Log("RIGHT");
			// Nos movemos a la derecha
			transform.Translate (paddleSpeed* Time.deltaTime, 0, 0);
		}

		// comprobamos que no nos salimos de la pantalla
		Vector3 pos = transform.position;

		bounds.center = transform.position;
		Vector3 off = Utils.ScreenBoundsCheck (bounds, BoundsTest.onScreen);
		if (off != Vector3.zero) {
			pos -= off;
			transform.position = pos;
		}
	}

	void OnCollisionEnter(Collision coll) {
		foreach (ContactPoint contact in coll.contacts) {
			// Queremos saber el punto de contacto con nuestro paddle
			// para poder modificar la dirección de la bola
			if (contact.thisCollider == GetComponent<Collider>()) {
				// Esto corresponde al punto de contacto del paddle
				float english = contact.point.x - transform.position.x;
				contact.otherCollider.GetComponent<Rigidbody>().AddForce(200f * english, 0, 0);

			}
		}

		// Find out what hit this basket
		/*GameObject collidedWith = coll.gameObject;
		if (collidedWith.tag == "Ball") {
			Destroy(collidedWith);
		}

		// Parse the text of the scoreGT into an int
		int score = int.Parse (scoreGT.text);

		// Add points for catching the apple
		score += 100;

		// Convert the score back to a string and display it
		scoreGT.text = score.ToString ();

		// Track the high score
		if (score > HighScore.score) {
			HighScore.score = score;
		}*/
	}
}
