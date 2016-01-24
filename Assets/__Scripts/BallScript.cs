using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public GameObject ball;
	public static float bottomY = -20f;

	public static int countSizeBall = 0;
	int maxCount = 500;
	// Booleanos para controlar el tamaño de la bola
	public static bool sizeBallNormal { get; set;}
	public static bool sizeBallBig { get; set; }
	public static bool sizeBallSmall { get; set; }

	// Constantes que regulan en cuánto va a aumentar o disminuir la velocidad de la bola
	float speedIncreaseConst = 1.25f;
	float speedDecreaseConst = 0.75f;

	// Control de los límites de velocidad de la bola
	float defaultMaxSpeed = 11f;
	float defaultMinSpeed = 9f;
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

	// Si la bola no tiene el tamaño normalizado, sumamos 1 a la cuenta
	// Si la bola ya ha estado el suficiente tiempo con el tamaño cambiado, 
	// reseteamos el contador y normalizamos el tamaño
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

	// Hace pequeña la bola (la mitad)
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

	// Maximiza el tamaño de la bola
	public void MaximiseBall() {
		countSizeBall = 0;
		BallScript ballsc = FindObjectOfType(typeof(BallScript)) as BallScript;
		ballsc.ball.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		sizeBallBig = true;
		sizeBallNormal = false;
		sizeBallSmall = false;
	}

	// Normaliza el tamaño de la bola para que vuelva a ser como al principio
	public void NormaliseBall() {
		BallScript ballsc = FindObjectOfType(typeof(BallScript)) as BallScript;
		ballsc.ball.transform.localScale = new Vector3(1f, 1f, 1f);
		sizeBallNormal = true;
		sizeBallBig = false;
		sizeBallSmall = false;
	}

	public void IncreaseSpeedBall() {
		maxSpeed = maxSpeed * speedIncreaseConst;
		ball.gameObject.GetComponent<Rigidbody> ().velocity = ball.gameObject.GetComponent<Rigidbody> ().velocity * speedIncreaseConst; 
	}

	public void DecreaseSpeedBall() {
		minSpeed = minSpeed * speedDecreaseConst;
		ball.gameObject.GetComponent<Rigidbody> ().velocity = ball.gameObject.GetComponent<Rigidbody> ().velocity * speedDecreaseConst; 
	}
}
