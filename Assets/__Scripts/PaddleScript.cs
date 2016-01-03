using UnityEngine;
using System.Collections;

public class PaddleScript : MonoBehaviour {

	public float paddleSpeed = 25f;
	public Bounds bounds;

	// Update is called once per frame
	void Update () {

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
		if (coll.gameObject.tag == "Ball") {
			foreach (ContactPoint contact in coll.contacts) {
				// Queremos saber el punto de contacto con nuestro paddle
				// para poder modificar la dirección de la bola
				if (contact.thisCollider == GetComponent<Collider> ()) {
					// Esto corresponde al punto de contacto del paddle
					float english = contact.point.x - transform.position.x;
					contact.otherCollider.GetComponent<Rigidbody> ().AddForce (200f * english, 0, 0);

				}
			}
		} 
	}
}
