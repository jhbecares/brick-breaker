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

		maxLives = 4;

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
			for(int i= 0; i<Lives.lives; i++) {
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
		// Posición de la bola: lo colocamos una unidad más arriba que el paddle
		//Vector3 ballPos = paddleList[paddleList.Count-1].transform.position + new Vector3(0, 0.75f, 0);

		// Rotación inicial de la bola - dummy
		//Quaternion ballRot = Quaternion.identity;

		//attachedBall = Instantiate ( ballPrefab, ballPos, ballRot ) as GameObject;
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
		//if (paddleList.Count < maxLives) {
			//int paddleIndex = paddleList.Count + 1;
			//Lives.lives = paddleIndex;

		Debug.Log (Lives.lives);
			if (Lives.lives < 4) {
				int numB = paddleList.Count;
				GameObject tBasketGO = Instantiate (paddlePrefab) as GameObject;
				Vector3 pos = new Vector3 (paddleList [numB - 1].transform.position.x, paddleBottomY + (paddleSpacingY * numB), 0);
				tBasketGO.transform.position = pos;
				paddleList.Add (tBasketGO);
			}

			Lives.lives++;
			PlayerPrefs.SetInt ("Lives", Lives.lives);
		//}
	}


	public void BallDestroyed() {
		//// Destruimos uno de los paddles
		// cogemos el índice de nuestra lista de paddles

		if (Lives.lives <= 4 && Lives.lives > 1) {
			int paddleIndex = paddleList.Count - 1;
			// cogemos la referencia al paddle
			GameObject tPaddleGO = paddleList [paddleIndex];
			// y borramos el objeto
			paddleList.RemoveAt (paddleIndex);
			Destroy (tPaddleGO);
		} 

		Lives.lives--;
		PlayerPrefs.SetInt ("Lives", Lives.lives);


		// Si se nos acaban las vidas, volvemos a empezar el juego
		if (Lives.lives == 0 ) {
			BrickScript.score = 0; // reseteamos la puntuación

			times = 0;
			Destroy (GameObject.FindGameObjectWithTag ("Highscore"));
			Destroy (GameObject.FindGameObjectWithTag ("Score"));
			Destroy (GameObject.FindGameObjectWithTag ("Lives")); 
			Application.LoadLevel ("GameOver");

			Destroy (gameObject);
		} 
		BallScript [] ballsc = FindObjectsOfType(typeof(BallScript)) as BallScript[];
		if (ballsc.Length == 0) {
			SpawnBall();
		}
	}

}
