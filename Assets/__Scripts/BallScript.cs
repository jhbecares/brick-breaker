using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public GameObject ball;
	public static float bottomY = -20f;

	public static int countSizeBall = 0;
	int maxCount = 500;
	public static bool sizeBallNormal { get; set;}
	public static bool sizeBallBig { get; set; }
	public static bool sizeBallSmall { get; set; }

	float defaultMaxSpeed = 11f;
	float defaultMinSpeed = 9f;
	float maxSpeed = 11f;
	float minSpeed = 9f;




	void Start() {
		sizeBallNormal = true;
		sizeBallBig = false;
		sizeBallSmall = false;
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

	void LateUpdate() {
		if (sizeBallBig == true || sizeBallSmall == true) {
			if (countSizeBall < maxCount) {
				countSizeBall++;
			}
			else if (countSizeBall == maxCount) {
				NormaliseBall();
				countSizeBall = 0;
			}
		}
	}

	
	public void MinimiseBall() {
		// volvemos a empezar de 0 para el máximo tiempo del powerup
		countSizeBall = 0;

		// le cambiamos la escala a una más pequeña
		gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

		// Guardamos con qué tamaño de bola estamos jugando
		sizeBallSmall = true;
		sizeBallNormal = false;
		sizeBallBig = false;
	}
	
	public void MaximiseBall() {
		countSizeBall = 0;
		BallScript ballsc = FindObjectOfType(typeof(BallScript)) as BallScript;
		ballsc.ball.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		sizeBallBig = true;
		sizeBallNormal = false;
		sizeBallSmall = false;
	}
	
	public void NormaliseBall() {
		BallScript ballsc = FindObjectOfType(typeof(BallScript)) as BallScript;
		ballsc.ball.transform.localScale = new Vector3(1f, 1f, 1f);
		sizeBallNormal = true;
		sizeBallBig = false;
		sizeBallSmall = false;
	}
}
