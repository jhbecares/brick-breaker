using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Picker : MonoBehaviour {

	public GameObject paddlePrefab;
	public GameObject ballPrefab;
	public GameObject attachedBall = null;

	static int times = 0;

	public static int numBaskets = 3;
	public float paddleBottomY = -14f;
	public float paddleSpacingY = 2f;
	public float ballBottomY = -6f;
	public List<GameObject> paddleList;
	public static int maxLives {get;set;}

	void Start () {
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Highscore"));
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Score"));
		DontDestroyOnLoad (GameObject.FindGameObjectWithTag ("Lives")); 

		maxLives = 3;

		if (times == 0) {
			PlayerPrefs.SetInt ("Score", 0);
			PlayerPrefs.SetInt ("Lives", numBaskets);
			times++;

			// Instanciar la nueva bola
			Lives.lives = numBaskets;
			
			Vector3 pos = Vector3.zero;
			paddleList = new List<GameObject>();
			for(int i= 0; i<numBaskets; i++) {
				GameObject tBasketGO = Instantiate ( paddlePrefab ) as GameObject;
				pos = Vector3.zero;
				pos.y = paddleBottomY + ( paddleSpacingY * i );
				tBasketGO.transform.position = pos;
				paddleList.Add(tBasketGO);
			}
			SpawnBall ();

		} else {
			Lives.lives = PlayerPrefs.GetInt("Lives");

			Vector3 pos = Vector3.zero;
			paddleList = new List<GameObject>();
			for(int i= 0; i<Lives.lives && i < maxLives; i++) {
				GameObject tBasketGO = Instantiate ( paddlePrefab ) as GameObject;
				pos = Vector3.zero;
				pos.y = paddleBottomY + ( paddleSpacingY * i );
				tBasketGO.transform.position = pos;
				paddleList.Add(tBasketGO);
			}
			SpawnBall ();
		}


	}

	void SpawnBall() {
		if (ballPrefab == null) {
			Debug.Log ("El prefab es null");
			return;
		}
		attachedBall = Instantiate ( ballPrefab ) as GameObject; 

	}

	// Usamos fixed update en lugar de update para que no haya lag en nuestra bola al moverla con el paddle
	void FixedUpdate() {

		// Comprobamos si estamos pulsando espacio para lanzar la bola
		if (attachedBall) {
			Rigidbody ballRB = attachedBall.GetComponent<Rigidbody> ();
			ballRB.position = paddleList[paddleList.Count-1].transform.position + new Vector3(0, 0.75f, 0);

			if (Input.GetButtonDown ("Jump")) {
				ballRB.isKinematic = false;
				//this.GetComponent<Rigidbody> ().AddForce (80, 800f, 0);
				ballRB.AddForce(300f*Input.GetAxis("Horizontal"), 500f, 0);
				attachedBall = null;
			}
		}
	}

	// Eliminamos uno de los paddles y actualizamos las vidas
	public void DestroyPaddles() {
		while (paddleList.Count > Lives.lives) {
			int paddleIndex = paddleList.Count - 1;
			Lives.lives = paddleIndex;
		
			// cogemos la referencia al paddle
			GameObject tPaddleGO = paddleList [paddleIndex];
			// y borramos el objeto
			paddleList.RemoveAt (paddleIndex);
			Destroy (tPaddleGO);
		}
	}


	// Crea un nuevo paddle cuando nos dan una vida más
	// y actualiza los atributos correspondientes
	public void AddLife() {
		if (Lives.lives+1 < maxLives) {
			int numB = paddleList.Count;
			GameObject tBasketGO = Instantiate (paddlePrefab) as GameObject;
			Vector3 pos = new Vector3 (paddleList [numB - 1].transform.position.x, paddleBottomY + (paddleSpacingY * numB), 0);
			tBasketGO.transform.position = pos;
			paddleList.Add (tBasketGO);
		}

		Lives.lives++;
		PlayerPrefs.SetInt ("Lives", Lives.lives);
	}

	public void InstantiateBalls() {
		BallScript currentBall = FindObjectOfType(typeof(BallScript)) as BallScript;
		//foreach (BallScript ball in ballsc) {
			GameObject go = Instantiate(ballPrefab) as GameObject;
			go.GetComponent<Rigidbody>().isKinematic = false;
			go.transform.position = currentBall.transform.position;
			go.GetComponent<Rigidbody> ().AddForce(300f*Input.GetAxis("Horizontal"), 300f, 0);
			go.GetComponent<Rigidbody>().velocity = currentBall.GetComponent<Rigidbody>().velocity;
	}

	public void BallDestroyed() {

		int livesOld = Lives.lives;

		if (Lives.lives > paddleList.Count) {
			// estamos jugando con más de una bola, y por tanto lo único que tenemos que hacer es restar
			Lives.lives--;
			PlayerPrefs.SetInt ("Lives", Lives.lives);
			// Si hay mas de una bola, no hacemos el spawn. 
			BallScript [] ballsc = FindObjectsOfType(typeof(BallScript)) as BallScript[];
			if (ballsc.Length <= 1)
				SpawnBall();
		} else if (Lives.lives <= maxLives && Lives.lives > 1 && Lives.lives == paddleList.Count) {
			// Las vidas y los paddles son iguales, por tanto hay que destruir uno de los paddle
			int paddleIndex = paddleList.Count - 1;
			// cogemos la referencia al paddle
			GameObject tPaddleGO = paddleList [paddleIndex];
			// y borramos el objeto
			paddleList.RemoveAt (paddleIndex);
			Destroy (tPaddleGO);
			Lives.lives--;
			PlayerPrefs.SetInt ("Lives", Lives.lives);

			// Comprobamos que a pesar de eliminar un paddle no tenemos sólo una bola, en cuyo caso
			// la volvemos a lanzar
			BallScript [] ballsc = FindObjectsOfType(typeof(BallScript)) as BallScript[];
			if (ballsc.Length < 2) {
				SpawnBall ();
			}
		} 
		// Si se nos acaban las vidas, volvemos a empezar el juego
		else if (Lives.lives == 1) {
			BrickScript.score = 0; // reseteamos la puntuación

			times = 0;
			Destroy (GameObject.FindGameObjectWithTag ("Highscore"));
			Destroy (GameObject.FindGameObjectWithTag ("Score"));
			Destroy (GameObject.FindGameObjectWithTag ("Lives")); 
			Application.LoadLevel ("GameOver");

			Destroy (gameObject);
		} else {
			// En cualquier otro caso (no debería llegar aquí) simplemente restamos
			// y volvemos a lanzar la bola
			Lives.lives--;
			PlayerPrefs.SetInt ("Lives", Lives.lives);
			SpawnBall ();
		}
	}

}
